using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workstation : MonoBehaviour
{
    public bool InUse { get; set; }
    public bool ReachedStation { get; internal set; }

    private Transform workerPlacement;
    private Transform foodPlacement;
    private Animator animator;
    private SpriteRenderer foodRenderer;
    private Player player;
    [SerializeField] private bool isStoveFood;
    [SerializeField] private bool animate;
    [SerializeField] private bool isCleaner;
   
    
    void Awake()
    {
        InUse = false;
        if (animate) foodPlacement = transform.GetChild(1);
        workerPlacement = transform.GetChild(0);
     
    }
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        if (animate)
        {
            foodRenderer = foodPlacement.GetComponent<SpriteRenderer>();
            animator = foodPlacement.GetComponent<Animator>();
            if (isStoveFood)
            {
                animator.SetBool("StoveFood", true);
            }
            else if (isCleaner)
            {
                animator.SetBool("Cleaner", true);
            }
            else
            {
                switch (UnityEngine.Random.Range(0, 2))
                {
                    case 0:
                        animator.SetBool("Wine", true);
                        break;
                    case 1:
                        animator.SetBool("Cup", true);
                        break;
                    default:
                        foodRenderer.enabled = false;
                        break;
                }
                switch (UnityEngine.Random.Range(0, 3))
                {
                    case 0:
                        animator.SetBool("Yellow", true);
                        break;
                    case 1:
                        animator.SetBool("Pink", true);
                        break;
                    case 2:
                        animator.SetBool("Green", true);
                        break;
                    default:
                        foodRenderer.enabled = false;
                        break;
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player.CheckCollision(gameObject.name) ;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player.ExitCollision(gameObject.name);
        }
    }

    void Update()
    {
        if (animate)
        {
            if (InUse && ReachedStation)
            {
                animator.enabled = true;
                foodRenderer.enabled = true;
            }
            else
            {
                ReachedStation = false;
                animator.enabled = false;
                foodRenderer.enabled = false;
            }
        }
    }

    internal void StopAnimation()
    {
        InUse = false;
        ReachedStation = false;
    }

    public Transform GetWorkerPlacement()
    {
        return workerPlacement;
    }

    internal void Animate()
    {
        InUse = true;
        ReachedStation = true;
    }

}
