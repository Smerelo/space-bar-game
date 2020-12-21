using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HeadEmployee : MonoBehaviour
{
    private HeadEmployeeManager employeeManager;
    public float Salary { get; set; }
    public int EmployeeNumber { get; private set; }
    public bool IsWaiting { get; private set; }
    private bool IsWorking { get; set; }
    public bool TaskBegun { get; private set; }

    private EmployeeBehaviour employeeBehaviour;
    private ZoneManagment newZone;
    private ZoneManagment oldZone;
    private string currentZone;
    public bool shouldChangeZone = false;
    private Transform input;
    private Transform output;
    private Order currentOrder;
    private Workstation workstation;
    private float moveSpeed;
    private float taskSpeed;
    private Transform waitZone;
    private float taskTimer;
    private int step = 0;


    void Awake()
    {
        employeeBehaviour = GetComponent<EmployeeBehaviour>();
        employeeBehaviour.IsHeadEmployee = true;
    }

    private void Start()
    {
        employeeManager = GameObject.Find("HeadEmployees")
            .GetComponent<HeadEmployeeManager>();
        oldZone = null;
        employeeBehaviour.Salary = Salary;
    }

    public void InstantiateEmployee(Cv cv)
    {
        employeeManager = GameObject.Find("HeadEmployees")
            .GetComponent<HeadEmployeeManager>();
        moveSpeed = cv.moveSpeed;
        taskSpeed = cv.taskSpeed;
        Salary = cv.price;
        EmployeeNumber = employeeManager.number + 1;
        employeeBehaviour.IsHeadEmployee = true;
        employeeBehaviour.Salary = cv.price;
    }

    internal void StartOrder(Order order, Workstation station)
    {
        Debug.Log("START");
        currentOrder = order;
        workstation = station;
        SetUpInputOutput();
        IsWorking = true;
        IsWaiting = false;
    }

    private void SetUpInputOutput()
    {
        switch (currentOrder.Zone)
        {
            case Constants.preparing:
                input = workstation.transform;
                output = employeeBehaviour.ParentZone.GetOutputPos();
                break;
            case Constants.serving:
                input = employeeBehaviour.ParentZone.GetInputPos();
                output = currentOrder.Table.GetWaiterZone();
                break;
            case Constants.cleaning:
                input = employeeBehaviour.ParentZone.GetInputPos();
                output = employeeBehaviour.ParentZone.GetOutputPos();
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (shouldChangeZone && !employeeBehaviour.IsBusy)
        {
            shouldChangeZone = false;
            ChangeZone();
        }
        if (!employeeBehaviour.IsBusy && currentZone != null && !IsWaiting)
        {
            IsWaiting = true;
            GoToWaitZone();
        }
        if (IsWorking && !TaskBegun && currentZone != null)
        {
            TaskBegun = true;
            ChooseTask();
        }
    }


    private void ChooseTask()
    {
        switch (currentZone)
        {
            case Constants.preparing:
                BarmanTask();
                break;
            case Constants.serving:
                WaiterTask();
                break;
            case Constants.cleaning:
                CleanerTask();
                break;
            default:
                break;
        }
    }

    private void CleanerTask()
    {
        throw new NotImplementedException();
    }

    private void WaiterTask()
    {
        if (!currentOrder.IsBeingTakenToClean)
        {
            if (step == 0)
            {
                object obj = 1f;
                LeanTween.move(gameObject, input.position,
                    Vector3.Distance(input.position, transform.position) / moveSpeed)
                    .setOnComplete(ChangeStep, obj);
                employeeBehaviour.DrawResource();
            }
            if (step == 1)
            {
                LeanTween.move(gameObject, output.position,
                     Vector3.Distance(output.position, transform.position) / moveSpeed)
                    .setOnComplete(StopWorking);
            }
        }
    }

    private void BarmanTask()
    {
        if (step == 0)
        {
            object obj = currentOrder.PreparationTime;
            LeanTween.move(gameObject, input.position,
                Vector3.Distance(input.position, transform.position) / moveSpeed)
                .setOnComplete(ChangeStep, obj);
            employeeBehaviour.DrawResource();
        }
        if (step == 1)
        {
            LeanTween.move(gameObject, output.position,
                 Vector3.Distance(output.position, transform.position) / moveSpeed)
                .setOnComplete(StopWorking);
        }
    }


    private void StopWorking()
    {
        if (currentZone == Constants.serving)
        {
            currentOrder.Customer.StartEating();
        }
        currentOrder.IsAssigned = false;
        employeeBehaviour.TaskAccomplished();
        workstation.InUse = false;
        IsWorking = false;
        TaskBegun = false;
        step = 0;
    }

    private void ChangeStep(object obj)
    {
        float timer = (float)obj;
        step++;
        Invoke("ChooseTask", timer);
    }

    private void GoToWaitZone()
    {
        LeanTween.move(gameObject, waitZone.position,
            Vector3.Distance(waitZone.position, transform.position) / moveSpeed);
    }

    private void ChangeZone()
    {
        IsWaiting = false;
        this.transform.parent = newZone.transform;
        employeeBehaviour.ParentZone = newZone;
        if (oldZone != null)
        {
            oldZone.RemoveSuperEmployee(employeeBehaviour);
        }
        newZone.AddSuperEmployee(employeeBehaviour);
        if (newZone.gameObject.name == Constants.preparing)
        {
            currentZone = Constants.preparing;
            waitZone = newZone.GetWaitingZone();
        }
        if (newZone.gameObject.name == Constants.serving)
        {
            currentZone = Constants.serving;
            waitZone = newZone.GetWaitingZone();


        }
        if (newZone.gameObject.name == Constants.cleaning)
        {
            waitZone = newZone.GetWaitingZone();
            currentZone = Constants.cleaning;
        }
        oldZone = newZone;
    }

    internal void GetNewZone(ZoneManagment zone)
    {
        shouldChangeZone = true;
        newZone = zone;
    }



}
