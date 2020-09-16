using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    private int foodType;
    public Table Table { get; private set; }
    public CustomerBehaviour Customer {get; private set;}
    public bool IsBeingPrepared { get; set;}
    public bool IsReady { get; set; }

    public Order(int foodType, Table table, CustomerBehaviour customer)
    {
        this.foodType = foodType;
        Table = table;
        Customer = customer;
    }
}
