﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderList : MonoBehaviour
{
    [SerializeField] private GameObject orderPrefab;
    [SerializeField] private Transform orderSpawn;
    [SerializeField] private Transform orderPos;
    private List<LTDescr> tweens;
    private List<OrderCard> orders;
    private int activeOrders = 0;
    private List<Order> backLog;
    private float gap = 100f;

    public bool Moving { get; private set; }

    void Start()
    {
        tweens = new List<LTDescr>();
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
        CheckOrdersPos();
    }


    private void CheckOrdersPos()
    {
        int i = 0;
        foreach (OrderCard order in orders)
        {
            if (order.orderNb != i)
            {
                order.orderNb = i;
            }
            if (!order.Moving && order.gameObject.transform.localPosition.x != orderPos.localPosition.x + gap * i)
            {
                object obj = order;
                order.Moving = true;
                order.tweenId = LeanTween.moveLocalX(order.gameObject,
                orderPos.localPosition.x + gap * i, 1f).setOnComplete(StopMoving).setOnCompleteParam(obj).id;
            }
            i++;
        }
    }

    private void StopMoving(object obj)
    {
        OrderCard order = (OrderCard)obj;
        order.Moving = false;
        LeanTween.cancel(order.tweenId);
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
            orders[activeOrders].orderNb = activeOrders;
            orders[activeOrders].AssignOrder(order);
            orders[activeOrders].PlaySound();
            object obj = orders[activeOrders];
            orders[activeOrders].tweenId = LeanTween.moveLocalX(orders[activeOrders].gameObject,
            orderPos.localPosition.x + gap * activeOrders, 1f).setOnComplete(StopMoving).setOnCompleteParam(obj).id;
            orders[activeOrders].Moving = true;
            activeOrders++;
        }
    }

    private void PlaySound()
    {
        orders[activeOrders - 1].Moving = false;

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
                    tCard.transform.localPosition.x - gap, .1f);
                tCard.orderNb--;
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

    internal  void ClearOrders()
    {
        foreach (OrderCard card in orders)
        {
            GameObject.Destroy(card.gameObject);
        }
        activeOrders = 0;
        orders.Clear(); 
    }
}
