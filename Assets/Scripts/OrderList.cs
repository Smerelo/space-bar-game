using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderList : MonoBehaviour
{
    [SerializeField] private GameObject orderPrefab;
    [SerializeField] private Transform orderSpawn;
    [SerializeField] private Transform orderPos;

    private List<GameObject> orders;
    private int activeOrders = 0;

    void Start()
    {
        orders = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void AddOrder(Order order)
    {
        orders.Add(Instantiate(orderPrefab, orderSpawn.position, 
            Quaternion.identity, transform));
        orders[activeOrders].GetComponent<OrderCard>().AssignOrder(order);
        LeanTween.moveLocalX(orders[activeOrders], 
            orderPos.localPosition.x + 70 * activeOrders, 1f);
        activeOrders++;
    }
}
