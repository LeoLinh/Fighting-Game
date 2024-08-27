using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D rb;
    private float xInput;
    public float jumpForce;
    private Animator anim;

    [Header("Dash info")]
    public GameObject dashEffect;
    public float dashSpeed;
    public float dashDuration;
    public float dashTimer;
    public float dashCoolDown;
    public float dashCoolDownTimer;

    [Header("Collision info")]
    public float groundCheckDistance;
    public LayerMask WhatisGround;
    private bool isGrounded;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Movement();
        CheckInput();
        Animation();
        Flip();
        CollisionCheck();

        dashTimer -= Time.deltaTime;
        dashCoolDownTimer -= Time.deltaTime;

        if(isGrounded == false)

        if(isGrounded) Instantiate(dashEffect, transform);
    }

    private void CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, WhatisGround);
    }

    private void Animation()
    {
        bool isMoving = rb.velocity.x != 0;
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isDashing", dashTimer > 0);
    }

    private void Movement()
    {
        if (dashTimer > 0)
        {
            rb.velocity = new Vector2(xInput * dashSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
        }
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            dashCoolDownTimer = dashCoolDown;
            dashTimer = dashDuration;
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

    }

    private void Flip()
    {
        if (rb.velocity.x > 0)
        {
            transform.localScale = Vector3.one;
        }
        if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }
}
