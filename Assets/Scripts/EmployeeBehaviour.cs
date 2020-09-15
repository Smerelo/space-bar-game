using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeBehaviour : MonoBehaviour
{
    public bool IsBusy { get; private set; }

    private bool shouldBeginTask;
    private Workstation workstation;
    private bool shouldWait;
    private bool platePickup;
    private float timeWaited;
    private Transform currentWorkstation;
    private int currentDestIndex;
    private int currentWaitTimeIndex;
    private List<Transform> destinations;
    private List<float> waitTimes;
    private Order currentOrder;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float speedBoostMultiplier;
    public ZoneManagment ParentZone { get; private set; }
    private bool hasPlate;
    private float timer;
    private float platePickupTimer;
    private int freeStationIndex;
    private bool hasFreedStation;

    void Awake()
    {
        ParentZone = transform.GetComponentInParent<ZoneManagment>();
    }

    void Update()
    {

    }

    public void TaskAccomplished()
    {
        currentOrder.IsBeingPrepared = false;
        IsBusy = false;
        destinations = null;
        currentDestIndex = 0;
        ParentZone.TaskAccomplished(currentOrder);
    }

    public void DrawResource()
    {
        ParentZone.DrawResource();
    }

    public void BeginTask(Workstation station, List<float> waitTimesList, Order order)
    {
        IsBusy = true;
        shouldBeginTask = true;
        currentOrder = order;
        workstation = station;
        waitTimes = waitTimesList;
    }
    public bool ShouldBeginTask(out Workstation station, out List<float> waitTimesList, out Order order)
    {
        order = currentOrder;
        waitTimesList = waitTimes;
        station = workstation;

        if (shouldBeginTask)
        {
            shouldBeginTask = false;
            return true;
        }
        return false;
    }
}
