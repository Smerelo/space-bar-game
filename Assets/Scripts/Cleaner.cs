using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleaner : MonoBehaviour
{

    [SerializeField] private float waitTime;
    private EmployeeBehaviour employeeBehaviour;
    private Transform waitingZone;
    private Workstation workstation;
    private bool hasDrawnResource;
    [SerializeField] private float movementSpeed = 1f;


    public bool taskFinished { get; private set; }

    private float timer;
    private Transform placeOfWork;
    public bool doingTask { get; private set; }

    void Start()
    {
        employeeBehaviour = GetComponent<EmployeeBehaviour>();
        waitingZone = employeeBehaviour.ParentZone.GetWaitingZone();

    }

    // Update is called once per frame
    void Update()
    {
        if (employeeBehaviour.ShouldBeginTask(out Workstation station, out List<float> waitTimesList, out Order order) || Input.GetKeyDown("q"))
        {
            BeginTask(station);
        }
        if (employeeBehaviour.IsBusy)
        {
            CleanPlate();
        }
        if (taskFinished )
        {
            GoToWaitPoint();
        }
    }

    private void GoToWaitPoint()
    {
        if (!(Vector3.Distance(waitingZone.position, transform.position) < 0.1f))
        {
            transform.position = Vector3.MoveTowards(transform.position, waitingZone.position, movementSpeed * Time.deltaTime);
        }
        else
        {
            taskFinished = false;
        }
    }

    private void CleanPlate()
    {
        if (Vector3.Distance(placeOfWork.position, transform.position) < 0.1f)
        {
            doingTask = true;
            workstation.InUse = true;
        }
        if ( doingTask && !hasDrawnResource)
        {
            hasDrawnResource = true;
            employeeBehaviour.DrawResource();
        }
        if(doingTask && hasDrawnResource)
        {
            timer += Time.deltaTime;
            if (timer >= waitTime)
            {
                doingTask = false;
                hasDrawnResource = false;
                employeeBehaviour.TaskAccomplished();
                taskFinished = true;
                timer = 0;
            }
        }
    }

    private void BeginTask(Workstation station)
    {
        workstation = station;
        placeOfWork = station.GetWorkerPlacement();
    }
}
