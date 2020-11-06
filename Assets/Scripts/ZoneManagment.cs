using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManagment : MonoBehaviour
{
    [SerializeField] private RessourceZone input;
    [SerializeField] private RessourceZone output;
    [SerializeField] private string zoneName;
    [SerializeField] private Transform stationsTrasform;
    [SerializeField] private Transform waitingZone;
    [SerializeField] private Transform spawnZone;
    [SerializeField] private float employeeSalary;
    private float startingSalary;
    [SerializeField] private bool test;
    [SerializeField] GameObject workerPrefab;
    private List<Workstation> stations;
    private List<EmployeeBehaviour> employees;
    private List<Order> orders;
    private CentralTransactionLogic zoneManager;
    private SEManager superEmployeeManager;
    void Start()
    {
        startingSalary = employeeSalary;
        zoneManager = GetComponentInParent<CentralTransactionLogic>();
        superEmployeeManager = GameObject.Find("SEManager").GetComponent<SEManager>();
        orders = new List<Order>();
        employees = new List<EmployeeBehaviour>();

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out EmployeeBehaviour employee))
            {
                employees.Add(employee);
            }
        }
        stations = new List<Workstation>();
        foreach (Transform s in stationsTrasform)
        {
            if (s.TryGetComponent(out Workstation station))
            {
                stations.Add(station);
            }
        }
    }
    public void CashIn(float amount)
    {
        zoneManager.CashIn(amount);
    }
    public float GiveSalary()
    {
        float total = 0f;
        foreach (EmployeeBehaviour emp in employees)
        {
            total += employeeSalary * Time.deltaTime / 60;
        }
        return total;
    }

    void Update()
    {
        TaskVarietyShuffle(UnityEngine.Random.Range(0, 2));
    }

    private void TaskVarietyShuffle(int rnd)
    {
        if (rnd == 0)
        {
            if (ShouldBeginTask(out Workstation workstation, out EmployeeBehaviour employee, out Order order))
            {
                input.SetAsideRessources(1);
                order.IsBeingPrepared = true;
                BeginTask(workstation, employee, order);
            }
            if (ShouldBringDirtyPlates(out Order o, out Waiter waiter))
            {
                o.IsBeingTakenToClean = true;
                waiter.BringDirtyPlates(o);
            }
        }
        else
        {
            if (ShouldBringDirtyPlates(out Order o, out Waiter waiter))
            {
                o.IsBeingTakenToClean = true;
                waiter.BringDirtyPlates(o);
            }
            if (ShouldBeginTask(out Workstation workstation, out EmployeeBehaviour employee, out Order order))
            {
                input.SetAsideRessources(1);
                order.IsBeingPrepared = true;
                BeginTask(workstation, employee, order);
            }
        }
        //print($"{zoneName}: {orders.Count} orders here");
    }

    internal void ClockIn()
    {
        input.ResetRessources();
    }

    internal void ClockOut()
    {
        foreach (EmployeeBehaviour employee in employees)
        {
            employee.AbortOrder();
        }
        orders.Clear();
    }

    public bool ShouldBringDirtyPlates(out Order order, out Waiter waiter)
    {
        order = null;
        waiter = null;
        EmployeeBehaviour employee = FindFreeEmployee();


        foreach (Order o in orders)
        {
            if (o.Customer.HasFinishedEating)
            {
                order = o;
            }
        }
        if (employee == null || order == null || order.IsBeingTakenToClean)
        {
            return false;
        }
        if (employee.TryGetComponent(out Waiter w))
        {
            waiter = w;
            return true;
        }
        return false;
    }
    public bool ShouldBeginTask(out Workstation workstation, out EmployeeBehaviour employee, out Order order)
    {
        workstation = FindUnoccupiedStation();
        employee = FindFreeEmployee();
        order = CheckForOrders();

        if (workstation == null || employee == null || order == null || order.IsBeingPrepared || input.AvailableRessources <= 0)
        {
            return false;
        }
        return true;
    }



    public void TaskAccomplished(Order order)
    {
        orders.Remove(order);
        output.AddRessources(1);
        zoneManager.SendToNextZone(order, zoneName);
    }
    internal void DrawResource()
    {
        input.RemoveRessources(1);
    }

    public void BeginTask(Workstation station, EmployeeBehaviour employee, Order order)
    {
        employee.BeginTask(station, order);
        test = false;
    }

    private EmployeeBehaviour FindFreeEmployee()
    {
        return DrawRandomAvailable<EmployeeBehaviour>(employees, (EmployeeBehaviour e) => !e.IsBusy);
    }

    private Workstation FindUnoccupiedStation()
    {
        return DrawRandomAvailable<Workstation>(stations, (Workstation station) => !station.InUse);
    }
    private Order CheckForOrders()
    {
        return DrawRandomAvailable<Order>(orders, (Order order) => !order.IsBeingPrepared);
    }

    private T DrawRandomAvailable<T>(List<T> list, Func<T, bool> check)
    {
        List<int> indexes = new List<int>(list.Count);
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = UnityEngine.Random.Range(0, list.Count);
            while (indexes.Contains(rnd))
            {
                rnd = UnityEngine.Random.Range(0, list.Count);
            }
            indexes.Add(rnd);
        }
        foreach (int index in indexes)
        {
            if (check(list[index]))
            {
                return list[index];
            }
        }
        return default(T);
    }

    public void HireEmployee()
    {
        GameObject newEmployee = Instantiate(workerPrefab, spawnZone.position, Quaternion.identity, transform);
        employees.Add(newEmployee.GetComponent<EmployeeBehaviour>());
    }

    public void UpgradeEmployee()
    {
        if (employees.Count == 0)
        {
            HireEmployee();
        }
        else
        {
            employees[0].SalaryRaise();
            employeeSalary += startingSalary;
        }
    }

    public Transform GetInputPos()
    {
        return input.Output;
    }

    public void AssignEmployee(GameObject employee)
    {

    }

    internal Transform GetOutputPos()
    {
        return output.Input;
    }

    internal Transform GetWaitingZone()
    {
        return waitingZone;
    }
    internal string GetName()
    {
        return zoneName;
    }
    public void AddOrder(Order order)
    {
        orders.Add(order);
    }
    public void TurnInOrder(Order order)
    {

    }

    public void Yell()
    {
        foreach (EmployeeBehaviour employee in employees)
        {
            employee.GotYelledAt = true;
        }
    }

    internal void RemoveSuperEmployee(EmployeeBehaviour superEmployee)
    {
        employees.Remove(superEmployee);
    }
    internal void AddSuperEmployee(EmployeeBehaviour superEmployee) 
    {
        employees.Add(superEmployee);
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            superEmployeeManager.SelectZone(this);
        }
    }
}
