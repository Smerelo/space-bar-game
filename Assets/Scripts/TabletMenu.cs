using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TabletMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI[] stats;
    [SerializeField] private Button left;
    [SerializeField] private Button right;
    [SerializeField] private Transform curriculums;
    [SerializeField] private Transform employeeCards;
    [SerializeField] private float speed = 15;
    [SerializeField] private GameObject[] hiringCards;
    [SerializeField] private EmployeeCard[] upgradeCards;

    private TabGroup tabGroup;
    private int menuPos1 = 2;
    private int menuPos2 = 2;
    public bool moving = false;
    private HeadEmployeeManager headEmployeeManager;


    void Start()
    {
        tabGroup = transform.GetComponentInChildren( typeof(TabGroup),true) as TabGroup;
        UpdateButtons();
        headEmployeeManager = GameObject.Find("HeadEmployees").GetComponent<HeadEmployeeManager>();
    }

    public void HireHeadEmployee(Cv cv)
    {
        if (headEmployeeManager.employeeList.Count == 3)
        {
            ShowMessage();
        }
        else
        {
            Debug.Log($"enters{headEmployeeManager.employeeList.Count}");
            upgradeCards[headEmployeeManager.employeeList.Count].gameObject.SetActive(true);
            upgradeCards[headEmployeeManager.employeeList.Count].InstantiateEmployee(cv);
        }

    }

    private void ShowMessage()
    {
        throw new NotImplementedException();
    }

    public void MoveRight()
    {
        if (tabGroup.tabIndex == 0)
        {
            menuPos1 -= 1;
            Vector3 newPos = new Vector3(curriculums.localPosition.x - 6.8f, curriculums.localPosition.y, curriculums.localPosition.z);
            LeanTween.moveLocalX(curriculums.gameObject, newPos.x, 0.5f);
        }
        else
        {
            menuPos2 -= 1;
            Vector3 newPos = new Vector3(employeeCards.localPosition.x - 6.8f, employeeCards.localPosition.y, employeeCards.localPosition.z);
            LeanTween.moveLocalX(employeeCards.gameObject, newPos.x, 0.5f);
        }

    }

    public void MoveLeft()
    {
        if (tabGroup.tabIndex == 0)
        {
            menuPos1 += 1;
            Vector3 newPos = new Vector3(curriculums.localPosition.x + 6.8f, curriculums.localPosition.y, curriculums.localPosition.z);
            LeanTween.moveLocalX(curriculums.gameObject, newPos.x, 0.5f);
        }
        else
        {

            menuPos2 += 1;
            Vector3 newPos = new Vector3(employeeCards.localPosition.x + 6.8f, employeeCards.localPosition.y, employeeCards.localPosition.z);
            LeanTween.moveLocalX(employeeCards.gameObject, newPos.x, 0.5f);
        }

    }

    public void UpdateButtons()
    {
        if (tabGroup.tabIndex == 0)
        {

            if (menuPos1 == 2)
            {
                left.interactable = false;
            }

            else if (menuPos1 == 0)
            {
                right.interactable = false;
            }
            else
            {
                left.interactable = true;
                right.interactable = true;
            }
        }
        else
        {
            if (headEmployeeManager.employeeList.Count == 0 || headEmployeeManager.employeeList.Count == 1)
            {
                left.interactable = false;
                right.interactable = false;
            }
            else
            {

            }
        }
    }


}
