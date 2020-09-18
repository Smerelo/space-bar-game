using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class CentralTransactionLogic : MonoBehaviour
{
    public List<Order> orders;
    private Dictionary<string, ZoneManagment> zones;
    private double moneyBalance;
    [SerializeField] double startingBalance;
    [SerializeField] private TextMeshProUGUI moneyCounterInt;
    private CustomerManager customerManager;
    private float shiftEnd;
    private GameOver gameOver;
    private GameObject gO;
    private bool ended;

    void Start()
    {
        moneyBalance = startingBalance;
        gameOver = GameObject.Find("GameOver").GetComponent<GameOver>();
        gO =  GameObject.Find("GameOver");
        gO.SetActive(false);
        customerManager = GameObject.Find("Customer Manager").GetComponent<CustomerManager>();
        shiftEnd = customerManager.dayLenght  * 60 +  10 * 60;
        orders = new List<Order>();
        zones = new Dictionary<string, ZoneManagment>();
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out ZoneManagment zoneManagment))
            {
                if (zoneManagment.GetName() == "")
                    Debug.Log("Put a name on your zones pls");
                zones.Add(zoneManagment.GetName(), zoneManagment);
            }
        }
    }
    public void CashIn(float amount)
    {
        moneyBalance += amount;
    }
    private void CheckBalance()
    {
        if ((moneyBalance <= 0 || customerManager.minutes > shiftEnd) && !ended)
        {
            ended = true;
            if (moneyBalance < 0)
            {
                moneyBalance = 0;
            }
            customerManager.StopTimer();
            gO.SetActive(true);
            gameOver.GetGameStatus(moneyBalance, customerManager.minutes);
        }
        foreach (ZoneManagment zone in zones.Values)
        {
            moneyBalance -= zone.GiveSalary();
        }
        int decimals = (int)Math.Round(100.0 * (moneyBalance - Math.Truncate(moneyBalance)));
        moneyCounterInt.text = $"${Math.Truncate(moneyBalance)}<size=-14>{decimals.ToString("D2")}</size>";
    }

    void Update()
    {
        CheckBalance();
    }

    public void AddOrder(Order order)
    {
        orders.Add(order);
        if (zones.TryGetValue(Constants.preparing, out ZoneManagment preparing))
        {
            preparing.AddOrder(order);
        }
    }

    internal void SendToNextZone(Order order, string zoneName)
    {
        order.ResetBools();
        switch (zoneName)
        {
            case Constants.preparing:
                if (zones.TryGetValue(Constants.serving, out ZoneManagment serving))
                {
                    serving.AddOrder(order);
                }
                break;
            case Constants.serving:
                if (zones.TryGetValue(Constants.cleaning, out ZoneManagment cleaning))
                {
                    cleaning.AddOrder(order);
                }
                break;
            case Constants.cleaning:
                break;
            default:
                break;
        }
    }
}
