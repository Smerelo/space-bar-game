using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadEmployeeManager : MonoBehaviour
{
    [HideInInspector] public List<HeadEmployee> employeeList;
    public Transform spawn;
    public int number = 1;
    internal void HireEmployee(HeadEmployee employee)
    {
        employeeList.Add(employee);
    }

    internal void FireEmployee(HeadEmployee headEmployee)
    {
        employeeList.Remove(headEmployee);
    }

    internal HeadEmployee GetRandomEmployee()
    {
        if (employeeList.Count == 0)
        {
            return null;
        }
        return employeeList[UnityEngine.Random.Range(0, employeeList.Count)];
    }
}
