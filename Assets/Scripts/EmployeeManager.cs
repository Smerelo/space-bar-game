using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EmployeeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bartender;
    [SerializeField] private TextMeshProUGUI waiter;
    [SerializeField] private TextMeshProUGUI cleaner;
    [SerializeField] private TextMeshProUGUI bartenderCountText;
    [SerializeField] private TextMeshProUGUI waiterCountText;
    [SerializeField] private TextMeshProUGUI cleanerCountText;
    [SerializeField] private TextMeshProUGUI totalText;
    private ZoneManagment barZone;
    private ZoneManagment waiterZone;
    private ZoneManagment cleanerZone;
    private float salaryBartender;
    private float salaryWaiter;
    private float salaryCleaner;
    // Start is called before the first frame update
    void Start()
    {
        barZone = GameObject.Find("Preparing").GetComponent<ZoneManagment>();
        waiterZone = GameObject.Find("Serving").GetComponent<ZoneManagment>();
        cleanerZone = GameObject.Find("Cleaning").GetComponent<ZoneManagment>();
        UpadateSalaryText();
        UpdateCount();
        UpdateSalary();
    }

    public void UpdateSalary()
    {
        salaryBartender = barZone.GetEmployeeSalary(0);
        salaryWaiter = waiterZone.GetEmployeeSalary(0);
        salaryCleaner = cleanerZone.GetEmployeeSalary(1);
        Debug.Log($"bar: {salaryBartender} waiter: { salaryWaiter} cleaner: {salaryCleaner}");
    }

    public void UpdateCount()
    {
        bartenderCountText.text = barZone.GetEmployeeCount().ToString();
        waiterCountText.text = waiterZone.GetEmployeeCount().ToString();
        cleanerCountText.text = cleanerZone.GetUpgradeCount().ToString();
    }

    public void UpdateTotal()
    {
        float totalPricel = salaryBartender + salaryCleaner + salaryWaiter;
        totalText.text = $"TOTAL: ${salaryBartender}/hour";
    }

    public void UpadateSalaryText()
    {
        bartender.text = $"${salaryBartender}/hour";
        waiter.text = $"${salaryWaiter}/hour";
        cleaner.text = $"${salaryCleaner}/hour";
    }


    // Update is called once per frame
    void Update()
    {
        UpdateSalary();
    }
}
