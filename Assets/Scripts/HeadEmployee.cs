using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HeadEmployee : MonoBehaviour
{
    private HeadEmployeeManager employeeManager;
    public float Salary { get; set; }
    public int EmployeeNumber { get; private set; }
    private EmployeeBehaviour employeeBehaviour;
    private Waiter waiterScript;
    private Barman barmanScript;

    private ZoneManagment newZone;
    private ZoneManagment oldZone;

    public bool shouldChangeZone = false;


    void Awake()
    {
        employeeBehaviour = GetComponent<EmployeeBehaviour>();
        waiterScript = GetComponent<Waiter>();
        barmanScript = GetComponent<Barman>();
        waiterScript.enabled = false;
        barmanScript.enabled = false;
        employeeBehaviour.IsHeadEmployee = true;
    }

    private void Start()
    {
        employeeManager = GameObject.Find("HeadEmployees").GetComponent<HeadEmployeeManager>();
        oldZone = null;
        employeeBehaviour.Salary = Salary;
    }

    public void InstantiateEmployee(Cv cv)
    {
        employeeManager = GameObject.Find("HeadEmployees").GetComponent<HeadEmployeeManager>();
        waiterScript.movementSpeed = cv.moveSpeed;
        waiterScript.TaskSpeed = cv.taskSpeed;
        barmanScript.movementSpeed = cv.moveSpeed;
        barmanScript.TaskSpeed = cv.taskSpeed;
        Salary = cv.price;
        EmployeeNumber = employeeManager.number + 1;
        employeeBehaviour.IsHeadEmployee = true;
        employeeBehaviour.Salary = cv.price;
    }

    private void Update()
    {
        if (shouldChangeZone && !employeeBehaviour.IsBusy)
        {
            shouldChangeZone = false;
            ChangeZone();
        }
    }

    private void ChangeZone()
    {
        this.transform.parent = newZone.transform;
        employeeBehaviour.ParentZone = newZone;
        if (oldZone != null)
        {
            oldZone.RemoveSuperEmployee(employeeBehaviour);
        }
        newZone.AddSuperEmployee(employeeBehaviour);
        if (newZone.gameObject.name == "Preparing")
        {
            waiterScript.enabled = false;
            barmanScript.enabled = true;
        }
        if (newZone.gameObject.name == "Serving")
        {
            barmanScript.enabled = false;
            waiterScript.enabled = true;
        }
        oldZone = newZone;
    }

    internal void GetNewZone(ZoneManagment zone)
    {
        shouldChangeZone = true;
        newZone = zone;
    }



}
