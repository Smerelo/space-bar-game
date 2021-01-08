using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.AI;
public class HeadEmployee : MonoBehaviour
{
    [HideInInspector] public string currentZone;
    [HideInInspector] public int employeeType;
    private HeadEmployeeManager employeeManager;
    public float Salary { get; set; }
    public int EmployeeNumber { get; private set; }
    public bool IsWaiting { get; private set; }
    private bool IsWorking { get; set; }
    public bool TaskBegun { get; private set; }
    public bool IsMoving { get; private set; }
    public bool AnimationChosen { get; private set; }
    public bool IsPreparingFood { get; private set; }
    public bool IsStunned { get; private set; }
    public bool HeadingToInput { get; private set; }

   

    public bool HeadingToOutput { get; private set; }

    private const string IDLE_ = "idle_";
    private const string IDLEBACK_ = "idleBack_";
    private const string WALKBACK_ = "walkBack_";
    private const string WALKFRONT_ = "walkFront_";
    private const string WALKLEFT_ = "walkLeft_";
    private const string WALKRIGHT_ = "walkRight_";


    private EmployeeBehaviour employeeBehaviour;
    private ZoneManagment newZone;


    private ZoneManagment oldZone;
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
    private float maxY = 1f;
    private GameObject midPoint;
    private OrderList orderList;
    private LTDescr currentTween;
    private bool isMotivated;
    private float motivationTimer = 0;
    private float motivationDuration = 4;
    private float motivation = 1;
    private NavMeshAgent agent;
    private bool waitFrame;

    //Main
    void Awake()
    {
        employeeBehaviour = GetComponent<EmployeeBehaviour>();
        employeeBehaviour.IsHeadEmployee = true;
    }

    private void Start()
    {
        orderList = GameObject.Find("OrderList").GetComponent<OrderList>();
        midPoint = GameObject.Find("MidPoint");
        animator = GetComponent<Animator>();
        headEmployeeCardMenu = GetComponent<HeadEmployeeCardMenu>();
        employeeManager = GameObject.Find("HeadEmployees")
            .GetComponent<HeadEmployeeManager>();
        oldZone = null;
        employeeBehaviour.Salary = Salary;
    }
    private void Update()
    {
        if (!IsStunned)
        {
            CheckMotivation();
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
            if (IsMoving)
            {
                CheckMovement();
            }
            if (shouldChangeZone && !employeeBehaviour.IsBusy)
            {
                shouldChangeZone = false;
                ChangeZone();
            }
            if (!employeeBehaviour.IsBusy && newZone != null && !IsWaiting)
            {
                GoToWaitZone();
            }
            if (IsWorking && !TaskBegun && currentZone != null && !IsMoving )
            {
                ChooseTask();
            }
        }
    }



    public void InstantiateEmployee(Cv cv)
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        employeeManager = GameObject.Find("HeadEmployees")
            .GetComponent<HeadEmployeeManager>();
        moveSpeed = cv.moveSpeed;
        agent.speed = cv.moveSpeed;
        taskSpeed = cv.taskSpeed;
        Salary = cv.price;
        EmployeeNumber = employeeManager.number + 1;
        employeeBehaviour.IsHeadEmployee = true;
        employeeBehaviour.Salary = cv.price;
        curriculum = cv;
        employeeType = curriculum.employeeType;

