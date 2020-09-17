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

    void Start()
    {
        moneyBalance = startingBalance;
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
        if (moneyBalance <= 0)
        {
            
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
        if (Input.GetKeyDown("1"))
        {
            if (zones.TryGetValue(Constants.cleaning, out ZoneManagment cleaningZone))
            {
                cleaningZone.HireEmployee();
            }
            else
            {
                Debug.Log("Failed to hire employee");
            }
        }
        if (Input.GetKeyDown("2"))
        {
            if (zones.TryGetValue(Constants.preparing, out ZoneManagment preparingZone))
            {
                preparingZone.HireEmployee();
            }
            else
            {
                Debug.Log("Failed to hire employee");
            }
        }
        if (Input.GetKeyDown("3"))
        {
            if (zones.TryGetValue(Constants.serving, out ZoneManagment servingZone))
            {
                servingZone.HireEmployee();
            }
            else
            {
                Debug.Log("Failed to hire employee");
            }
        }
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
