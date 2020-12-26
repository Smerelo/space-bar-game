using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    public enum FoodTypes
    {
        GreenCup,
        PinkCup,
        YellowCup,
        GreenBeer,
        PinkBeer,
        YellowBeer,
        WineBottle,
        CoffeMug
    }
    public FoodTypes FoodType { get; private set; }
    public Table Table { get; private set; }
    public CustomerBehaviour Customer { get; private set; }
    public bool IsBeingPrepared { get; set; }
    public bool IsBeingTakenToClean { get; set; }
    public bool IsReady { get; set; }
    public bool IsAssigned { get; set;}
    public string Zone { get; set;}

    public float PreparationTime { get; set; }

    public Transform input;
    public Transform output;
    public Order(FoodTypes foodType, Table table, CustomerBehaviour customer)
    {
        FoodType = foodType;
        Table = table;
        Customer = customer;
        PreparationTime = Order.GetFoodTypeAsset(FoodType).PreparationTime;
    }
    public Order(Table table, CustomerBehaviour customer)
    {
        FoodType = RandomFoodType();
        Table = table;
        Customer = customer;
    }
    public Order(FoodTypes foodTypes, Table table)
    {
        FoodType = foodTypes;
        Table = table;                                              
    }

    internal float GenerateMealPrice()
    {
        GameAssets.FoodTypeAsset food = GetFoodTypeAsset(FoodType);
        
        float waitTimeBonus = Customer.GetWaitTimeBonus();
        float moneyPayed = food.Price + (food.Price * waitTimeBonus * 1.5f); //linear function maybe change to sigmoid
        Customer.Pay(moneyPayed);
        return moneyPayed;
    }

    public Sprite GetFoodSprite()
    {
        GameAssets.FoodTypeAsset food = GetFoodTypeAsset(FoodType);
        return food.foodSprite;
    }

    internal void ResetBools()
    {
        IsBeingPrepared = false;
        IsBeingTakenToClean = false;
        IsReady = false;
    }

    public static FoodTypes RandomFoodType()         
    {
        Array values = Enum.GetValues(typeof(FoodTypes));
        int i = UnityEngine.Random.Range(0, values.Length);
        return (FoodTypes)values.GetValue(i);
    }

    public static GameAssets.FoodTypeAsset GetFoodTypeAsset(FoodTypes type)
    {
        foreach (GameAssets.FoodTypeAsset food in GameAssets.I.FoodPlateTypes)
        {
            if (food.Type == type)
            {
                return food;
            }
        }
        Debug.LogError($"There is no entry for {type} in GameAssets");
        return null;
    }

    internal Workstation GetTable()
    {
        return Table.GetComponent<Workstation>();
    }
}
