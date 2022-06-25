using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private float speed = 4f;
    private bool isFacingRight = true;
    public Animator animator;
    //private bool isRunning = false;
    private bool isRunning = false;
    private double stamina = 10;
    private bool isTired = false;

    [SerializeField] private Rigidbody2D rb;

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        switch(vertical){
            case(1):
                animator.SetBool("Foward", false);
                animator.SetBool("Backward", true);
            break;
            case(-1):
                animator.SetBool("Backward", false);
                animator.SetBool("Foward", true);
            break;
            case(0):
                animator.SetBool("Foward", false);
                animator.SetBool("Backward", false);
            break;
        }
        if(vertical != 0 || horizontal != 0)
        {
            animator.SetFloat("Speed", 1);
        } else {
            animator.SetFloat("Speed", 0);
        }
    }
    
    private void FixedUpdate()
    {
        Flip();
        Walk();
        Running();
        Tired();
        
        Debug.Log(stamina);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void Running()
    {
        isRunning = Input.GetKey(KeyCode.LeftShift);
        if(isRunning && !isTired)
        {
            stamina-=0.1;
            animator.SetBool("isRunning", true);
            StopCoroutine(RegenStamina());
            speed = 6f;
        }else{
            animator.SetBool("isRunning", false);
            speed = 4f;
            StartCoroutine(RegenStamina());
        }
    }

    private void Walk()
    {
        rb.velocity = new Vector2(horizontal * speed, vertical * speed);
    }

    private IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(5);
        if(stamina < 10){
            stamina += 0.1;
            yield return new WaitForSeconds(1);
        };
        
    }

        private void Tired()
    {
        if(stamina <= 0)
        {
            isTired = true;
        } else {
            isTired = false;
        }
    }
}