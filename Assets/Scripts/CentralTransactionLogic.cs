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
    [SerializeField] private Color positiveBalanceColor;
    [SerializeField] private Color negativeBalanceColor;
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
        if ((customerManager.minutes > shiftEnd) && !ended)
        {
            ended = true;
            customerManager.StopTimer();
            gO.SetActive(true);
            gameOver.GetGameStatus(moneyBalance, customerManager.minutes, positiveBalanceColor, negativeBalanceColor);
        }
        foreach (ZoneManagment zone in zones.Values)
        {
            moneyBalance -= zone.GiveSalary();
        }
        if (moneyBalance >= 0)
        {
            moneyCounterInt.color = positiveBalanceColor;
        }
        else
        {
            moneyCounterInt.color = negativeBalanceColor;
        }
        int decimals = Mathf.Abs((int)Math.Round(100.0 * (moneyBalance - Math.Truncate(moneyBalance))));
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
