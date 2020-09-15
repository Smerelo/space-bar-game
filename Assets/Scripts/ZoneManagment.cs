using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManagment : MonoBehaviour
{
    [SerializeField] private GameObject input;
    [SerializeField] private GameObject output;
    [SerializeField] private string zoneName;
    private List<Workstation> stations;
    [SerializeField] private Transform stationsTrasform;

    private List<GameObject> employees;

    private List<EmployeeBehaviour> employeesScripts;
    [SerializeField] private Transform waitingZone;
    [SerializeField] private bool test;
    [SerializeField] List<float> waitTimes;
    [SerializeField] GameObject workerPrefab;

    void Start()
    {
        employees = new List<GameObject>();
        employeesScripts = new List<EmployeeBehaviour>();

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out EmployeeBehaviour employee))
            {
                employees.Add(child.gameObject);
                employeesScripts.Add(employee);
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

    void Update()
    {
        if (test)
        {
            BeginTask(FindUnoccupiedStation(), FindFreeEmployee());
        }
    }

    public bool ShouldBeginTask(out Workstation workstation, out EmployeeBehaviour employee)
    {
        workstation = FindUnoccupiedStation();
        employee = FindFreeEmployee();
        if (workstation == null || employee == null)
        {
            return false;
        }
        return true;
    }

    public void TaskAccomplished()
    {

    }
    public void BeginTask(Workstation station, EmployeeBehaviour employee)
    {
        employee.BeginTask(station, waitTimes);
        test = false;
    }
    private EmployeeBehaviour FindFreeEmployee()
    {
        foreach (EmployeeBehaviour employee in employeesScripts)    
        {
            if (!employee.IsBusy)
            {
                return employee;
            }
        }
        return null;
    }

    private Workstation FindUnoccupiedStation()
    {
        foreach (Workstation station in stations)
        {
            if (!station.InUse)
            {
                return station;
            }
        }
        return null;
    }
    public void HireEmployee()
    {
        GameObject newEmployee = Instantiate(workerPrefab, waitingZone.position, Quaternion.identity, transform);
        employees.Add(newEmployee);
        employeesScripts.Add(newEmployee.GetComponent<EmployeeBehaviour>());
    }

    public Transform GetInputPos()
    {
        return input.transform;
    }

    public void AssignEmployee(GameObject employee)
    {

    }

    internal Transform GetOutputPos()
    {
        return output.transform;
    }

    internal Transform GetWaitingZone()
    {
        return waitingZone;
    }
    internal string GetName()
    {
        return zoneName;
    }
}
