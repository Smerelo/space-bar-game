using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float yellCooldown;
    private float cooldownTimer = 0;
    private bool canYell = true;
    private Animator animator;
    private float yellDuration = 1;
    private float timer = 0;
    private bool yelling;

    private void Start()
    {
        yellCooldown = 8f;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!yelling)
        {
            Move();
        }
        Yell();
    }

    private void Move()
    {
        float horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float verticalMovement = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            animator.SetBool("walking", true);
        }
        else
        {
            animator.SetBool("walking", false);

        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            animator.SetBool("walk", true);

        }
        else
        {
            animator.SetBool("walk", false);
        }
        animator.SetFloat("directionX", Input.GetAxis("Horizontal"));
        animator.SetFloat("directionY", Input.GetAxis("Vertical"));
        transform.position += new Vector3(horizontalMovement, verticalMovement, 0);
    }

    private void Yell()
    {
        if (!canYell)
        {
            cooldownTimer += Time.deltaTime;
        }
        if (cooldownTimer >= yellCooldown) 
        {
            canYell = true;
            cooldownTimer = 0;
        }
        if (canYell && Input.GetKeyDown("space") && this.transform.parent != null)
        {
            canYell = false;
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