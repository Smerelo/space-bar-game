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
    public float moveSpeed { get; set; }
    public float taskSpeed { get; set; }
    public float salary { get; set; }
    public int employeeNumber { get; private set; }
    
    private void Start()
    {
        employeeManager = GameObject.Find("HeadEmployees").GetComponent<HeadEmployeeManager>();
    }

    public void InstantiateEmployee(Cv cv)
    {
        moveSpeed = cv.moveSpeed;
        taskSpeed = cv.taskSpeed;
        salary = cv.price;
        employeeNumber = employeeManager.employeeList.Count + 1;
    } 
   
}
