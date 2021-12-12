using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float MovementSpeed = 30F;
    private Rigidbody2D rigidBody;
    private Vector3 PlayerPosition;
    [SerializeField]
    private float jumpForce = 200;

    public void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {

        if (Input.GetAxis("Horizontal") > 0)
        {
            //rigidBody.velocity = new Vector2(+MovementSpeed * Time.fixedDeltaTime, rigidBody.velocity.y);
            transform.position = new Vector2(+MovementSpeed * Time.fixedDeltaTime, 0);

        }
        if ((Input.GetAxis("Horizontal") < 0))
        {
            transform.position = new Vector2(-MovementSpeed * Time.fixedDeltaTime,0 );

        }

    }


    public void MovePlayer(float deltaTime, Vector3 PlayerPosition, float playerSpeed, float jumpForce)
    {
        

       


    }

    public enum PlayerMovementEnum
    {
        Idle,
        Right,
        Left,
        Jump
    }

}
