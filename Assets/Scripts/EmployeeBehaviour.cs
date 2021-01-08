using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeBehaviour : MonoBehaviour
{
    public bool IsBusy { get; set; }
    public bool GotYelledAt { get; set; }

    private bool shouldBeginTask;
    private Workstation workstation;
    private int currentDestIndex;
    private int currentWaitTimeIndex;
    private List<Transform> destinations;
    private Order currentOrder;
    public ZoneManagment ParentZone { get;  set; }
    public float Salary { get; set; }
    public bool IsHeadEmployee { get; set; }
    [SerializeField] Cleaner cleaner;
    private HeadEmployee headEmployee;

    void Awake()
    {
        ParentZone = transform.GetComponentInParent<ZoneManagment>();
        cleaner = GetComponent<Cleaner>();
        headEmployee = GetComponent<HeadEmployee>();
    }

    void Update()
    {
    }

    public void TaskAccomplished()
    {
        if (currentOrder == null)
            print("popis");
        currentOrder.IsBeingPrepared = false;
        IsBusy = false;
        destinations = null;
        currentDestIndex = 0;
        ParentZone.TaskAccomplished(currentOrder);
    }
    public void AbortOrder()
    {
        currentOrder = null;
        IsBusy = false;
        headEmployee.StopOrder();
        destinations = null;
        currentDestIndex = 0;
    }

    public void DrawResource()
    {
        ParentZone.DrawResource();
    }

    public void BeginTask(Workstation station, Order order)
    {
        IsBusy = true;
        currentOrder = order;
        headEmployee.StartOrder(order, station);
    }

   

    public bool ShouldBeginTask(out Workstation station, out Order order)
    {
        order = currentOrder;
        station = workstation;

        if (shouldBeginTask)
        {
            shouldBeginTask = false;
            return true;
        }
        return false;
    }

    internal void SetCurrentOrder(Order order)
    {
        currentOrder = order;
    }
 
    internal void SalaryRaise()
    {
        if (cleaner != null)
        {
            cleaner.Upgrade();
        }
        else
        {
        }
    }

    internal void Downgrade()
    {
        if (cleaner != null)
        {
            cleaner.Downgrade();
        }
    }
}
