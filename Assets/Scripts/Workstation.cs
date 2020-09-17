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
                switch (Random.Range(0, 3))
                {
                    case 1:
                        animator.SetBool("Wine", true);
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
