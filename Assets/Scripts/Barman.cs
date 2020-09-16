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
    }

    private void MoveToWaitPoint()
    {
        if (!(getDistance(waitingZone.position, transform.position) < 0.1f))
        {
            transform.position = MoveTowardsAdjacent(transform.position, waitingZone.position, movementSpeed * Time.deltaTime);
        }
    }

    private void MoveToNextPoint()
    {
        if (getDistance(destinations[currentDestIndex].position, transform.position) < 0.1f)
        {
            shouldWait = true;
        }
        else
        {
            transform.position = MoveTowardsAdjacent(transform.position, destinations[currentDestIndex].position, movementSpeed * Time.deltaTime);
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

    private Vector3 MoveTowardsAdjacent(Vector3 position, Vector3 destination, float speed)
    {
        int xDirection = destination.x > position.x ? 1 : -1;
        int yDirection = destination.y > position.y ? 1 : -1;
        if (Mathf.Abs(destination.x - position.x) > 0.01f)
        {
            position += new Vector3(speed * xDirection, 0, 0);
        }
        else if (Mathf.Abs(destination.y - position.y) > 0.01f)
        {
            position += new Vector3(0, speed * yDirection, 0);
        }
        return position;
    }
    private float getDistance(Vector3 a, Vector3 b)
    {
        return Mathf.Sqrt((b.x - a.x) * (b.x - a.x) + (b.y - a.y) * (b.y - a.y));
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
        currentOrder = order;
    }
}
