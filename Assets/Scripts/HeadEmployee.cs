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
    public bool IsMoving { get; private set; }
    public bool AnimationChosen { get; private set; }
    public bool IsPreparingFood { get; private set; }

    private const string IDLE_ = "idle_";
    private const string IDLEBACK_ = "idleBack_";
    private const string WALKBACK_ = "walkBack_";
    private const string WALKFRONT_ = "walkFront_";
    private const string WALKLEFT_ = "walkLeft_";
    private const string WALKRIGHT_ = "walkRight_";


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
    private HeadEmployeeCardMenu headEmployeeCardMenu;
    private Cv curriculum;
    private string currentState;
    private Animator animator;
    private Vector3 movingTo;
    private Vector3 movingFrom;
    private float maxY= .2f;
    private GameObject midPoint;
   
    void Awake()
    {
        employeeBehaviour = GetComponent<EmployeeBehaviour>();
        employeeBehaviour.IsHeadEmployee = true;
    }

    private void Start()
    {
        midPoint = GameObject.Find("MidPoint");
        animator = GetComponent<Animator>();
        headEmployeeCardMenu = GetComponent<HeadEmployeeCardMenu>();
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
        curriculum = cv;
        if (headEmployeeCardMenu == null)
        {
            headEmployeeCardMenu = GetComponent<HeadEmployeeCardMenu>();
        }
        headEmployeeCardMenu.FillInfo(cv);
    }

    internal void StartOrder(Order order, Workstation station)
    {
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
                input = workstation.GetWorkerPlacement();
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
        if (!IsMoving)
        {
            if (IsPreparingFood && transform.position.y > midPoint.transform.position.y)
                ChangeAnimationState(IDLEBACK_ + curriculum.employeeType);
            else
                ChangeAnimationState(IDLE_ + curriculum.employeeType);
        }
        if (IsMoving && !AnimationChosen)
        {
            ChooseAnimation();
        }
        if (shouldChangeZone && !employeeBehaviour.IsBusy)
        {
            shouldChangeZone = false;
            ChangeZone();
        }
        if (!employeeBehaviour.IsBusy && currentZone != null && !IsWaiting && !IsMoving)
        {
            IsWaiting = true;
            GoToWaitZone();
        }
        if (IsWorking && !TaskBegun && currentZone != null && !IsMoving)
        {
            TaskBegun = true;
            ChooseTask();
        }
       
    }

    private void ChooseAnimation()
    {
        AnimationChosen = true;
        float diffY = Mathf.Abs(movingFrom.y - movingTo.y);
        if (movingTo.x > movingFrom.x)
        {
            if (diffY  < maxY)
            {
                ChangeAnimationState(WALKRIGHT_ + curriculum.employeeType);
            }
            else
            {
                if (movingTo.y > movingFrom.y)
                {
                    ChangeAnimationState(WALKBACK_ + curriculum.employeeType);
                }
                else
                {
                    ChangeAnimationState(WALKFRONT_ + curriculum.employeeType);
                }
            }
        }
     
        if (movingTo.x < movingFrom.x)
        {
            if (diffY < maxY)
            {
                ChangeAnimationState(WALKLEFT_ + curriculum.employeeType);
            }
            else
            {
                if (movingTo.y > movingFrom.y)
                {
                    ChangeAnimationState(WALKBACK_ + curriculum.employeeType);
                }
                else
                {
                    ChangeAnimationState(WALKFRONT_ + curriculum.employeeType);
                }
            }
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
                movingTo = input.position;
                movingFrom = transform.position;
                IsMoving = true;
                AnimationChosen = false;
            }
            if (step == 1)
            {
                LeanTween.move(gameObject, output.position,
                     Vector3.Distance(output.position, transform.position) / moveSpeed)
                    .setOnComplete(StopWorking);
                movingTo = output.position;
                movingFrom = transform.position;
                IsMoving = true;
                AnimationChosen = false;

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
            IsMoving = true;
            IsPreparingFood = true;
            movingTo = input.position;
            movingFrom = transform.position;
            AnimationChosen = false;
        }
        if (step == 1)
        {
            workstation.StopAnimation();
            IsPreparingFood = false;
            LeanTween.move(gameObject, output.position,
                 Vector3.Distance(output.position, transform.position) / moveSpeed)
                .setOnComplete(StopWorking);
            IsMoving = true;
            movingTo = output.position;
            movingFrom = transform.position;
            AnimationChosen = false;
        }
    }


    private void StopWorking()
    {
        IsMoving = false;
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
        if (IsPreparingFood)
        {
            workstation.Animate();
        }
        IsMoving = false;
        float timer = (float)obj;
        step++;
        Invoke("ChooseTask", timer);
    }

    private void GoToWaitZone()
    {
        LeanTween.move(gameObject, waitZone.position,
            Vector3.Distance(waitZone.position, transform.position) / moveSpeed).setOnComplete(StopMoving);
        IsMoving = true;
        movingTo = waitZone.position;
        AnimationChosen = false;

        movingFrom = transform.position;
    }

    private void StopMoving()
    {
        IsMoving = false;
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

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }
        animator.Play(newState);
        currentState = newState;
    }

}
