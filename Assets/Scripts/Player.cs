using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float yellCooldown;
    private float cooldownTimer = 0;
    public bool CanYell { get; private set; }
    private Animator animator;
    private float yellDuration = 1;
    private float timer = 0;
    private bool yelling;
    private float lastHorizontal;
    private float lastVertical;
    private bool shouldYell;
    

    private void Start()
    {
        CanYell = true;
        yellCooldown = 8f;
        animator = GetComponent<Animator>();
        shouldYell = false;
    }

    void Update()
    {
        if (!yelling)
        {
            Move();
        }
        Yell();
        if (Input.GetKeyDown("space"))
        {
            ShouldYell();
        }
    }
    private void WhatDirection()
    {
        animator.SetFloat("horizontal", lastHorizontal - 0.001f);
        animator.SetFloat("vertical", lastVertical - 0.0001f);
        animator.SetBool("isMovingVertically", Mathf.Abs(lastHorizontal) < 1.1f * Mathf.Abs(lastVertical));
    }

    private void Move()
    {
        float horizontalMovement = CrossPlatformInputs.Instance.GetHorizontal() * moveSpeed * Time.deltaTime;
        float verticalMovement = CrossPlatformInputs.Instance.GetVertical() * moveSpeed * Time.deltaTime;
        WhatDirection();
        if (CrossPlatformInputs.Instance.GetAxisHorizontal() != 0 || CrossPlatformInputs.Instance.GetAxisVertical() != 0)
        {
            lastHorizontal = 10 * CrossPlatformInputs.Instance.GetAxisHorizontal();
            lastVertical = 10 * CrossPlatformInputs.Instance.GetAxisVertical();
            animator.SetBool("isWalking", true);

        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        transform.position += new Vector3(horizontalMovement, verticalMovement, 0);
    }

    public void ShouldYell()
    {
        shouldYell = true;
    }

    private void Yell()
    {
        if (!CanYell)
        {
            cooldownTimer += Time.deltaTime;
        }
        if (cooldownTimer >= yellCooldown) 
        {
            CanYell = true;
            cooldownTimer = 0;
        }
        if (CanYell && shouldYell && this.transform.parent != null)
        {
            shouldYell = false;
            CanYell = false;
            yelling = true;
            animator.SetBool("yelling", true);
            this.transform.parent.gameObject.GetComponent<ZoneManagment>().Yell();
        }
        if (yelling)
        {
            timer += Time.deltaTime;
            if (timer >= yellDuration)
            {
                timer = 0;
                yelling = false;
                animator.SetBool("yelling", false);
            }

        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Zone"))
        {
            this.transform.parent = other.transform;
        }
    }
   /* private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Zone"))
        {
            this.transform.parent = null;
        }
    }*/
}