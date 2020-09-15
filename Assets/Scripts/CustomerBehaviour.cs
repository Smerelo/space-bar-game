using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerBehaviour : MonoBehaviour
{
    private Table assignedTable;
    public bool IsSitting { get; private set; }
    public bool IsEating { get; private set; }
    public bool IsWating { get; private set; }
    public bool IsWatingForTable { get;  set; }


    private int foodPreference;

    private float eatingTime;

    private CustomerManager manager;

    void Start()
    {
        manager = GetComponentInParent<CustomerManager>();
        foodPreference = UnityEngine.Random.Range(0, 2);
        eatingTime = manager.GetEatingTime();
    }

    public void AssignTable(Table table)
    {
        assignedTable = table;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void WaitForTable()
    {
        throw new NotImplementedException();
    }
}
