using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleaner : MonoBehaviour
{

    [SerializeField] private List<float> waitTimes;
    private EmployeeBehaviour employeeBehaviour;
    private Transform waitingZone;
    private Workstation workstation;
    private bool hasDrawnResource;
    [SerializeField] private float movementSpeed = 1f;


    public bool taskFinished { get; private set; }

    private float timer;
    private List<Transform> destinations;
    private int currentDestIndex;
    private bool shouldWait;
    private bool hasCleanedPlate;
    private int platesCleanedIndex;
    private int drawResourceIndex;


    void Start()
    {
        employeeBehaviour = GetComponent<EmployeeBehaviour>();
        waitingZone = employeeBehaviour.ParentZone.GetWaitingZone();

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
    }

    private void GoToWaitPoint()
    {
        if (!(getDistance(waitingZone.position, transform.position) < 0.1f))
        {
            transform.position = MoveTowardsAdjacent(transform.position, waitingZone.position, movementSpeed * Time.deltaTime);
        }
    }

    private void CleanPlate()
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
