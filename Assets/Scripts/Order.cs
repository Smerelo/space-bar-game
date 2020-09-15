using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order 
{
    private int foodType;
    private Table table;
    public bool IsBeingPrepared { get; set;}
    public bool IsReady { get; set; }

    public Order(int foodType, Table table)
    {
        this.foodType = foodType;
        this.table = table;
    }
}
