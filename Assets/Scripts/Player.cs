using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float yellCooldown;
    [SerializeField] private Transform taskArrow;
    [SerializeField] private YellButton yellButton;
    [SerializeField] private Slider slider;
    [HideInInspector] public bool orderAssigned;
    [SerializeField] private CinemachineVirtualCamera camera;
    [SerializeField] private GameObject ui;
    [SerializeField] private GameObject ui2;


    public bool CanYell { get; private set; }
    private bool IsInWorkStation { get; set; }
    public bool IsDoingTask { get; private set; }
    private bool TaskDone {get; set;}
    public bool HasPlate { get; private set; }
    public bool HasDishesToClean { get; private set; }

    private Animator animator;
    private OrderList orderList;
    private float yellDuration = 1;
    private float timer = 0;
    private bool yelling;
    private float lastHorizontal;
    private float lastVertical;
    private bool shouldYell;
    private Order currentOrder;
    private ZoneManagment preparing;
    private ZoneManagment serving;
    private ZoneManagment cleaning;
    private Transform input;
    private Transform output;
    private Workstation workstation;
    private Table table;
    private float cooldownTimer = 0;
    private int typeOfTask;
    private int mode = 0;
    private int step;
    private float taskSpeed = 1.5f;
    private Workstation cleaningStation;


    private void Start()
    {
        orderList = GameObject.Find("OrderList").GetComponent<OrderList>();
        if (yellButton == null)
        {
            yellButton = GameObject.Find("Motivate").GetComponent<YellButton>();
        }
        preparing = GameObject.Find("Preparing").GetComponent<ZoneManagment>();
        serving = GameObject.Find("Serving").GetComponent<ZoneManagment>();
        cleaning = GameObject.Find("Cleaning").GetComponent<ZoneManagment>();
        CanYell = true;
        yellCooldown = 8f;
        animator = GetComponent<Animator>();
        shouldYell = false;
    }

    internal void UpgradeTaskSpeed(float currentSpeed)
    {
        taskSpeed = currentSpeed;
    }

    private void FixedUpdate()
    {
        if (!yelling && !IsDoingTask)
        {
            Move();
        }
    }

    void Update()
    {
        if (!orderAssigned && !HasDishesToClean)
        {
            taskArrow.gameObject.SetActive(false);
        }
        if (IsDoingTask && slider.value < slider.maxValue)
        {
            slider.value += Time.deltaTime * taskSpeed;
        }
        else if(IsDoingTask && slider.value >= slider.maxValue)
        {
            IsDoingTask = false;
            yellButton.ChangeSprite(0);
            mode = 0;
            TaskDone = true;
            slider.gameObject.SetActive(false);
            if (typeOfTask == 0 || typeOfTask == 1)
            {
                step = 2;
                StartNextStep();
            }
        }
        Yell();
        if (Input.GetKeyDown("space") && !IsDoingTask)
        {
            DoAction();
        }
    }

    internal void UpgradeSpeed(float currentSpeed)
    {
        moveSpeed = currentSpeed;
    }


    internal float GetCurrentSpeed()
    {
        return moveSpeed;
    }

    internal float GetCurrentTaskSpeed()
    {
        return taskSpeed;
    }

    internal bool AssignOrder(Order order)
    {
        if (!orderAssigned && !HasDishesToClean)
        {

            currentOrder = order;
            currentOrder.assignedTo = gameObject;
            if (currentOrder.IsBeingPrepared)
            {
                typeOfTask = 0;
                step = 1;
                workstation = preparing.FindUnoccupiedStation();
                workstation.InUse = true;
                Vector3 workpos = workstation.transform.position;
                taskArrow.gameObject.SetActive(true);
                taskArrow.position = new Vector3(workpos.x, workpos.y + 1, 0);
                SetUpInputOutput();
                orderAssigned = true;
            }
            if (currentOrder.IsReady || currentOrder.IsBeingTakenToClean)
            {
                table = currentOrder.Table;
                taskArrow.gameObject.SetActive(true);
                SetUpInputOutput();
                taskArrow.position = new Vector3(input.position.x, input.position.y + 1, 0);
                orderAssigned = true;
            }
           
            return true;
        }
        return false;
    }

    internal void RemoveOrder()
    {
        taskArrow.gameObject.SetActive(false);

        slider.maxValue = 0f;
        slider.value = 0;
        slider.gameObject.SetActive(true);
        HasPlate = false;
        step = 0;
        mode = 0;
        orderAssigned = false;
        IsDoingTask = false;
        orderList.RemoveOrder(currentOrder);
        currentOrder.IsAssigned = false;
        currentOrder = null;
    }

    private void SetUpInputOutput()
    {
        switch (currentOrder.Zone)
        {
            case Constants.preparing:
                input = workstation.transform;
                output =  preparing.GetOutputPos();
                break;
            case Constants.serving:
                if (currentOrder.IsReady)
                {
                    input = serving.GetInputPos();
                    output = currentOrder.Table.GetWaiterZone();
                }
                else if (currentOrder.IsBeingTakenToClean)
                {
                    input = currentOrder.Table.GetWaiterZone();
                    output = serving.GetOutputPos();
                }
                break;
            case Constants.cleaning:
                input = cleaning.GetInputPos();
                output = cleaning.GetOutputPos();
                break;
            default:
                break;
        }
    }

    private void StartNextStep()
    {
        if (currentOrder == null && HasDishesToClean)
        {
            cleaningStation.StopAnimation();
            HasDishesToClean = false;
            cleaning.output.AddRessources(1);
        }
        else if (currentOrder.IsBeingPrepared)
        {
            workstation.StopAnimation();
            workstation = null;
            taskArrow.gameObject.SetActive(true);
            taskArrow.position = new Vector3(output.position.x,
                output.position.y + 1, 0);
        }
        else if (currentOrder.IsBeingTakenToClean)
        {
            HasPlate = true;
            serving.CashIn(currentOrder.GenerateMealPrice());
            currentOrder.Customer.PayAndLeave();
            currentOrder.GenerateMealPrice();
            taskArrow.gameObject.SetActive(true);
            taskArrow.position = new Vector3(output.position.x,
                output.position.y + 1, 0);
        }
    }

    public void DoAction()
    {
        switch (mode)
        {
            case 0:
                ShouldYell();
                break;
            case 1:
                PrepareFood();
                break;
            case 2:
                LeaveReadyPlate();
                break;
            case 3:
                TakePlate();
                break;
            case 4:
                ServePlate();
                break;
            case 5:
                TakeDirtyPlate();
                break;

            case 6:
                LeaveDirtyPlate();
                break;
            case 7:
                TakeDishes();
                break;
            case 8:
                CleanDishes();
                break;
            default:
                break;
        }
    }

    private void CleanDishes()
    {
        IsDoingTask = true;
        slider.gameObject.SetActive(true);
        slider.maxValue = 1f;
        slider.value = 0;
    }

    private void TakeDirtyPlate()
    {
        IsDoingTask = true;
        slider.gameObject.SetActive(true);
        slider.maxValue = 1f;
        slider.value = 0;
    }

    private void LeaveDirtyPlate()
    {
        taskArrow.gameObject.SetActive(false);
        HasPlate = false;
        step = 0;
        mode = 0;
        orderAssigned = false;
        serving.TaskAccomplished(currentOrder);
        orderList.RemoveOrder(currentOrder);
        currentOrder.IsAssigned = false;
        currentOrder.assignedTo = null;
        currentOrder = null;
    }

    private void ServePlate()
    {
        HasPlate = false;
        currentOrder.Customer.StartEating();
        currentOrder.IsReady = false;
        orderAssigned = false;
        yellButton.ChangeSprite(0);
        mode = 0;
        step = 0;
        if (!currentOrder.BossOrder)
        {
            serving.OrderDone(currentOrder);
        }
        else
        {
            orderList.RemoveOrder(currentOrder);
            serving.DiscardOrder(currentOrder);
        }
        currentOrder.assignedTo = null;
        currentOrder = null;
    }

    private void TakeDishes()
    {
        if (cleaningStation == null)
        {
            cleaningStation = cleaning.GetFreeWorkStation();
        }
        if (currentOrder != null)
        {
            currentOrder.IsAssigned = false;
            if (workstation != null)
            {
                workstation.InUse = false;
                workstation = null;
            }
            orderAssigned = false;
            currentOrder = null;
        }
        yellButton.ChangeSprite(0);
        mode = 0;
        taskArrow.gameObject.SetActive(true);
        Debug.Log(cleaningStation);
        taskArrow.position = new Vector3(cleaningStation.GetWorkerPlacement().position.x,
              cleaningStation.GetWorkerPlacement().position.y + 1, 0);
        cleaning.DrawResource();
        HasDishesToClean = true;
    }

    private void TakePlate()
    {
        HasPlate = true;
        serving.DrawResource();
        taskArrow.gameObject.SetActive(true);
        taskArrow.position = new Vector3(output.position.x,
              output.position.y + 1, 0);
        yellButton.ChangeSprite(0);
        mode = 0;
    }

    private void LeaveReadyPlate()
    {
        currentOrder.Zone = Constants.serving;
        currentOrder.IsBeingPrepared = false;
        currentOrder.IsReady = true;
        currentOrder.IsAssigned = false;
        currentOrder.assignedTo = null;
        orderAssigned = false;
        yellButton.ChangeSprite(0);
        mode = 0;
        step = 0;
        orderList.SendOrderToNextStep(currentOrder);
        preparing.TaskAccomplished(currentOrder);
        currentOrder = null;
    }

 
    private void PrepareFood()
    {
        if (preparing.input.RessourceQuantity > 0)
        {
            IsDoingTask = true;
            slider.gameObject.SetActive(true);
            workstation.Animate();
            preparing.DrawResource();
            slider.maxValue = currentOrder.PreparationTime;
            slider.value = 0;
        }
    }

    private void WhatDirection()
    {
        animator.SetFloat("horizontal", lastHorizontal - 0.001f);
        animator.SetFloat("vertical", lastVertical - 0.0001f);
        animator.SetBool("isMovingVertically", Mathf.Abs(lastHorizontal) 
            < 1.1f * Mathf.Abs(lastVertical));
    }

    public void CheckCollision(String name)
    {
        if (name == "DirtyPlates" && cleaning.GetRessourceQuantity() > 0 && !HasDishesToClean && !HasPlate)
        {
            mode = 7;
            yellButton.ChangeSprite(1);
            cleaning.StopSound();
        }
        if (HasDishesToClean && name == "CleanStation")
        {
            mode = 8;
            yellButton.ChangeSprite(1);
            taskArrow.gameObject.SetActive(false);
        }
        if (currentOrder != null)
        {
            if (currentOrder.IsBeingPrepared)
            {
                if (workstation != null && workstation.name == name)
                {
                    mode = 1;
                    IsInWorkStation = true;
                    taskArrow.gameObject.SetActive(false);
                    yellButton.ChangeSprite(1);
                }
                if (step == 2 && name == "ReadyPlates")
                {
                    mode = 2;
                    yellButton.ChangeSprite(1);
                    taskArrow.gameObject.SetActive(false);
                }
            }
            else if (currentOrder.IsReady)
            {
                if (!HasPlate && name == "ReadyPlates")
                {
                    mode = 3;
                    yellButton.ChangeSprite(1);
                    taskArrow.gameObject.SetActive(false);
                }
                if (HasPlate && name == table.name)
                {
                    mode = 4;
                    yellButton.ChangeSprite(1);
                    taskArrow.gameObject.SetActive(false);
                }
            }
            else if (currentOrder.IsBeingTakenToClean)
            {
                if (!HasPlate && name == table.name)
                {
                    mode = 5;
                    yellButton.ChangeSprite(1);
                    taskArrow.gameObject.SetActive(false);
                }

                if (HasPlate && name == "DirtyPlates")
                {
                    mode = 6;
                    yellButton.ChangeSprite(1);
                    taskArrow.gameObject.SetActive(false);
                }
            }
        }
    }

    public void ExitCollision(string name)
    {
        if (name == "DirtyPlates")
        {

            mode = 0;
            yellButton.ChangeSprite(0);
            if (!HasDishesToClean && cleaning.output.RessourceQuantity == 0)
            {
                cleaning.PlaySound();
            }
        }
        if (name == "CleanStation")
        {

            mode = 0;
            yellButton.ChangeSprite(0);
            if (HasDishesToClean)
            {
                taskArrow.gameObject.SetActive(true);
            }
        }
        if (currentOrder != null)
        {
            if (currentOrder.IsBeingPrepared)
            {
                if (workstation != null && workstation.name == name)
                {
                    mode = 0;
                    IsInWorkStation = false;
                    if (!TaskDone)
                    {
                        taskArrow.gameObject.SetActive(true);
                    }
                    yellButton.ChangeSprite(0);
                }
                if (step == 2 && name == "ReadyPlates")
                {
                    mode = 0;
                    yellButton.ChangeSprite(0);
                    taskArrow.gameObject.SetActive(true);
                }
            }
            else if (currentOrder.IsReady)
            {
                if (!HasPlate && name == "ReadyPlates")
                {
                    mode = 0;
                    yellButton.ChangeSprite(0);
                    taskArrow.gameObject.SetActive(true);

                }
                if (HasPlate && name == currentOrder.Table.name)
                {
                    mode = 0;
                    yellButton.ChangeSprite(0);
                    taskArrow.gameObject.SetActive(false);
                }
            }
            else if (currentOrder.IsBeingTakenToClean)
            {
                if (HasPlate && name == "DirtyPlates")
                {
                    mode = 0;
                    yellButton.ChangeSprite(0);
                    taskArrow.gameObject.SetActive(true);

                }
                if (!HasPlate && name == currentOrder.Table.name)
                {
                    mode = 0;
                    yellButton.ChangeSprite(0);
                    taskArrow.gameObject.SetActive(false);
                }
            }
        }

    }

    private void Move()
    {
        float horizontalMovement = CrossPlatformInputs.Instance.GetHorizontal() 
            * moveSpeed * Time.deltaTime;
        float verticalMovement = CrossPlatformInputs.Instance.GetVertical() 
            * moveSpeed * Time.deltaTime;
        WhatDirection();
        if (CrossPlatformInputs.Instance.GetAxisHorizontal() != 0 
            || CrossPlatformInputs.Instance.GetAxisVertical() != 0)
        {
            lastHorizontal = 10 * CrossPlatformInputs.Instance.GetAxisHorizontal();
            lastVertical = 10 * CrossPlatformInputs.Instance.GetAxisVertical();
            animator.SetBool("isWalking", true);

        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        transform.position += new Vector3(horizontalMovement, verticalMovement, 0);
    }

    public void ShouldYell()
    {
        shouldYell = true;
    }

    private void Yell()
    {
        if (!CanYell)
        {
            cooldownTimer += Time.deltaTime;
        }
        if (cooldownTimer >= yellCooldown) 
        {
            CanYell = true;
            cooldownTimer = 0;
        }
        if (CanYell && shouldYell && this.transform.parent != null)
        {
            shouldYell = false;
            CanYell = false;
            yelling = true;
            animator.SetBool("yelling", true);
            this.transform.parent.gameObject.GetComponent<ZoneManagment>().Yell();
        }
        if (yelling)
        {
            timer += Time.deltaTime;
            if (timer >= yellDuration)
            {
                timer = 0;
                yelling = false;
                animator.SetBool("yelling", false);
            }

        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Zone"))
        {
            this.transform.parent = other.transform;
        }
    }
  
}