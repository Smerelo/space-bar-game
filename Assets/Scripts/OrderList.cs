using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderList : MonoBehaviour
{
    [SerializeField] private GameObject orderPrefab;
    [SerializeField] private Transform orderSpawn;
    [SerializeField] private Transform orderPos;

    private List<OrderCard> orders;
    private int activeOrders = 0;

    void Start()
    {
        orders = new List<OrderCard>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void AddOrder(Order order)
    {
        orders.Add(Instantiate(orderPrefab, orderSpawn.position, 
            Quaternion.identity, transform).GetComponent<OrderCard>());
        orders[activeOrders].AssignOrder(order);
        LeanTween.moveLocalX(orders[activeOrders].gameObject, 
            orderPos.localPosition.x + 70 * activeOrders, 1f);
        activeOrders++;
    }

    public void RemoveOrder(Order order)
    {
        foreach (OrderCard card in orders)
        {
            Order orderTemp = card.GetOrder();
            if (orderTemp == order)
            {
                orders.Remove(card);
                GameObject.Destroy(card.gameObject);
                break;
            }
        }
        activeOrders--;
    }

    public void SendOrderToNextStep(Order order)
    {
        foreach (OrderCard card in orders)
        {
            Order orderTemp = card.GetOrder();
            if (orderTemp == order)
            {
                card.GetComponent<OrderCard>().NextStep();
            }
        }
    }
}
