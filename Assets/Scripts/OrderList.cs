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
    private List<Order> backLog;

    public bool Moving { get; private set; }

    void Start()
    {
        backLog = new List<Order>();
        orders = new List<OrderCard>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Moving && backLog.Count > 0)
        {
            CheckBacklog();
        }
    }

    private void CheckBacklog()
    {
        foreach (Order order in backLog)
        {
            AddOrder(order);
        }
    }

    internal void AddOrder(Order order)
    {
        if (Moving)
        {
            backLog.Add(order);
        }
        else
        {
            orders.Add(Instantiate(orderPrefab, orderSpawn.position,
                Quaternion.identity, transform).GetComponent<OrderCard>());
            orders[activeOrders].AssignOrder(order);
            LeanTween.moveLocalX(orders[activeOrders].gameObject,
                orderPos.localPosition.x + 70 * activeOrders, 1f);
            activeOrders++;
        }
    }

    public void RemoveOrder(Order order)
    {
        activeOrders--;
        foreach (OrderCard card in orders)
        {
            Order orderTemp = card.GetOrder();
            if (orderTemp == order)
            {
                MoveOrders(card);
                orders.Remove(card);
                GameObject.Destroy(card.gameObject);
                break;
            }
        }
    }

    private void MoveOrders(OrderCard card)
    {
        Moving = true;
        bool hasReachOrder = false;
        foreach (OrderCard tCard in orders)
        {
            if (tCard == card)
            {
                hasReachOrder = true;
            }
            if (hasReachOrder)
            {
                LeanTween.moveLocalX(tCard.gameObject,
                    tCard.transform.localPosition.x - 70, .1f);
            }
        }
        Moving = false;
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
