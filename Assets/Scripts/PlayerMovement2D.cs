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
    private bool isRunning = false;
    private float maxStamina = 10.0f;
    private float currentStamina;
    private float staminaRegen = 2f;
    private float staminaDrain = 5f;
    private bool isTired = false;

    [SerializeField] private Rigidbody2D rb;

    void Start()
    {
        currentStamina = maxStamina;
    }
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
        Transform();
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
        if(isRunning && currentStamina > 0 && animator.GetFloat("Speed") == 1 && !isTired)
        {
            currentStamina -= staminaDrain * Time.deltaTime;
            animator.SetBool("isRunning", true);
            speed = 6f;
        }else{
            animator.SetBool("isRunning", false);
            speed = 4f;
            if(currentStamina <= maxStamina -0.01)
            {
                currentStamina += staminaRegen * Time.deltaTime;
            }
            if(isTired)
            {
                speed = 2f;
            }
        }
    }

    private void Walk()
    {
        rb.velocity = new Vector2(horizontal * speed, vertical * speed);
    }

    private void Tired()
    {
        if(currentStamina <= 0)
        {
            isTired = true;
            animator.SetBool("Tired", true);
            speed = 2f;
        }
        if(currentStamina >= maxStamina-0.2f)
        {
            isTired = false;
            animator.SetBool("Tired", false);
            speed = 4f;
        }
    }

    private void Transform()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            animator.SetBool("Transform", true);
        }
    }
}