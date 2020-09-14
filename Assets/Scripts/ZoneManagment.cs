using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManagment : MonoBehaviour
{
    [SerializeField] private GameObject input;
    [SerializeField] private GameObject output;
    [SerializeField] private string zoneName;
    private List<GameObject> employees;
    private List<EmployeeBehaviour> employeesScripts; 

    void Start()
    {
        foreach (GameObject child in transform)
        {
            if (child.TryGetComponent(out EmployeeBehaviour employee))
            {
                employees.Add(child);
                employeesScripts.Add(employee);
            }
        }
    }

    void Update()
    {

    }

    public void TaskAccomplished()
    {

    }
    public 
    void BeginTask()
    {

    }
    public void HireEmployee()
    {
    }

    public void AssignEmployee(GameObject employee)
    {

    }
}
