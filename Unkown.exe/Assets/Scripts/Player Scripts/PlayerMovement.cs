using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D _rb;
    private Animator _anim;

    [Header("Animation Strings")]
    const string Walk = "Run";
    const string Idle = "Idle";
    public string slidingAnimation = "Slide";
    public string currentState;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private LayerMask _cornerCorrectLayer;

    [Header("Movement Variables")]
    [SerializeField] private float _movementAcceleration = 70f;
    [SerializeField] private float _maxMoveSpeed = 12f;
    [SerializeField] public float _groundLinearDrag = 7f;

    [SerializeField] public float _horizontalDirection;
    private float _verticalDirection;
    private bool _changingDirection => (_rb.velocity.x > 0f && _horizontalDirection < 0f) || (_rb.velocity.x < 0f && _horizontalDirection > 0f);
    public bool _facingRight = true;

    [Header("Jump Variables")]
    [SerializeField] private float _jumpForce = 12f;
    [SerializeField] private float _airLinearDrag = 2.5f;
    [SerializeField] private float _fallMultiplier = 8f;
    [SerializeField] private float _lowJumpFallMultiplier = 5f;
    [SerializeField] private float _downMultiplier = 12f;
    [SerializeField] private float _hangTime = .1f;

    [Header("Enemy Interaction")]
    [SerializeField] private List<GameObject> nearbyFlies = new List<GameObject>();
    public GameObject nearestFly;

    [Header("Player Energy")]
    [SerializeField] private const int MaxPlayerEnergy = 100;
    [SerializeField] public int PlayerEnergy = 0;
    public bool playerHasMax = false;
    public bool playerHasMin = true;
    [SerializeField] private int lostEnergyPerSec = 1;
    [SerializeField] private const int minEnergy = 0;
    [SerializeField] int flyEnergyRecover = 5;
    private float _hangTimeCounter;


    [Header("Player Shooting")]
    public GameObject Lazer;
    


    [Header("Sliding")]
    public BoxCollider2D normalCollider;
    public BoxCollider2D slideCollider;
    public float slideSpeed = 5f;
    private bool isSliding;

    [Header("Ground Collision Variables")]
    [SerializeField] private float _groundRaycastLength;
    [SerializeField] private Vector3 _groundRaycastOffset;
    private bool _onGround;

    [Header("Wall Collision Variables")]
    [SerializeField] private float _wallRaycastLength;
    private bool _onWall = false;
    private bool _onRightWall = false;

    [Header("Corner Correction Variables")]
    [SerializeField] private float _topRaycastLength;
    [SerializeField] private Vector3 _edgeRaycastOffset;
    [SerializeField] private Vector3 _innerRaycastOffset;
    private bool _canCornerCorrect = false;

    [Header("Slide")]
    [SerializeField] private float _slideMultiplier;

    private bool isEating = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    public string GetCurrentState()
    {
        return this.currentState;
    }

    private void Update()
    {
        _horizontalDirection = GetInput().x;
        _verticalDirection = GetInput().y;
        Animation();
        AssignNearstFly(_rb.transform.position, nearbyFlies);
        EnergyControl();
   
    }


    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        _anim.Play(newState);
    }

    private void FixedUpdate()
    {
        CheckCollisions();
        if (!isSliding)
        {
            MoveCharacter();
        }


        if (_onGround)
        {
            ApplyGroundLinearDrag();
            _hangTimeCounter = _hangTime;
        }
        else
        {
            ApplyAirLinearDrag();
            FallMultiplier();
            _hangTimeCounter -= Time.fixedDeltaTime;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSliding = true;
            performSlide();

        }
        else
        {
            isSliding = false;
            slideCollider.enabled = false;
            normalCollider.enabled = true;
            _groundLinearDrag = 4f;
        }
        if (Input.GetKeyDown("space"))
        {
            EatFly();
        }
        if (_canCornerCorrect) CornerCorrect(_rb.velocity.y);
    }
    #region Input
    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
    #endregion
    private void MoveCharacter()
    {
        _rb.AddForce(new Vector2(_horizontalDirection, 0f) * _movementAcceleration);

        if (Mathf.Abs(_rb.velocity.x) > _maxMoveSpeed)
            _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x) * _maxMoveSpeed, _rb.velocity.y);
    }

    private void ApplyGroundLinearDrag()
    {
        if (Mathf.Abs(_horizontalDirection) < 0.4f || _changingDirection)
        {
            _rb.drag = _groundLinearDrag;
        }
        else
        {
            _rb.drag = 0f;
        }
    }

    private void ApplyAirLinearDrag()
    {
        _rb.drag = _airLinearDrag;
    }

    private void FallMultiplier()
    {
        if (_verticalDirection < 0f)
        {
            _rb.gravityScale = _downMultiplier;
        }
        else
        {
            if (_rb.velocity.y < 0)
            {
                _rb.gravityScale = _fallMultiplier;
            }
            else if (_rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                _rb.gravityScale = _lowJumpFallMultiplier;
            }
            else
            {
                _rb.gravityScale = 1f;
            }
        }
    }

    private void performSlide()
    {
        normalCollider.enabled = false;
        slideCollider.enabled = true;

        if (!_facingRight)
        {
            _rb.AddForce(new Vector2(_horizontalDirection, 0f) * slideSpeed * Time.deltaTime, ForceMode2D.Impulse);
            _groundLinearDrag = 1f;
        }
        else
        {
            _rb.AddForce(new Vector2(_horizontalDirection, 0f) * slideSpeed * Time.deltaTime, ForceMode2D.Impulse);
            _groundLinearDrag = 1f;
        }


    }

    void Flip()
    {
        _facingRight = !_facingRight;
        transform.Rotate(0f, 180f, 0f);
    }


    void Animation()
    {

        if ((_horizontalDirection < 0f && _facingRight || _horizontalDirection > 0f && !_facingRight))
        {
            Flip();
        }
        if (_horizontalDirection > 0 && !isSliding)
        {
            ChangeAnimationState(Walk);
        }
        else if (_horizontalDirection < 0 && !isSliding)
        {
            ChangeAnimationState(Walk);
        }
        else if (isSliding)
        {
            ChangeAnimationState(slidingAnimation);
        }
        else
        {
            ChangeAnimationState(Idle);
        }


    }

    void CornerCorrect(float Yvelocity)
    {
        //Push player to the right
        RaycastHit2D _hit = Physics2D.Raycast(transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength, Vector3.left, _topRaycastLength, _cornerCorrectLayer);
        if (_hit.collider != null)
        {
            float _newPos = Vector3.Distance(new Vector3(_hit.point.x, transform.position.y, 0f) + Vector3.up * _topRaycastLength,
                transform.position - _edgeRaycastOffset + Vector3.up * _topRaycastLength);
            transform.position = new Vector3(transform.position.x + _newPos, transform.position.y, transform.position.z);
            _rb.velocity = new Vector2(_rb.velocity.x, Yvelocity);
            return;
        }

        //Push player to the left
        _hit = Physics2D.Raycast(transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLength, Vector3.right, _topRaycastLength, _cornerCorrectLayer);
        if (_hit.collider != null)
        {
            float _newPos = Vector3.Distance(new Vector3(_hit.point.x, transform.position.y, 0f) + Vector3.up * _topRaycastLength,
                transform.position + _edgeRaycastOffset + Vector3.up * _topRaycastLength);
            transform.position = new Vector3(transform.position.x - _newPos, transform.position.y, transform.position.z);
            _rb.velocity = new Vector2(_rb.velocity.x, Yvelocity);
        }
    }

    private void CheckCollisions()
    {
        //Ground Collisions
        _onGround = Physics2D.Raycast(transform.position + _groundRaycastOffset, Vector2.down, _groundRaycastLength, _groundLayer) ||
                    Physics2D.Raycast(transform.position - _groundRaycastOffset, Vector2.down, _groundRaycastLength, _groundLayer);

        //Corner Collisions
        _canCornerCorrect = Physics2D.Raycast(transform.position + _edgeRaycastOffset, Vector2.up, _topRaycastLength, _cornerCorrectLayer) &&
                            !Physics2D.Raycast(transform.position + _innerRaycastOffset, Vector2.up, _topRaycastLength, _cornerCorrectLayer) ||
                            Physics2D.Raycast(transform.position - _edgeRaycastOffset, Vector2.up, _topRaycastLength, _cornerCorrectLayer) &&
                            !Physics2D.Raycast(transform.position - _innerRaycastOffset, Vector2.up, _topRaycastLength, _cornerCorrectLayer);

        //Wall Collisions
        _onWall = Physics2D.Raycast(transform.position, Vector2.right, _wallRaycastLength, _wallLayer) ||
                    Physics2D.Raycast(transform.position, Vector2.left, _wallRaycastLength, _wallLayer);
        _onRightWall = Physics2D.Raycast(transform.position, Vector2.right, _wallRaycastLength, _wallLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        //Ground Check
        Gizmos.DrawLine(transform.position + _groundRaycastOffset, transform.position + _groundRaycastOffset + Vector3.down * _groundRaycastLength);
        Gizmos.DrawLine(transform.position - _groundRaycastOffset, transform.position - _groundRaycastOffset + Vector3.down * _groundRaycastLength);

        //Corner Check
        Gizmos.DrawLine(transform.position + _edgeRaycastOffset, transform.position + _edgeRaycastOffset + Vector3.up * _topRaycastLength);
        Gizmos.DrawLine(transform.position - _edgeRaycastOffset, transform.position - _edgeRaycastOffset + Vector3.up * _topRaycastLength);
        Gizmos.DrawLine(transform.position + _innerRaycastOffset, transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLength);
        Gizmos.DrawLine(transform.position - _innerRaycastOffset, transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength);

        //Corner Distance Check
        Gizmos.DrawLine(transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength,
                        transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength + Vector3.left * _topRaycastLength);
        Gizmos.DrawLine(transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLength,
                        transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLength + Vector3.right * _topRaycastLength);

        //Wall Check
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * _wallRaycastLength);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * _wallRaycastLength);
    }

    private void AddFliesToNearby(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fly"))
        {
            this.nearbyFlies.Add(collision.gameObject);
            Debug.Log("Fly is in range");
        }
    }
    private void RemoveFliesFromNearby(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fly"))
        {
            if (collision.gameObject == nearestFly)
            {
                nearestFly = null;
            }
            this.nearbyFlies.Remove(collision.gameObject);
        }
    }
    private void AssignNearstFly(Vector2 location, List<GameObject> flies)
    {
        nearestFly = ChooseNearest(location, flies);
    }

    private void EatFly()
    {
        if (!this.playerHasMax)
        {
            Destroy(nearestFly);
            nearestFly = null;
            this.AddEnergy(flyEnergyRecover);
        }
        
    }


    GameObject ChooseNearest(Vector2 location, List<GameObject> flies)
    {
        float nearestSqrMag = float.PositiveInfinity;
        GameObject nearestFly = null;

        for (int i = 0; i < flies.Count; i++)
        {
            float sqrMag = ((Vector2)flies.ElementAt(i).transform.position - location).sqrMagnitude;
            if (sqrMag < nearestSqrMag)
            {
                nearestSqrMag = sqrMag;
                nearestFly = flies.ElementAt(i);
            }
        }

        return nearestFly;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fly"))
        {
            AddFliesToNearby(collision);
        }


    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fly"))
        {
            RemoveFliesFromNearby(collision);
        }
    }


    #region Player Energy Control
    private void AddEnergy(int amnt)
    {
        this.PlayerEnergy += amnt;
    }
    private void RemoveEnergy(int amnt)
    {
        this.PlayerEnergy -= amnt;
    }

    private void EnergyControl()
    {
        if(this.PlayerEnergy >= MaxPlayerEnergy)
        {
            this.PlayerEnergy = 100;
            this.playerHasMax = true;
        } else if(this.PlayerEnergy < MaxPlayerEnergy)
        {
            this.playerHasMax = false;
        } else if(this.PlayerEnergy == minEnergy)
        {
            this.playerHasMin = true;
        } else
        {
            this.playerHasMin = false;
        }
        Debug.Log(this.PlayerEnergy);
    }
    #endregion

    #region Player Shooting Control
    
    #endregion



}
