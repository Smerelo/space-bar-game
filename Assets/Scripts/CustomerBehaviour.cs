using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerBehaviour : MonoBehaviour
{
    private Table assignedTable;
    public bool IsWatingForTable { get;  set; }
    public bool movingUp { get; private set; }

    public int numberInQueue;
    public bool HasFinishedEating { get; private set; }
    public bool isWalking { get; private set; }

    private bool isLeaving;
    private bool isEating;
    private int foodPreference;
    private float eatingTime;
    private CustomerManager manager;
    [SerializeField] private float movementSpeed;
    private bool isSitting;
    private Vector3 waitZone;
    private Vector3 waitPosition;
    private float timer;
    private Animator animator;
    private bool sentOrder;
    private TextBubble textBubble;
    private CustomerFood food;
    [SerializeField] private float bubbleUptime = 1f;

    private void Awake()
    {

    }
    void Start()
    {
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
        foodPreference = UnityEngine.Random.Range(0, textBubble.SpritesCount);
        eatingTime = manager.GetEatingTime();
    }

    public void AssignTable(Table table)
    {
        assignedTable = table;
        assignedTable.InUse = true;
    }

    public void StartEating()
    {
        food.ShowFood(eatingTime);
        isEating = true;
        animator.SetBool("isEating", true);
        //maybe animate or smth
    }
    public void PayAndLeave()
    {
        food.HideFood();
        HasFinishedEating = false;
        isSitting = false;
        manager.customers.Remove(this);
        animator.SetBool("isSitting", false);
        isLeaving = true;
        Destroy(this.gameObject);
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
                    animator.SetBool("isEating", false);

                }
            }
            if (MoveToTableAndCheck() && !sentOrder)
            {
                sentOrder = true;
                isSitting = true;
                transform.position += new Vector3(0,0,-3);
                animator.SetBool("isSitting", true);

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
        
    }

    private void SendOrder() 
    {
        textBubble.SetSprite(foodPreference);
        textBubble.ShowBubble(bubbleUptime);
        food.SetFood(foodPreference);
        Order order = new Order(foodPreference, assignedTable, this);
        manager.SendOrder(order);
    }

    private bool MoveToTableAndCheck()
    {
        if (!(Vector3.Distance(assignedTable.GetSittingZone(), transform.position) < 0.1f))
        {
            isWalking = true;
            animator.SetBool("isWalking", true);

            if (assignedTable.GetSittingZone().x > transform.position.x)
            {
                animator.SetFloat("direction", 1);
            }
            else
            {
                animator.SetFloat("direction", -1);
            }
            transform.position = Vector3.MoveTowards(transform.position, assignedTable.GetSittingZone(), movementSpeed * Time.deltaTime);
            return false;
        }
        animator.SetBool("isWalking", false);
        isWalking = false;
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
