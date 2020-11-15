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
        totalText.text = $"SPENDING:${totalPrice}/hour";
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
            closePos.position = sideMenu.transform.position;
            openPos.localPosition = new Vector3(-720f, sideMenu.transform.localPosition.y, sideMenu.transform.localPosition.z);
            arrowButton.transform.localScale = scale;
            LeanTween.moveLocalX(sideMenu, openPos.localPosition.x, 0.3f);
            openSideMenu = true;
        }

        else
        {
            Debug.Log($"enters{closePos.localPosition.x}");
            arrowButton.transform.localScale = scale;
            LeanTween.moveLocalX(sideMenu, closePos.localPosition.x, 0.3f);
            openSideMenu = false;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (openSideMenu == true)
        {
        }
        else
        {
        }
    }
}
