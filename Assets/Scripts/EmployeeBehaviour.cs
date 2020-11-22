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
    private bool shouldWait;
    private bool platePickup;
    private float timeWaited;
    private Transform currentWorkstation;
    private int currentDestIndex;
    private int currentWaitTimeIndex;
    private List<Transform> destinations;
    private Order currentOrder;
    public ZoneManagment ParentZone { get;  set; }
    private bool hasPlate;
    private float timer;
    private float platePickupTimer;
    private int freeStationIndex;
    private bool hasFreedStation;
    [SerializeField] Cleaner cleaner;

    void Awake()
    {
        ParentZone = transform.GetComponentInParent<ZoneManagment>();
        cleaner = GetComponent<Cleaner>();
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
        shouldBeginTask = true;
        if (order == null)
            print("null bish");
        currentOrder = order;
        workstation = station;
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
            Debug.Log("Can only upgrade cleaners atm");
        }
    }
}
