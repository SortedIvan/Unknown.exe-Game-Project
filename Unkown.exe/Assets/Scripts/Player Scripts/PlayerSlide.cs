using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlide : MonoBehaviour
{
    public PlayerMovement PL;
    public bool isSliding = false;

    public Rigidbody2D rb;
    public Animator anim;

    public BoxCollider2D normalCollider;
    public BoxCollider2D slideCollider;

    public float slideSpeed = 5f;

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            performSlide();
    }

    private void performSlide()
    {
        isSliding = true;
        // to be added anim.SetBool(sliding);
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
        StartCoroutine("stopSlide");

    }

    IEnumerator stopSlide()
    {
        yield return new WaitForSeconds(0.8f);
        //anim.Play("Idle");
        //anim.SetBool ("isSlide", false);
        slideCollider.enabled = false;
        normalCollider.enabled = true;
        PL._groundLinearDrag = 4f;

    }









}
