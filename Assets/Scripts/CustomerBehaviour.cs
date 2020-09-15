using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerBehaviour : MonoBehaviour
{
    private Table assignedTable;
    public bool IsEating { get; private set; }
    public bool IsWatingForTable { get;  set; }


    private int foodPreference;

    private float eatingTime;

    private CustomerManager manager;
    [SerializeField] private float movementSpeed;
    private bool isSitting;

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

    }


}
