using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableBackground : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
{
     private HeadEmployeeCardMenu employee;


    private void Start()
    {

        employee = transform.root.gameObject.GetComponent<HeadEmployeeCardMenu>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        employee.HideCardMenu();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        employee.HideCardMenu();
    }
}
