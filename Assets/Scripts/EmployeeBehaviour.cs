using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeBehaviour : MonoBehaviour
{
    public bool IsBusy { get; private set; }

    private Workstation workstation;
    private bool shouldWait;
    private bool platePickup;
    private float timeWaited;
    private Transform currentWorkstation;
    private int currentDestIndex;
    private int currentWaitTimeIndex;
    private List<Transform> destinations;
    private List<float> waitTimes;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float speedBoostMultiplier;
    private ZoneManagment parentZone;
    private bool hasPlate;
    private float timer;
    private float platePickupTimer;
    private int freeStationIndex;
    private bool hasFreedStation;

    void Start()
    {
        parentZone = transform.GetComponentInParent<ZoneManagment>();
    }

    void Update()
    {
        if (IsBusy)
        {
            MoveToNextPoint();
        }
        else
        {
            MoveToWaitPoint();
        }
    }

    private void MoveToWaitPoint()
    {
        if (!(Vector3.Distance(parentZone.GetWaitingZone().position, transform.position) < 0.1f))
        {
            transform.position = Vector3.MoveTowards(transform.position, parentZone.GetWaitingZone().position, movementSpeed * Time.deltaTime);
        }
    }

    private void MoveToNextPoint()
    {
        if (Vector3.Distance(destinations[currentDestIndex].position, transform.position) < 0.1f)
        {
            shouldWait = true;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, destinations[currentDestIndex].position, movementSpeed * Time.deltaTime);
        }
        if (shouldWait)
        {
            timer += Time.deltaTime;
            if (timer >= waitTimes[currentDestIndex])
            {
                shouldWait = false;
                timer = 0;
                currentDestIndex++;
                if (!hasFreedStation && currentDestIndex == freeStationIndex)
                {
                    hasFreedStation = true;
                    workstation.InUse = false;
                }
            }
        }
        if (currentDestIndex >= destinations.Count)
        {
            IsBusy = false;
            destinations = null;
            currentDestIndex = 0;
        }

    }

    public void BeginTask(Workstation station, List<float> waitTimesList)
    {
        IsBusy = true;
        workstation = station;
        workstation.InUse = true;
        destinations = new List<Transform>();
        destinations.Add(parentZone.GetInputPos());
        destinations.Add(station.GetWorkerPlacement());
        freeStationIndex = 2;
        destinations.Add(parentZone.GetOutputPos());
        waitTimes = waitTimesList;
    }
}
