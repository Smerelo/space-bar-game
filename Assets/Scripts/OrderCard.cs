using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OrderCard : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private Order currentOrder;
    private Player player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        currentOrder.IsAssigned = true;
        player.AssignOrder(currentOrder);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        currentOrder.IsAssigned = true;
        player.AssignOrder(currentOrder);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.scale(gameObject, new Vector3(1.1f, 1.1f, 1), 0.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.scale(gameObject, new Vector3(1f, 1f, 1), 0.1f);
    }

    public void AssignOrder(Order order)
    {
        currentOrder = order;
    }
   
}
