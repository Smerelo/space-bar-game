using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class EmployeeCard : MonoBehaviour
{
    private Cv curriculum;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI taskSpeedText;
    [SerializeField] private TextMeshProUGUI mSpeedText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button hireButton;
    [SerializeField] private GameObject hireStamp;

    private Animator animator;
    private HeadEmployeeManager headEmployeeManager;
    private GameObject cards;
    private List<EmployeeCard> upgradeCards;
    private TabletMenu tablet;
    private GameObject employee;
    private bool changeAnimation;
    private string currentState;
    private CentralTransactionLogic ctl;

    // Start is called before the first frame update
   
    void Start()
    {
        ctl = GameObject.Find("SpaceCantina").GetComponent<CentralTransactionLogic>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        headEmployeeManager = GameObject.Find("HeadEmployees").GetComponent<HeadEmployeeManager>();
        tablet = GameObject.Find("Tablet").GetComponent<TabletMenu>();
        currentState = null;
    } 

    // Update is called once per frame
    void Update()
    {
        ChangeAnimation("Employee_" + curriculum.employeeType);
    }

    private void ChangeAnimation(string newState)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(newState))
        {
            animator.Play(newState);
            currentState = newState;
        }

    }

    public void HireHeadEmployee()
    {
        if (ctl.GetBalance() >= curriculum.price && headEmployeeManager.employeeList.Count < 2)
        {
            tablet.HireHeadEmployee(curriculum);
            hireStamp.SetActive(true);
            hireButton.interactable = false;
            ctl.CashIn(-curriculum.price);
        }
    }

    public void FireEmployee()
    {
        Destroy(employee.gameObject);
        tablet.ResetCard(this);
        headEmployeeManager.FireEmployee(employee.GetComponent<HeadEmployee>());
    }

    public void InstantiateEmployee(Cv cv, GameObject hiredEmployee)
    {
        nameText.text = cv.name;
        taskSpeedText.text = $"{cv.taskSpeed.ToString("F2")}s";
        mSpeedText.text = $"{cv.moveSpeed.ToString("F2")}";
        priceText.text = $"${cv.price.ToString("F0")}/Hour";
        curriculum = cv;
        changeAnimation = true;
        employee = hiredEmployee;
    }

    internal void ResetCard()
    {
        GenerateStats();
        hireStamp.SetActive(false);
        hireButton.interactable = true;
    }

    public void GenerateStats()
    {
        if (animator == null)
        {
            animator = transform.GetChild(0).GetComponent<Animator>();

        }
        string name = GetName();
        float price = GetPrice();
        float mSpeed = GetMoveSpeed();
        float taskSpeed = GetTaskSpeed();
        int employeeType = GetEmployeeType();
        curriculum = new Cv(name, price, taskSpeed, mSpeed, employeeType);
        nameText.text = name;
        taskSpeedText.text = $"{taskSpeed.ToString("F2")}";
        mSpeedText.text = $"{mSpeed.ToString("F2")}";
        priceText.text = $"${price.ToString("F0")}";
        curriculum.employeeType = employeeType;
        changeAnimation = true;
    }

    private int GetEmployeeType()
    {
        return UnityEngine.Random.Range(1, 4);
    }

    private float GetMoveSpeed()
    {
        return UnityEngine.Random.Range(2f, 4f);
    }

    private float GetTaskSpeed()
    {
        return UnityEngine.Random.Range(0.6f, 1.1f);
    }

    private float GetPrice()
    {
        return UnityEngine.Random.Range(100, 180);
    }

    private string GetName()
    {
        return Constants.Names[UnityEngine.Random.Range(0, Constants.Names.Count)];
    }
}
