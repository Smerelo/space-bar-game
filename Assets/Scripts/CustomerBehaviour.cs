using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerBehaviour : MonoBehaviour
{
    private Table assignedTable;
    public bool IsEating { get; private set; }
    public bool IsWatingForTable { get;  set; }
    public bool movingUp { get; private set; }

    private TableManager tableManager;
    public int numberInQueue;
    private int foodPreference;
    private float eatingTime;
    private CustomerManager manager;
    [SerializeField] private float movementSpeed;
    private bool isSitting;
    private Vector3 waitZone;
    private Vector3 waitPosition;
    private Vector3 newQueuePosition;

    void Start()
    {
        Debug.Log(waitZone);
        manager = GetComponentInParent<CustomerManager>();
        foodPreference = UnityEngine.Random.Range(0, 2);
        eatingTime = manager.GetEatingTime();
    }

    public void AssignTable(Table table)
    {
        assignedTable = table;
        assignedTable.InUse = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (assignedTable != null)
        {

            if (MoveToTableAndCheck() && !isSitting)
            {
                isSitting = true;
                SendOrder();
            }
        }
        if (IsWatingForTable)
        {
            if (!(Vector3.Distance(waitPosition, transform.position) < 0.1f))
            {
                transform.position = Vector3.MoveTowards(transform.position,waitPosition, movementSpeed * Time.deltaTime);
            }
        }
        if (movingUp)
        {
            if (!(Vector3.Distance(newQueuePosition, transform.position) < 0.1f))
            {
                transform.position = Vector3.MoveTowards(transform.position, newQueuePosition, movementSpeed * Time.deltaTime);

            }
            else
            {
                movingUp = false;
            }

        }
    }

    private void SendOrder() {
        Order order = new Order(foodPreference, assignedTable);
        manager.SendOrder(order);
    }

    private bool MoveToTableAndCheck()
    {
        if (!(Vector3.Distance(assignedTable.GetSittingZone(), transform.position) < 0.1f))
        {
            transform.position = Vector3.MoveTowards(transform.position, assignedTable.GetSittingZone(), movementSpeed * Time.deltaTime);
            return false;
        }
        return true; 
    }

    internal void WaitForTable()
    {
        waitZone = GameObject.Find("Queue").GetComponent<Transform>().position;
        waitPosition = waitZone + new Vector3(0, -2 * numberInQueue);
    }

    internal void MoveUp()
    {
        movingUp = true;
        newQueuePosition = waitPosition + new Vector3(0, 2);
    }
}
