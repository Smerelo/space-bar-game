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
    private float dayClock;
    private bool timeStop;
    private float minutes;
    public int DayCounter { get; set; }
    public bool dayFinished { get; private set; }

    [SerializeField] private GameObject endOfDayMenu;
    private CentralTransactionLogic CTL;


    void Start()
    {
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
        PauseGame();
        ShowEndOfDayMenu();
    }

    private void PauseGame()
    {
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

  /*  if ((customerManager.minutes > shiftEnd) && !ended)
        {
            ended = true;
            customerManager.StopTimer();
            gO.SetActive(true);
            //gameOver.GetGameStatus(moneyBalance, customerManager.minutes, positiveBalanceColor, negativeBalanceColor);
        }*/
}
