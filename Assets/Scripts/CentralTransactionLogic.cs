using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralTransactionLogic : MonoBehaviour
{
    public List<Order> orders;
    private Dictionary<string, ZoneManagment> zones;
    void Start()
    {
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

    void Update()
    {
        if (Input.GetKeyDown("u"))
        {
            if (zones.TryGetValue("Preparing", out ZoneManagment preparingZone))
            {
                preparingZone.HireEmployee();
            }
            else
            {
                print("Failed to hire employee");
            }
        }
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
