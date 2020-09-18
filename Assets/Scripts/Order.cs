using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    public int FoodType { get; private set; }
    public Table Table { get; private set; }
    public CustomerBehaviour Customer {get; private set;}
    public bool IsBeingPrepared { get; set;}
    public bool IsBeingTakenToClean { get; set; }
    public bool IsReady { get; set; }
    private float minPrice = 15f;
    private float maxPrice = 25f;
    public Order(int foodType, Table table, CustomerBehaviour customer)
    {
        FoodType = foodType;
        Table = table;
        Customer = customer;
    }
    public Order(Table table, CustomerBehaviour customer)
    {
        FoodType = 0;
        Table = table;
        Customer = customer;
    }

    internal float GenerateMealPrice()
    {
        float mealPrice = UnityEngine.Random.Range(minPrice, maxPrice);
        float waitTime = Customer.GetWaitTime();
        float moneyPayed = mealPrice + waitTime * 1.5f;
        Customer.Pay(moneyPayed);
        Debug.Log(moneyPayed);
        return moneyPayed;
    }

    internal void ResetBools()
    {
        IsBeingPrepared = false;
        IsBeingTakenToClean = false;
        IsReady = false;
    }
}
