using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TabletMenu : MonoBehaviour
{
    [SerializeField] private Button left;
    [SerializeField] private Button right;
    [SerializeField] private Transform curriculums;
    [SerializeField] private Transform employeeCards;
    [SerializeField] private EmployeeCard[] hiringCards;
    [SerializeField] private List<EmployeeCard> upgradeCards;

    private TabGroup tabGroup;
    private int menuPos1 = 2;
    private int menuPos2 = 0;
    public bool moving = false;
    private HeadEmployeeManager headEmployeeManager;
    public GameObject headEmployee;

    void Start()
    {
        tabGroup = transform.GetComponentInChildren( typeof(TabGroup),true) as TabGroup;
        UpdateButtons();
        headEmployeeManager = GameObject.Find("HeadEmployees").
                              GetComponent<HeadEmployeeManager>();
    }

    private void Update()
    {
        UpdateButtons();
    }
    public void GenerateNewCards()
    {
        foreach (EmployeeCard card in hiringCards)
        {
            card.ResetCard();
        }
    }

    public void HireHeadEmployee(Cv cv)
    {
        if (headEmployeeManager.employeeList.Count == 3)
        {
            ShowMessage();
        }
        else
        {
            HeadEmployee newEmployee =  Instantiate(headEmployee, headEmployeeManager.spawn.position, 
                                        Quaternion.identity).GetComponent<HeadEmployee>();
            newEmployee.InstantiateEmployee(cv);
            headEmployeeManager.HireEmployee(newEmployee);
            upgradeCards[headEmployeeManager.employeeList.Count -1].gameObject.SetActive(true);
            upgradeCards[headEmployeeManager.employeeList.Count - 1].InstantiateEmployee(cv, newEmployee.gameObject);
        }

    }

    internal void ResetCard(EmployeeCard employeeCard)
    {
        int index = -1;
        Transform cardPos = employeeCard.gameObject.transform;
        for (int i = 0; i < upgradeCards.Count; i++)
        {
            if (upgradeCards[i] == employeeCard)
            {
                index = i;
                break;
            }
        }
        Debug.Log(index);
        LeanTween.moveLocalX(employeeCard.gameObject, upgradeCards[2].gameObject.transform.localPosition.x, 0.3f);
        MoveAfterIndex(index, cardPos);
        employeeCard.gameObject.gameObject.SetActive(false);
        EmployeeCard temp = upgradeCards[index];
        upgradeCards.RemoveAt(index);
        upgradeCards.Add(temp);
    }

    private void MoveAfterIndex(int index, Transform pos)
    {
        for (int i = 0; i < upgradeCards.Count; i++)
        {
            if (i > index)
            {
                LeanTween.moveLocalX(upgradeCards[i].gameObject, 
                                    upgradeCards[i].gameObject.transform.localPosition.x - 6.8f, 0.4f);

            }
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
            menuPos2 += 1;
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

            menuPos2 -= 1;
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
                right.interactable = true;
            }

            else if (menuPos1 == 0)
            {
                right.interactable = false;
                left.interactable = true;
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
                if (headEmployeeManager.employeeList.Count == 2)
                {
                    if (menuPos2 == 0 )
                    {
                        left.interactable = false;
                        right.interactable = true;

                    }
                    if (menuPos2 == 1)
                    {
                        left.interactable = true;
                        right.interactable = false;
                    }
                }
                else if (headEmployeeManager.employeeList.Count == 3)
                {
                    if (menuPos2 == 0)
                    {
                        left.interactable = false;
                        right.interactable = true;

                    }

                    if (menuPos2 == 1)
                    {
                        left.interactable = true;
                        right.interactable = true;
                    }
                    if (menuPos2 == 2)
                    {
                        left.interactable = true;
                        right.interactable = false;
                    }
                }

            }
        }
    }


}
