using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerBehaviour : MonoBehaviour
{
    private Table assignedTable;
    private bool isEating;
    public bool IsWatingForTable { get;  set; }
    public bool HasFinishedEating { get; private set; }

    private bool isLeaving;
    private int foodPreference;

    private float eatingTime;

    private CustomerManager manager;
    [SerializeField] private float movementSpeed;
    private bool isSitting;
    private float timer;

    void Start()
    {
        manager = GetComponentInParent<CustomerManager>();
        foodPreference = UnityEngine.Random.Range(0, 2);
        eatingTime = manager.GetEatingTime();
    }

    public void AssignTable(Table table)
    {
        assignedTable = table;
        assignedTable.InUse = true;
    }

    public void StartEating()
    {
        isEating = true;
        //maybe animate or smth
    }
    public void PayAndLeave()
    {
        HasFinishedEating = false;
        isLeaving = true;
        Destroy(this.gameObject, 2);
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
            if (isEating)
            {
                timer += Time.deltaTime;
                if (timer >= eatingTime)
                {
                    HasFinishedEating = true;
                    timer = 0;
                    isEating = false;
                }
            }
        }
    }

    private void SendOrder() {
        Order order = new Order(foodPreference, assignedTable, this);
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

    }


}
