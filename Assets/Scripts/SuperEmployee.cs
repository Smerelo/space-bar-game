using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperEmployee : MonoBehaviour
{
    private EmployeeBehaviour employeeBehaviour;
    private SEManager superEmployeeManager;
    private Waiter waiterScript;
    private Barman barmanScript;
    
    private ZoneManagment newZone;
    private ZoneManagment oldZone;

    private bool shouldChangeZone = false;

    // Start is called before the first frame update
    void Awake()
    {
        employeeBehaviour = GetComponent<EmployeeBehaviour>();
        waiterScript = GetComponent<Waiter>();
        barmanScript = GetComponent<Barman>();
        waiterScript.enabled = false;
    }

    private void Start()
    {
        superEmployeeManager = GameObject.Find("SEManager").GetComponent<SEManager>();
        oldZone = employeeBehaviour.ParentZone;
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
        employeeBehaviour.ChangeParentZone(transform.GetComponentInParent<ZoneManagment>());
        oldZone.RemoveSuperEmployee(employeeBehaviour);
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

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Debug.Log("has been selected");
            superEmployeeManager.SelectEmployee(this);
        }
    }

    
}
