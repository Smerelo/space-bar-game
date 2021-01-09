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
    [SerializeField] private float timeMultiplier = 3;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject MobileUi;
    [SerializeField] private EmployeeCard[] employeeCards;
    [SerializeField] private TextMeshProUGUI dayCountText;
    [SerializeField] private Boss currentBoss;
    [SerializeField] private Animator transition;
    [SerializeField] private GameObject banner;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject menu2;
    [SerializeField] private GameObject endOfDayMenu;
    [HideInInspector] public bool dayFinished { get; private set; }
    [HideInInspector] public bool bossActive { get; private set; }



    private float dayClock;
    private bool timeStop;
    private float minutes;
    private AudioManager audio;
    private OrderList orders;
    public int dayCounter { get; set; }
    public bool PlayingCinematic { get; private set; }

    private TableManager tableManager;

    private CentralTransactionLogic CTL;


    void Start()
    {
        audio = GameObject.Find("Main Camera").GetComponent<AudioManager>();
        dayCounter = 3;
        tableManager = GameObject.Find("TableManager").GetComponent<TableManager>();
        CTL = GameObject.Find("SpaceCantina").GetComponent<CentralTransactionLogic>();
        dayClock = dayStart;
        endOfDayMenu.SetActive(false);
        orders = GameObject.Find("OrderList").GetComponent<OrderList>();
    }

    void Update()
    {
        if (dayCounter == 0)
        {
            EndDay();
        }

        if (Input.GetKeyDown("q"))
        {
            EndDay();
        }
        if (!dayFinished)
        {
            if (!PlayingCinematic)
            {
                dayClock += timeMultiplier * Time.deltaTime;
                if (!bossActive && dayClock >= dayEnd)
                {
                    EndDay();
                }
                UpdateClock();
            }
        }
    }

    private void EndDay()
    {
        dayCounter++;
        for (int i = 0; i < employeeCards.Length; i++)
        {
            employeeCards[i].ResetCard();
        }
        PauseGame();
        PlayTransition();
    }

    private void PlayTransition()
    {
        //Cursor.visible = true;
        endOfDayMenu.SetActive(true);
        transition.Play("Transition");
        Invoke("ShowEndOfDayMenu", 1.3f);
    }

    private void StartBossFight()
    {
        currentBoss.BossCinematic();
    }

  

    public void PauseGame()
    {
        CTL.EmployeeClockOut();
        orders.ClearOrders();
        tableManager.FreeTables();
       // MobileUi.SetActive(false);
        dayFinished = true;
    }

    private void ShowEndOfDayMenu()
    {
        audio.PlayMenuMusic();
        menu.SetActive(true);
        banner.SetActive(true);
        transition.Play("Iddle_BackGround");
        endOfDayMenu.GetComponent<EndOfDay>().SetGameStatus(CTL.GetBalance());
    }

    public void NextDay()
    {
       // Cursor.visible = false;
        transition.Play("Inverse_Transition");
        menu.SetActive(false);
        menu2.SetActive(false);
        banner.SetActive(false);
        Invoke("StartDay", 1.3f);

    }


    private void StartDay()
    {
        if (dayCounter == 5)
        {
            bossActive = true;
            StartBossFight();
        }
        dayClock = dayStart;
        CTL.EmployeeClockIn();
        dayFinished = false;
        endOfDayMenu.SetActive(false);
        if (!bossActive)
        {
            audio.ResumeNormalMusic();
        }
    }
    private void UpdateClock()
    {
        string hour;
        string minute;
        hour = ZeroPadding(Mathf.FloorToInt(dayClock / 60));
        minute = ZeroPadding(Mathf.FloorToInt(dayClock) % 60);
        text.text = hour + ':' + minute;
    }

    public void UpdateDayCounter()
    {
        dayCountText.text = "DAY:" + dayCounter.ToString(); 
    }

    private string ZeroPadding(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }
}
