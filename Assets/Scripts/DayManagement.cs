using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayManagement : MonoBehaviour
{
    [SerializeField] private float employeePrice;
    [SerializeField] private float customerPatience;
    [SerializeField] private float customerTraffic;
    [SerializeField] private float foodPrices;
    [SerializeField] private float foodQuantity;
    [SerializeField] private float dayStart;
    [SerializeField] private float dayEnd;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject MobileUi;
    [SerializeField] private EmployeeCard[] employeeCards;

    private float dayClock;
    private bool timeStop;
    private float minutes;
    public int DayCounter { get; set; }
    public bool dayFinished { get; private set; }
    private TableManager tableManager;

    [SerializeField] private GameObject endOfDayMenu;
    private CentralTransactionLogic CTL;


    void Start()
    {
        tableManager = GameObject.Find("TableManager").GetComponent<TableManager>();
        CTL = GameObject.Find("SpaceCantina").GetComponent<CentralTransactionLogic>();
        dayClock = dayStart;
        endOfDayMenu.SetActive(false);
    }

    void Update()
    {
        if (!dayFinished)
        {
            dayClock += 3 * Time.deltaTime;
            if (dayClock >= dayEnd)
            {
                EndDay();
            }
            UpdateClock();
        }
    }

    private void EndDay()
    {
        for (int i = 0; i < employeeCards.Length; i++)
        {
            employeeCards[i].GenerateStats();
        }
        PauseGame();
        ShowEndOfDayMenu();
    }

    private void PauseGame()
    {
        tableManager.FreeTables();
        MobileUi.SetActive(false);
        dayFinished = true;
        CTL.EmployeeClockOut();
    }

    private void ShowEndOfDayMenu()
    {
        endOfDayMenu.SetActive(true);
        endOfDayMenu.GetComponent<EndOfDay>().SetGameStatus(CTL.GetBalance()); 
    }

    public void NextDay()
    {
        dayClock = dayStart;
        CTL.EmployeeClockIn();
        dayFinished = false;
        endOfDayMenu.SetActive(false);
    }

    private void UpdateClock()
    {
        string hour;
        string minute;
        hour = ZeroPadding(Mathf.FloorToInt(dayClock / 60));
        minute = ZeroPadding(Mathf.FloorToInt(dayClock) % 60);
        text.text = hour + ':' + minute;
    }

    private string ZeroPadding(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }
}
