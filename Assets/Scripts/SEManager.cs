using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{
    private bool employeeSelected = false;
    private SuperEmployee selectedEmployee;

    internal void SelectEmployee(SuperEmployee employee)
    {
        if (!employeeSelected)
        {
            employeeSelected = true;
            selectedEmployee = employee;
        }
    }

    internal void SelectZone(ZoneManagment zone) 
    {
        if (employeeSelected)
        {
            selectedEmployee.GetNewZone(zone);
            employeeSelected = false;
            selectedEmployee = null;
        }
    }
}
