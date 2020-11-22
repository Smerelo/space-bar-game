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
}
