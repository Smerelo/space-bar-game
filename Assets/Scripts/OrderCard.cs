using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OrderCard : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private Order currentOrder;
    private Player player;
    private Animator animator;
    private string currentState;
    private Image foodImage;

    private const string ORDER_NORMAL = "Order_Normal";
    private const string ORDER_SLECTED = "Order_Selected";
    private const string ORDER_2 = "Order_2";

    private bool IsSelected { get; set; }

    void Start()
    {
        foodImage = transform.GetChild(1).GetComponent<Image>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!currentOrder.IsAssigned && !player.orderAssigned)
        {
            IsSelected = true;
            LeanTween.scale(gameObject, new Vector3(1.1f, 1.1f, 1), 0.1f);
            currentOrder.IsAssigned = true;
            player.AssignOrder(currentOrder);

        }
    }

 

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!currentOrder.IsAssigned && !player.orderAssigned)
        {
            IsSelected = true;
            LeanTween.scale(gameObject, new Vector3(1.1f, 1.1f, 1), 0.1f);
            currentOrder.IsAssigned = true;
            player.AssignOrder(currentOrder);
        }
       
    }

    internal Order GetOrder()
    {
        return currentOrder;
    }

    internal void NextStep()
    {
        IsSelected = false;
        LeanTween.scale(gameObject, new Vector3(1f, 1f, 1), 0.1f);
        currentOrder.IsAssigned = false;
        if (currentOrder.IsReady)
        {
            ChangeAnimationState(ORDER_SLECTED);

        }
        if (currentOrder.IsBeingTakenToClean)
        {
            ChangeAnimationState(ORDER_2);

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.scale(gameObject, new Vector3(1.1f, 1.1f, 1), 0.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsSelected)
        {
            LeanTween.scale(gameObject, new Vector3(1f, 1f, 1), 0.1f);

        }
    }

    public void AssignOrder(Order order)
    {
        currentOrder = order;
        if (foodImage == null)
        {
            foodImage = transform.GetChild(1).GetComponent<Image>();
        }
        foodImage.sprite = order.GetFoodSprite();
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }
        animator.Play(newState);
        currentState = newState;
    }


}
