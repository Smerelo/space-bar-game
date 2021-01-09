using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
public class OrderCard : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private Order currentOrder;
    private Player player;
    private Animator animator;
    private string currentState;
    private Image foodImage;
    private Animator animator2;
    private GameObject ownerImage;
    private AudioSource audio;

    private const string ORDER_NORMAL = "Order_Normal";
    private const string ORDER_SLECTED = "Order_Selected";
    private const string ORDER_2 = "Order_2";

    private const string W_ = "w_";
    private const string P = "p";



    private bool IsSelected { get; set; }

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        ownerImage = transform.GetChild(2).gameObject;
        foodImage = transform.GetChild(1).GetComponent<Image>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        animator2 = transform.GetChild(2).GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    void Start()
    {
        
    }

    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!currentOrder.IsAssigned && !player.orderAssigned)
        {
            ownerImage.SetActive(true);
            animator2.Play("p");
            Debug.Log(currentOrder.IsAssigned);
            IsSelected = true;
            LeanTween.scale(gameObject, new Vector3(1.1f, 1.1f, 1), 0.1f);
            currentOrder.IsAssigned = true;
            player.AssignOrder(currentOrder);
        }
    }

    internal void PlaySound()
    {
        audio.Play();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!currentOrder.IsAssigned && !player.orderAssigned)
        {
            ownerImage.SetActive(true);
            animator2.Play("p");
            Debug.Log(currentOrder.IsAssigned);
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
        ownerImage.SetActive(false);
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
        order.card = this;
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

    internal void ChangeOwner(int employeeType)
    {
        if (ownerImage == null)
        {
            ownerImage = transform.GetChild(2).gameObject;

        }
        ownerImage.SetActive(true);
        animator2.Play(W_ + (employeeType));
    }
}
