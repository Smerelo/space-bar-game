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
    [SerializeField] private bool isStoveFood;
    [SerializeField] private bool animate;
    void Awake()
    {
        InUse = false;
        if (animate) foodPlacement = transform.GetChild(1);
        workerPlacement = transform.GetChild(0);
     
    }
    void Start()
    {
        if (animate)
        {
            foodRenderer = foodPlacement.GetComponent<SpriteRenderer>();
            animator = foodPlacement.GetComponent<Animator>();
            if (isStoveFood)
            {
                animator.SetBool("StoveFood", true);
            }
            else
            {
                switch (Random.Range(0, 2))
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
                switch (Random.Range(0, 3))
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

    public Transform GetWorkerPlacement()
    {
        return workerPlacement;
    }
}