        if (headEmployeeCardMenu == null)
        {
            headEmployeeCardMenu = GetComponent<HeadEmployeeCardMenu>();
        }
        headEmployeeCardMenu.FillInfo(cv);
    }

    internal void StartOrder(Order order, Workstation station)
    {
        currentOrder = order;
        order.card.ChangeOwner(employeeType);
        currentOrder.assignedTo = gameObject;
        workstation = station;
        SetUpInputOutput();
        IsWorking = true;
        IsWaiting = false;
        IsMoving = false;
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
                if (currentOrder.IsReady)
                {
                    input = employeeBehaviour.ParentZone.GetInputPos();
                    output = currentOrder.Table.GetWaiterZone();
                }
                else if (currentOrder.IsBeingTakenToClean)
                {
                    input = currentOrder.Table.GetWaiterZone();
                    output = employeeBehaviour.ParentZone.GetOutputPos();
                }
                break;
            case Constants.cleaning:
                input = employeeBehaviour.ParentZone.GetInputPos();
                output = employeeBehaviour.ParentZone.GetOutputPos();
                break;
            default:
                break;
        }
    }

    internal bool CheckAndRemove(Order order)
    {
        if (order == currentOrder)
        {
            AnimationChosen = false;
            agent.isStopped = false;
            IsMoving = false;
            currentOrder.IsAssigned = false;
            if (workstation != null)
            {
                workstation.InUse = false;
                workstation.StopAnimation();
            }
            IsWorking = false;
            TaskBegun = false;
            step = 0;
            orderList.RemoveOrder(currentOrder);
            currentOrder = null;
        }
        return false;
    }

    private void CheckMovement()
    {

        if (IsWaiting)
        {
            if (waitFrame && agent.remainingDistance <= agent.stoppingDistance)
            {
                StopMoving();
            }
            if (!waitFrame)
            {
                waitFrame = true;
            }
        }
        else
        {
            if (currentOrder != null && currentOrder.IsBeingPrepared)
            {
                if (HeadingToInput)
                {
                    object obj = currentOrder.PreparationTime;
                    if (!waitFrame)
                    {
                        waitFrame = true;
                    }
                    if (waitFrame && agent.remainingDistance <= agent.stoppingDistance)
                    {
                        ChangeStep(obj);
                        employeeBehaviour.DrawResource();
                        HeadingToInput = false;
                        waitFrame = false;
                    }
                }
                if (HeadingToOutput)
                {
                    if (!waitFrame)
                    {
                        waitFrame = true;
                    }
                    if (waitFrame && agent.remainingDistance <= agent.stoppingDistance)
                    {
                        HeadingToOutput = false;
                        waitFrame = false;
                        currentOrder.IsReady = true;
                        StopWorking();
                    }
                }
            }
            else if (currentOrder != null && currentOrder.IsReady)
            {
                if (HeadingToInput)
                {
                    object obj = .5f;
                    if (waitFrame && agent.remainingDistance <= agent.stoppingDistance)
                    {
                        ChangeStep(obj);
                        employeeBehaviour.DrawResource();
                        HeadingToInput = false;
                        waitFrame = false;
                    }
                    if (!waitFrame)
                    {
                        waitFrame = true;
                    }

                }
                if (HeadingToOutput)
                {

                    HeadingToOutput = true;
                    if (waitFrame && agent.remainingDistance <= agent.stoppingDistance)
                    {
                        waitFrame = false;
                        HeadingToOutput = false;
                        PassOrder();
                    }
                    if (!waitFrame)
                    {
                        waitFrame = true;
                    }
                }
            }
            else if (currentOrder != null && currentOrder.IsBeingTakenToClean)
            {
                if (HeadingToInput)
                {
                    object obj = 1.5f;
                    if (waitFrame && agent.remainingDistance <= agent.stoppingDistance)
                    {
                        ChangeStep(obj);
                        HeadingToInput = false;
                        waitFrame = false;
                    }
                    if (!waitFrame)
                    {
                        waitFrame = true;
                    }

                }
                if (HeadingToOutput)
                {
                    HeadingToOutput = true;
                    if (waitFrame && agent.remainingDistance <= agent.stoppingDistance)
                    {
                        waitFrame = false;
                        HeadingToOutput = false;
                        StopWorking();
                    }
                    if (!waitFrame)
                    {
                        waitFrame = true;
                    }
                }
            }
        }
    }

    //Tasks
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


    private void BarmanTask()
    {
        if (step == 0 && newZone.input.RessourceQuantity > 0)
        {
            TaskBegun = true;
            agent.SetDestination(input.position);
            HeadingToInput = true;
            IsPreparingFood = true;
            StartMoving();
        }
        if (step == 1)
        {
            workstation.StopAnimation();
            IsPreparingFood = false;
            StartMoving();
            agent.SetDestination(output.position);
            HeadingToOutput = true;
        }
    }
    private void WaiterTask()
    {
        if (currentOrder.IsReady)
        {
            if (step == 0)
            {
                TaskBegun = true;
                agent.SetDestination(input.position);
                HeadingToInput = true;
             
                StartMoving();
            }
            if (step == 1)
            {
                HeadingToOutput = true;
        
                agent.SetDestination(output.position);
                StartMoving();
            }
        }
        else if (currentOrder.IsBeingTakenToClean)
        {
            if (step == 0)
            {
                TaskBegun = true;
                agent.SetDestination(input.position);
                StartMoving();
                HeadingToInput = true;
            }
            if (step == 1)
            {
                currentOrder.Customer.PayAndLeave();
                currentOrder.GenerateMealPrice();
                employeeBehaviour.ParentZone.CashIn(currentOrder.GenerateMealPrice());
                agent.SetDestination(output.position);
                HeadingToOutput = true;
                StartMoving();
            }
        }
    }

   
    //Stun
    internal void Stop()
    {
        agent.isStopped = true;
        ChangeAnimationState(IDLE_ + curriculum.employeeType);
        IsMoving = false;
        IsStunned = true;
    }

    internal void Stun()
    {

        Invoke("RemoveStun", 3f);
    }


    private void RemoveStun()
    {
        IsMoving = true;
        agent.isStopped = false;
        IsStunned = false;
    }


    //Movement
    private void GoToWaitZone()
    {
        agent.SetDestination(waitZone.position);
        IsWaiting = true;
        IsMoving = true;
        AnimationChosen = false;
        movingTo = waitZone.position;
        movingFrom = transform.position;
    }

    private void StopMoving()
    {
        IsWaiting = true;
        currentTween = null;
        IsMoving = false;
    }

    private void CheckMotivation()
    {

        if (employeeBehaviour.GotYelledAt)
        {
            employeeBehaviour.GotYelledAt = false;
            isMotivated = true;
            agent.speed += 1.5f;
        }
        if (isMotivated)
        {
            motivationTimer += Time.deltaTime;
            motivation = 1.3f;
            if (motivationTimer >= motivationDuration)
            {
                motivationTimer = 0;
                motivation = 1;
                isMotivated = false;
                agent.speed = curriculum.moveSpeed;
            }

        }
    }

    private void ChooseAnimation()
    {
        AnimationChosen = true;
        float diffY = Mathf.Abs(movingFrom.y - movingTo.y);
        if (movingTo.x > movingFrom.x)
        {
            if (diffY < maxY)
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

    private void StartMoving()
    {
        movingTo = output.position;
        movingFrom = transform.position;
        IsMoving = true;
        AnimationChosen = false;
    }

    private void PassOrder()
    {
        AnimationChosen = false;
        currentTween = null;
        if (!currentOrder.BossOrder)
        {
            newZone.OrderDone(currentOrder);
        }
        else
        {
            orderList.RemoveOrder(currentOrder);
            newZone.DiscardOrder(currentOrder);
        }
        employeeBehaviour.IsBusy = false;
        currentOrder.Customer.StartEating();
        IsMoving = false;
        currentOrder.IsAssigned = false;
        IsWorking = false;
        TaskBegun = false;
        step = 0;
        currentOrder = null;
    }

    private void StopWorking()
    {
        AnimationChosen = false;
        agent.isStopped = false;
        orderList.SendOrderToNextStep(currentOrder);
        IsMoving = false;
        currentOrder.IsAssigned = false;
        employeeBehaviour.TaskAccomplished();
        workstation.InUse = false;
        IsWorking = false;
        TaskBegun = false;
        step = 0;
        if (currentZone == Constants.serving)
        {
            orderList.RemoveOrder(currentOrder);
        }
        currentOrder = null;
    }

    private void ChangeStep(object obj)
    {
        currentTween = null;
        if (IsPreparingFood)
        {
            workstation.Animate();
        }
        IsMoving = false;
        float timer = (float)obj;
        step++;
        StartCoroutine(Dotask("ChooseTask", timer));
    }

    private IEnumerator Dotask(string method, float timer)
    {
        float t = 0;
        while (t < timer)
        {
            yield return new WaitForSeconds(1);
            if (!IsStunned)
            {
                t += 1 * motivation;
            }

        }

        Invoke(method, 0);

    }

    

    private void ChangeZone()
    {
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


    internal void StopOrder()
    {
        AnimationChosen = false;
        agent.isStopped = false;
        IsMoving = false;
        IsPreparingFood = false;
        HeadingToInput = false;
        HeadingToOutput = false;
        employeeBehaviour.IsBusy = false;
        workstation.InUse = false;
        IsWorking = false;
        TaskBegun = false;
        step = 0;
        currentOrder = null;        
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
