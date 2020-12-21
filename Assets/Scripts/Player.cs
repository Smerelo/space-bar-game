using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float yellCooldown;
    [SerializeField] private Transform taskArrow;
    private float cooldownTimer = 0;
    public bool CanYell { get; private set; }
    private Animator animator;
    private float yellDuration = 1;
    private float timer = 0;
    private bool yelling;
    private float lastHorizontal;
    private float lastVertical;
    private bool shouldYell;
    private Order currentOrder;
    private ZoneManagment preparing;
    private ZoneManagment serving;
    private ZoneManagment cleaning;
    private Workstation workstation;
    private Table table;


    internal void AssignOrder(Order order)
    {
        if (currentOrder == null)
        {
            currentOrder = order;
            if (currentOrder.IsBeingPrepared)
            {
                workstation = preparing.GetFreeWorkStation();
                Debug.Log(workstation);

                workstation.InUse = true;
                Vector3 workpos = workstation.transform.position;
                Debug.Log(workpos);
                taskArrow.position = new Vector3(workpos.x, workpos.y + 1, 0);
            }
            if (currentOrder.IsReady)
            {
                table = currentOrder.Table;
            }
        }
    }

    private void Start()
    {
        preparing = GameObject.Find("Preparing").GetComponent<ZoneManagment>();
        serving = GameObject.Find("Serving").GetComponent<ZoneManagment>();
        cleaning = GameObject.Find("Cleaning").GetComponent<ZoneManagment>();
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