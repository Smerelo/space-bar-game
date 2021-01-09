using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;
public class CustomerBehaviour : MonoBehaviour
{
    private Table assignedTable;
    public bool IsWatingForTable { get;  set; }
    public bool movingUp { get; private set; }
    public int numberInQueue;
    public bool HasFinishedEating { get; private set; }
    public bool isWalking { get; private set; }
    public bool hasReviewed { get; private set; }

    private bool isLeaving;
    [HideInInspector]public bool isEating;
    private Order.FoodTypes foodPreference;
    private float eatingTime;
    private CustomerManager manager;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float patienceMultiplier = 10f;
    private bool isSitting;
    private Vector3 waitZone;
    private Vector3 waitPosition;
    private float timer;
    private Animator animator;
    private bool sentOrder;
    private TextBubble textBubble;
    private CustomerFood food;
    private bool waitingForOrder;
    private float waitTiimer;
    private float maxPatience;
    private Order order;
    [SerializeField] private float bubbleUptime = 1f;
    private Transform UI;
    [SerializeField] private GameObject moneyPrefab;
    private Transform moneyPos;
    [SerializeField] private Color negativeColor;
    private CentralTransactionLogic ctl;
    private Popularity popularity;
    private NavMeshAgent agent;
    private bool waitFrame;
    private OrderList orderList;


   
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ctl = GameObject.Find("SpaceCantina").GetComponent<CentralTransactionLogic>();
        maxPatience = Order.GetFoodTypeAsset(foodPreference).PreparationTime * patienceMultiplier;
        waitTiimer = maxPatience;
        Debug.Log(maxPatience);
        moneyPos = transform.GetChild(1);
        UI = GameObject.Find("UI").transform; ;
        foreach (Transform t in transform)
        {
            if (t.TryGetComponent(out TextBubble bub))
            {
                textBubble = bub;
            }
            if (t.TryGetComponent(out CustomerFood f))
            {
                food = f;
            }
        }
        animator = GetComponent<Animator>();
        manager = GetComponentInParent<CustomerManager>();
        foodPreference = Order.RandomFoodType();
        // eatingTime = manager.GetEatingTime();
        eatingTime = 7f;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    internal void Pay(float mealprice)
    {
        GameObject money = Instantiate(moneyPrefab, moneyPos.position, Quaternion.identity, this.transform).transform.GetChild(0).gameObject;
        money.transform.position = moneyPos.position;
        TextMeshProUGUI m_text = money.GetComponent<TextMeshProUGUI>();
        int decimals = Mathf.Abs((int)Math.Round(100.0 * (mealprice - Math.Truncate(mealprice))));
        m_text.text = $"${Math.Truncate(mealprice)}<size=-14>{decimals.ToString("D2")}</size>";
        if (mealprice < 0)
        {
            m_text.color = negativeColor;
        }
        Destroy(money, 1f);
    }

    internal float GetWaitTimeBonus()
    {
        return waitTiimer / maxPatience;
    }

    internal Order GetOrder()
    {
        return order;
    }

    public void AssignTable(Table table)
    {
        assignedTable = table;
        assignedTable.InUse = true;
    }

    public void StartEating()
    {
        waitingForOrder = false;
        if (order != null && !order.BossOrder)
        {
            food.ShowFood(eatingTime);
            animator.SetBool("isEating", true);
        }
        isEating = true;
    }
    public void PayAndLeave()
    {
        food.HideFood();
        HasFinishedEating = false;
        isSitting = false;
        manager.customers.Remove(this);
        animator.SetBool("isSitting", false);
        isLeaving = true;
        Destroy(this.gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (assignedTable != null)
        {
            if (isEating)
            {
                timer += Time.deltaTime;
                if (timer >= eatingTime)
                {
                    HasFinishedEating = true;
                    timer = 0;
                    isEating = false;
                    order.IsReady = false;
                    order.IsBeingTakenToClean = true;
                    orderList = GameObject.Find("OrderList").GetComponent<OrderList>();
                    orderList.SendOrderToNextStep(order);
                    animator.SetBool("isEating", false);

                }
            }
            if (MoveToTableAndCheck() && !sentOrder)
            {
                sentOrder = true;
                isSitting = true;
                transform.position += new Vector3(0,0,-3);
                animator.SetBool("isSitting", true);
                waitingForOrder = true;

                SendOrder();
            }
        }
        if (IsWatingForTable)
        {
            if (!(Vector3.Distance(waitPosition, transform.position) < 0.1f))
            {
                isWalking = true;
                animator.SetBool("isWalking", true);
                transform.position = Vector3.MoveTowards(transform.position,  waitPosition, movementSpeed * Time.deltaTime);
                if (waitPosition.x > transform.position.x)
                {
                    animator.SetFloat("direction", 1);
                }
                else
                {
                    animator.SetFloat("direction", -1);
                }
            }
            else
            {
                isWalking = false;
                animator.SetBool("isWalking", false);
                animator.SetBool("isWaiting", true);

            }
        }
        if (waitingForOrder)
        {
            waitTiimer -= Time.deltaTime;
            if (waitTiimer < 0)
            {
                GetAngry();
            }
            if (waitTiimer <= -10 && !hasReviewed)
            {
                hasReviewed = true;
                Leave();
            }

        }
        
    }

    private void Leave()
    {
        popularity = GameObject.Find("PopularityBar").GetComponent<Popularity>();
        ctl.DestroyOrder(order);
        popularity.UpdatePopularity(-1);
        assignedTable.InUse = false;
        manager.customers.Remove(this);
        animator.SetBool("isSitting", false);
        isLeaving = true;
        Destroy(this.gameObject, 1f);
    }



    internal void GoHome()
    {
        Destroy(gameObject);
    }

    private void GetAngry()
    {
        animator.SetBool("isAngry", true);

    }

    private void SendOrder() 
    {
        textBubble.SetSprite(foodPreference);
        textBubble.ShowBubble(bubbleUptime);
        food.SetFood(foodPreference);
        order = new Order(foodPreference, assignedTable, this);
        manager.SendOrder(order);
    }

    private bool MoveToTableAndCheck()
    {
        Vector3 sittingZone = new Vector3(assignedTable.GetSittingZone().x,
           assignedTable.GetSittingZone().y - .3f, transform.position.z);
        agent.SetDestination(sittingZone);

        if (agent.remainingDistance >= agent.stoppingDistance || float.IsInfinity(agent.remainingDistance))
        {
            return false;
        }
        if (!waitFrame)
        {
            waitFrame = true;
            return false;
        }
        return true;
  
    }

    internal void WaitForTable()
    {
        waitZone = GameObject.Find("Queue").GetComponent<Transform>().position;
        waitPosition = waitZone + new Vector3(1 * numberInQueue, 0);
    }

    internal void MoveUp()
    {
        movingUp = true;
        waitPosition = waitPosition + new Vector3(-1, 0);
    }
}
