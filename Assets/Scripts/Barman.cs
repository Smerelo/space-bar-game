using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barman : MonoBehaviour
{
    private EmployeeBehaviour employeeBehaviour;
    private Workstation workstation;
    private List<Transform> destinations;
    private int freeStationIndex;
    private Transform waitingZone;
    [SerializeField] private List<float> waitTimes;
    private Order currentOrder;
    [SerializeField] public float movementSpeed = 1f;
    public float TaskSpeed { get; set; }
    private int currentDestIndex;
    private bool shouldWait;
    private float timer;
    private bool hasFreedStation;
    private int drawResourceIndex;
    private bool hasDrawnResource;

    [SerializeField] private float motivationDuration;
    [SerializeField] private float motivatedSpeedMultiplier;
    private Transform midPoint;
    [SerializeField] private float motivatedTimerMultiplier;
    private float motivationTimer = 0;
    private float movementSpeedMultiplier = 1f;
    private float timerSpeedMultiplier = 1f;
    private bool isMotivated = false;
    private Animator animator;

    void Start()
    {
        midPoint = GameObject.Find("MidPoint").transform;
        animator = GetComponent<Animator>();
        employeeBehaviour = GetComponent<EmployeeBehaviour>();
        waitingZone = employeeBehaviour.ParentZone.GetWaitingZone();
    }

    void Update()
    {
        if (employeeBehaviour.ShouldBeginTask(out Workstation station, out Order order))
        {
            BeginTask(station, order);
        }
        if (employeeBehaviour.IsBusy)
        {
            MoveToNextPoint();
        }
        else
        {
            MoveToWaitPoint();
        }
        ManageMotivation();
    }

    void ManageMotivation()
    {
        if (employeeBehaviour.GotYelledAt)
        {
            employeeBehaviour.GotYelledAt = false;
            isMotivated = true;
        }
        if (isMotivated)
        {
            motivationTimer += Time.deltaTime;
            movementSpeedMultiplier = motivatedSpeedMultiplier;
            timerSpeedMultiplier = motivatedTimerMultiplier;
            if (motivationTimer >= motivationDuration)
            {
                motivationTimer = 0;
                isMotivated = false;
                movementSpeedMultiplier = 1f;
                timerSpeedMultiplier = 1f;
            }
            
        }
    }

    private void MoveToWaitPoint()
    {
        if (!(Vector3.Distance(waitingZone.position, transform.position) < 0.1f))
        {
            animator.SetBool("isWalking", true);
            if (waitingZone.position.x > transform.position.x)
            {
                animator.SetFloat("direction", 1);
            }
            else
            {
                animator.SetFloat("direction", -1);
            }
            transform.position = Vector3.MoveTowards(transform.position, waitingZone.position, movementSpeed * Time.deltaTime * movementSpeedMultiplier);
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isWating", true);
        }
    }

    private void MoveToNextPoint()
    {
        animator.SetBool("isWating", false);
        if (Vector3.Distance(destinations[currentDestIndex].position, transform.position) < 0.1f)
        {
            animator.SetBool("isWalking", false);
            shouldWait = true;
        }
        else
        {
            animator.SetBool("isWalking", true);
            if (destinations[currentDestIndex].position.x > transform.position.x)
            {
                animator.SetFloat("direction", 1);
            }
            else
            {
                animator.SetFloat("direction", -1);
            }
            transform.position = Vector3.MoveTowards(transform.position, destinations[currentDestIndex].position, movementSpeed * Time.deltaTime * movementSpeedMultiplier);
        }
        if (shouldWait)
        {
            animator.SetBool("isWalking", false);
            if (currentDestIndex == 1)
            {
                workstation.ReachedStation = true;
            }
            if (transform.position.y > midPoint.position.y)
            {
                animator.SetBool("isPreparingFood", true);

            }
            else
            {
                animator.SetBool("isPreparingDrink", true);
            }

            timer += Time.deltaTime * timerSpeedMultiplier;
            if (timer >= waitTimes[currentDestIndex])
            {
                if (transform.position.y > midPoint.position.y)
                {
                    animator.SetBool("isPreparingFood", false);

                }
                else
                {
                    animator.SetBool("isPreparingDrink", false);
                }

                shouldWait = false;
                timer = 0;
                currentDestIndex++;
                if (!hasFreedStation && currentDestIndex == freeStationIndex)
                {
                    hasFreedStation = true;
                    workstation.InUse = false;
                }
                if (!hasDrawnResource && currentDestIndex == drawResourceIndex)
                {
                    hasDrawnResource = true;
                    employeeBehaviour.DrawResource();
                }
            }
        }
        if (currentDestIndex >= destinations.Count)
        {
            employeeBehaviour.TaskAccomplished();
            currentDestIndex = 0;
        }
    }

    public void BeginTask(Workstation station, Order order)
    {
        hasFreedStation = false;
        hasDrawnResource = false;
        workstation = station;
        workstation.InUse = true;
        destinations = new List<Transform>
        {
            employeeBehaviour.ParentZone.GetInputPos(),
            station.GetWorkerPlacement(),
            employeeBehaviour.ParentZone.GetOutputPos()
        };
        freeStationIndex = 2;
        drawResourceIndex = 1;
        waitTimes[1] = Order.GetFoodTypeAsset(order.FoodType).PreparationTime * UnityEngine.Random.Range(0.9f, 1.1f);
        currentOrder = order;
    }
}
