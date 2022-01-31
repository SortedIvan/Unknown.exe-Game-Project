using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlide : MonoBehaviour
{
    public PlayerMovement PL;
    public bool isSliding = false;
    public string slidingAnimation = "Slide";
    public string idleAnimation = "Idle";

    public Rigidbody2D rb;
    public Animator anim;

    public BoxCollider2D normalCollider;
    public BoxCollider2D slideCollider;

    public float slideSpeed = 5f;

    private void Start()
    {
        this.anim = gameObject.GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            performSlide();
        }
        else
        {
            PL.ChangeAnimationState(idleAnimation);
            slideCollider.enabled = false;
            normalCollider.enabled = true;
            isSliding = false;
            PL._groundLinearDrag = 4f;
        }
            

    }

    private void performSlide()
    {
        isSliding = true;
        PL.ChangeAnimationState(slidingAnimation);
        normalCollider.enabled = false;
        slideCollider.enabled = true;

        if (!PL._facingRight)
        {
            rb.AddForce(new Vector2(PL._horizontalDirection, 0f) * slideSpeed * Time.deltaTime);
            PL._groundLinearDrag = 1f;
        }
        else
        {
            rb.AddForce(new Vector2(PL._horizontalDirection, 0f) * slideSpeed * Time.deltaTime);
            PL._groundLinearDrag = 1f;
        }
        

    }










}
