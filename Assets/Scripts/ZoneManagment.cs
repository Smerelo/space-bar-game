using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManagment : MonoBehaviour
{
    [SerializeField] public RessourceZone input;
    [SerializeField] public RessourceZone output;
    [SerializeField] private string zoneName;
    [SerializeField] private Transform stationsTrasform;
    [SerializeField] private Transform waitingZone;
    [SerializeField] private Transform spawnZone;
    [SerializeField] public float employeeSalary;
    [SerializeField] private AlertArrow arrow;
    private float startingSalary;
    [SerializeField] private bool test;
    [SerializeField] GameObject workerPrefab;
    private List<Workstation> stations;
    private List<EmployeeBehaviour> employees;
    private List<EmployeeBehaviour> headEmployees;
    private List<Order> orders;
    private List<Order> waitList;
    private CentralTransactionLogic zoneManager;
    private EmployeeManager employeeManager;
    private int upgradeCount = 0;

    public bool SoundStopped { get; private set; }
    public bool SoundPlayed { get; private set; }

    void Start()
    {
        SoundStopped = true;
        startingSalary = employeeSalary;
        zoneManager = GetComponentInParent<CentralTransactionLogic>();
        orders = new List<Order>();
        waitList = new List<Order>();
        employees = new List<EmployeeBehaviour>();
        headEmployees = new List<EmployeeBehaviour>();

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out EmployeeBehaviour employee))
            {
                if (employee.IsHeadEmployee == false)
                {
                    employees.Add(employee);
                }
                if (employee.IsHeadEmployee == true) 
                {
                    headEmployees.Add(employee);
                }
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
        //employeeManager = GameObject.Find("SideMenu").GetComponent<EmployeeManager>();
    }

    internal GameObject GetRandomEmployee()
    {
        if (headEmployees.Count == 0)
        {
            return null;
        }
        return headEmployees[UnityEngine.Random.Range(0, employees.Count)].gameObject;
    }

    public void CashIn(float amount)
    {
        zoneManager.CashIn(amount + 5);
    }
    public float GiveSalary()
    {
        float total = 0f;
        foreach (EmployeeBehaviour emp in employees)
        {
            total += employeeSalary * Time.deltaTime / 60;
        }
        foreach (EmployeeBehaviour headEmp in headEmployees)
        {
            total += headEmp.Salary * Time.deltaTime / 60;
            if (headEmp.Salary == 0)
            {
                Debug.LogWarning("Head employee salary is 0");
            }
        }
        return total;
    }

    void Update()
    {
        //TaskVarietyShuffle(UnityEngine.Random.Range(0, 2));
        if (headEmployees.Count > 0)
        {
            CheckCurrentOrders();
        }
        if (zoneName == Constants.serving && waitList.Count > 0)
        {

            CheckWaitList();
        }
        if (zoneName == Constants.cleaning)
        {
            CheckForDishes();
        }

    }

    private void CheckForDishes()
    {
        if (output.RessourceQuantity == 0 && input.RessourceQuantity > 0 && !SoundPlayed)
        {
            SoundPlayed = true;
            SoundStopped = false; 
            arrow.gameObject.SetActive(true);
            arrow.PlaySound();
        }
    }

    public void PlaySound()
    {
        arrow.gameObject.SetActive(true);
        arrow.PlaySound();
        SoundStopped = false;
    }

    public void StopSound()
    {
        if (zoneName == Constants.cleaning && !SoundStopped)
        {
            Debug.Log("here");  
            arrow.gameObject.SetActive(false);
            arrow.StopSound();
            SoundStopped = true;
        }
    }

    private void CheckWaitList()
    {
        foreach (Order order in waitList)
        {
            if (order.IsBeingTakenToClean && !order.IsAssigned)
            {
                Workstation workstation = order.GetTable();
                EmployeeBehaviour employee = GetEmployee();
                if (employee != null)
                {
                    order.IsAssigned = true;
                    employee.BeginTask(workstation, order);
                }

            }
        }
    }

    private void CheckCurrentOrders()
    {
        Workstation workstation;
        foreach (Order order  in orders)
        {
            if (!order.IsAssigned)
            {
                if (zoneName == Constants.preparing)
                {
                    workstation = FindUnoccupiedStation();
                }
                else
                    workstation = order.GetTable();
                if (workstation != null)
                {
                    EmployeeBehaviour employee = GetEmployee();
                    if(employee != null)
                    {
                        order.IsAssigned = true;
                        employee.BeginTask(workstation, order);
                    }
                   
                }
            }
        }
    }

    public Workstation GetFreeWorkStation()
    {
        foreach (Workstation workstation in stations)
        {
            if (!workstation.InUse)
            {
                return workstation;
            }
        }
        return null;
    }

    private EmployeeBehaviour GetEmployee()
    {
        foreach (EmployeeBehaviour employee in headEmployees)
        {
            if (!employee.IsBusy)
            {
                return employee;
            }
        }
        return null;
    }

    internal void RemoveEmployee(GameObject employee)
    {
        headEmployees.Remove(employee.GetComponent<EmployeeBehaviour>());
        Destroy(employee);
    }


    internal void RemoveSuperEmployee(EmployeeBehaviour superEmployee)
    {
        headEmployees.Remove(superEmployee);
    }
    internal void AddSuperEmployee(EmployeeBehaviour superEmployee)
    {
        headEmployees.Add(superEmployee);
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
        print($"{zoneName}: {orders.Count} orders here");
    }

    internal void CheckAndRemoveOrder(Order order)
    {
        foreach (Order order1 in orders)
        {
            if (order == order1)
            {
                orders.Remove(order);
                break;
            }
        }
    }

    internal void ClockIn()
    {
        input.ResetRessources();
    }

    internal void ClockOut()
    {
        foreach (EmployeeBehaviour employee in headEmployees)
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

    public int GetRessourceQuantity()
    {
        return input.RessourceQuantity;
    }

    public float GetEmployeeSalary(int mod)
    {
        if (mod == 0)
        {
            return employees.Count * startingSalary;
        }
        return upgradeCount * startingSalary;
    }


    public int GetEmployeeCount()
    {
        return employees.Count;
    }
    public void TaskAccomplished(Order order)
    {
        orders.Remove(order);
        if (order.IsBeingTakenToClean)
        {
            waitList.Remove(order);
        }
        output.AddRessources(1);
        if (!order.IsBeingTakenToClean)
        {
            zoneManager.SendToNextZone(order, zoneName);
        }
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
        EmployeeBehaviour freeEmployee = DrawRandomAvailable<EmployeeBehaviour>(headEmployees, (EmployeeBehaviour e) => !e.IsBusy);
        return freeEmployee;
    }

    public Workstation FindUnoccupiedStation()
    {
        return DrawRandomAvailable<Workstation>(stations, (Workstation station) => !station.InUse);
    }

    internal void OrderDone(Order currentOrder)
    {
        orders.Remove(currentOrder);
        waitList.Add(currentOrder);
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

    internal void DiscardOrder(Order currentOrder)
    {
        orders.Remove(currentOrder);
        output.AddRessources(1);
    }

    public void HireEmployee()
    {
        GameObject newEmployee = Instantiate(workerPrefab, spawnZone.position, Quaternion.identity, transform);
        EmployeeBehaviour newEmployeeBehaviour = newEmployee.GetComponent<EmployeeBehaviour>();
        newEmployeeBehaviour.IsHeadEmployee = false;
        employees.Add(newEmployeeBehaviour);
    }

    public void FireEmployee()
    {
        if (employees.Count != 0)
        {
            GameObject employee = employees[0].gameObject;
            employees.RemoveAt(0);
            GameObject.Destroy(employee);
        }
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
        upgradeCount += 1;
    }

    public void DowngradeEmployee()
    {
        if (upgradeCount == 0)
        {

        }
        else if (upgradeCount == 1)
        {
            FireEmployee();
            upgradeCount -= 1;
            employeeSalary -= startingSalary;
        }
        else
        {
            employees[0].Downgrade();
            upgradeCount -= 1;
            employeeSalary -= startingSalary;
        }
    }
    public int GetUpgradeCount()
    {
        return upgradeCount;
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
        order.Zone = zoneName;
        orders.Add(order);
    }
    public void TurnInOrder(Order order)
    {

    }

    public void Yell()
    {
        foreach (EmployeeBehaviour employee in headEmployees)
        {
            employee.GotYelledAt = true;
        }
    }
}
