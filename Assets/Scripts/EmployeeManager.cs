using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EmployeeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bartender;
    [SerializeField] private TextMeshProUGUI waiter;
    [SerializeField] private TextMeshProUGUI cleaner;
    [SerializeField] private TextMeshProUGUI bartenderCountText;
    [SerializeField] private TextMeshProUGUI waiterCountText;
    [SerializeField] private TextMeshProUGUI cleanerCountText;
    [SerializeField] private TextMeshProUGUI totalText;
    [SerializeField] private GameObject sideMenu;
    [SerializeField] private Image arrowButton;
    [SerializeField] private Transform openPos;
    [SerializeField] private Transform closePos;
    [SerializeField] private float speed;


    private ZoneManagment barZone;
    private ZoneManagment waiterZone;
    private ZoneManagment cleanerZone;
    private float salaryBartender;
    private float salaryWaiter;
    private float salaryCleaner;
    private bool openSideMenu = false;
    // Start is called before the first frame update
    void Start()
    {
        barZone = GameObject.Find("Preparing").GetComponent<ZoneManagment>();
        waiterZone = GameObject.Find("Serving").GetComponent<ZoneManagment>();
        cleanerZone = GameObject.Find("Cleaning").GetComponent<ZoneManagment>();
        UpdateSalaryText();
        UpdateCount();
        UpdateSalary();
    }

    public void UpdateSalary()
    {
        salaryBartender = barZone.GetEmployeeSalary(0);
        salaryWaiter = waiterZone.GetEmployeeSalary(0);
        salaryCleaner = cleanerZone.GetEmployeeSalary(1);
        UpdateSalaryText();
        UpdateTotal();
    }

    public void UpdateCount()
    {
        bartenderCountText.text = barZone.GetEmployeeCount().ToString();
        waiterCountText.text = waiterZone.GetEmployeeCount().ToString();
        cleanerCountText.text = cleanerZone.GetUpgradeCount().ToString();
    }

    public void UpdateTotal()
    {
        float totalPrice = salaryBartender + salaryCleaner + salaryWaiter;
        totalText.text = $"TOTAL: ${totalPrice}/hour";
    }

    public void UpdateSalaryText()
    {
        bartender.text = $"${salaryBartender}/hour";
        waiter.text = $"${salaryWaiter}/hour";
        cleaner.text = $"${salaryCleaner}/hour";
    }

    public void ShowSideMenu()
    {
        Vector3 scale = new Vector3(-arrowButton.transform.localScale.x, arrowButton.transform.localScale.y, 0);

        if (!openSideMenu)
        {
            arrowButton.transform.localScale = scale;
            openSideMenu = true;
        }

        else
        {
            arrowButton.transform.localScale = scale;
            openSideMenu = false ;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (openSideMenu == true)
        {
            sideMenu.transform.position = Vector2.MoveTowards(sideMenu.transform.position, openPos.position, speed *Time.deltaTime);
        }
        else
        {
            sideMenu.transform.position = Vector2.MoveTowards(sideMenu.transform.position, closePos.position, speed * Time.deltaTime);
        }
    }
}
