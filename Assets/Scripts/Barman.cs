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
    private List<float> waitTimes;
    private Order currentOrder;
    [SerializeField] private float movementSpeed = 1f;
    private int currentDestIndex;
    private bool shouldWait;
    private float timer;
    private bool hasFreedStation;
    private int drawResourceIndex;
    private bool hasDrawnResource;

    void Start()
    {
        employeeBehaviour = GetComponent<EmployeeBehaviour>();
        waitingZone = employeeBehaviour.ParentZone.GetWaitingZone();
    }

    void Update()
    {
        if (employeeBehaviour.ShouldBeginTask(out Workstation station, out List<float> waitTimesList, out Order order))
        {
            BeginTask(station , waitTimesList, order);
        }
        if (employeeBehaviour.IsBusy)
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
        if (!(Vector3.Distance(waitingZone.position, transform.position) < 0.1f))
        {
            transform.position = Vector3.MoveTowards(transform.position, waitingZone.position, movementSpeed * Time.deltaTime);
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

    public void BeginTask(Workstation station, List<float> waitTimesList, Order order)
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
        waitTimes = waitTimesList;
        currentOrder = order;
    }
}
