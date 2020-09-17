using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleaner : MonoBehaviour
{

    [SerializeField] private List<float> waitTimes;
    private List<float> InitialWaitTimes;
    private EmployeeBehaviour employeeBehaviour;
    private Transform waitingZone;
    private Workstation workstation;
    private bool hasDrawnResource;
    [SerializeField] private float movementSpeed = 1f;
    private float upgradePercent;
    private float upgradeLevel;


    public bool taskFinished { get; private set; }

    private float timer;
    private List<Transform> destinations;
    private int currentDestIndex;
    private bool shouldWait;
    private bool hasCleanedPlate;
    private int platesCleanedIndex;
    private int drawResourceIndex;

    [SerializeField] private float motivationDuration;
    [SerializeField] private float motivatedSpeedMultiplier;
    [SerializeField] private float motivatedTimerMultiplier;
    private float motivationTimer = 0;
    private float movementSpeedMultiplier = 1f;
    private float timerSpeedMultiplier = 1f;
    private bool isMotivated = false;
    private Vector3 lastPosition;
    private Animator animator;

    void Start()
    {
        upgradeLevel = 1;
        upgradePercent = 0;
        employeeBehaviour = GetComponent<EmployeeBehaviour>();
        waitingZone = employeeBehaviour.ParentZone.GetWaitingZone();
        InitialWaitTimes = new List<float>(waitTimes);
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (employeeBehaviour.ShouldBeginTask(out Workstation station, out Order order))
        {
            BeginTask(station);
        }
        if (employeeBehaviour.IsBusy)
        {
            CleanPlate();
        }
        else
        {
            GoToWaitPoint();
        }
        ManageMotivation();
        WhatDirection();
    }
    private void WhatDirection()
    {
        Vector3 dir = transform.position - lastPosition;
        float horizontal = Vector3.Dot(dir, new Vector3(1, 0));
        float vertical = Vector3.Dot(dir, new Vector3(0, 1));
        animator.SetFloat("horizontal", horizontal);
        animator.SetFloat("vertical", vertical);
        Debug.Log(horizontal);
        Debug.Log(vertical);
        animator.SetBool("isMovingVertically", Mathf.Abs(horizontal) < Mathf.Abs(vertical));
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
            if (motivationTimer >= motivationDuration)
            {
                isMotivated = false;
                movementSpeedMultiplier = 1f;
                timerSpeedMultiplier = 1f;
            }
            movementSpeedMultiplier = motivatedSpeedMultiplier;
            timerSpeedMultiplier = motivatedTimerMultiplier;
        }
    }

    internal void Upgrade()
    {
        if (upgradePercent < 1)
        {
            upgradeLevel++;
            upgradePercent += 1 / upgradeLevel;
        }
        for (int i = 0; i < waitTimes.Count; i++)
        {
            waitTimes[i] = upgradePercent * InitialWaitTimes[i];
        }
    }

    private void GoToWaitPoint()
    {
        if (!(Vector3.Distance(waitingZone.position, transform.position) < 0.1f))
        {
            animator.SetBool("isWalking", true);
            lastPosition = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, waitingZone.position, movementSpeed * Time.deltaTime * movementSpeedMultiplier);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void CleanPlate()
    {
        if (Vector3.Distance(destinations[currentDestIndex].position, transform.position) < 0.1f)
        {
            shouldWait = true;
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", true);
            lastPosition = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, destinations[currentDestIndex].position, movementSpeed * Time.deltaTime * movementSpeedMultiplier);
        }
        if (shouldWait)
        {
            if (currentDestIndex == 1)
            {
                workstation.ReachedStation = true;
            }
            timer += Time.deltaTime * timerSpeedMultiplier;
            if (timer >= waitTimes[currentDestIndex])
            {
                shouldWait = false;
                timer = 0;
                currentDestIndex++;
                if (!hasCleanedPlate && currentDestIndex == platesCleanedIndex)
                {
                    hasCleanedPlate = true;
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

    private void BeginTask(Workstation station)
    {
        hasCleanedPlate = false;
        hasDrawnResource = false;
        workstation = station;
        workstation.InUse = true;
        workstation = station;
        destinations = new List<Transform>
        {
            employeeBehaviour.ParentZone.GetInputPos(),
            station.GetWorkerPlacement(),
            employeeBehaviour.ParentZone.GetOutputPos()
        };
        drawResourceIndex = 1;
        platesCleanedIndex = 2;
    }
}
