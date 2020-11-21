using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class EmployeeCard : MonoBehaviour
{
    private Cv curriculum;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI taskSpeedText;
    [SerializeField] private TextMeshProUGUI mSpeedText;
    [SerializeField] private TextMeshProUGUI priceText;
    private HeadEmployeeManager headEmployeeManager;
    private GameObject cards;
    private List<EmployeeCard> upgradeCards;
    private TabletMenu tablet;

    // Start is called before the first frame update
   
    void Start()
    {
        headEmployeeManager = GameObject.Find("HeadEmployees").GetComponent<HeadEmployeeManager>();
        tablet = GameObject.Find("Tablet").GetComponent<TabletMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void HireHeadEmployee()
    {
        tablet.HireHeadEmployee(curriculum); 
    }

    public void InstantiateEmployee(Cv cv)
    {
        nameText.text = cv.name;
        taskSpeedText.text = $"{cv.taskSpeed.ToString("F2")}s";
        mSpeedText.text = $"{cv.moveSpeed.ToString("F2")}";
        priceText.text = $"${cv.price.ToString("F0")}/Hour";
        curriculum = cv;
    }

    public void GenerateStats()
    {
        string name = GetName();
        float price = GetPrice();
        float mSpeed = GetMoveSpeed();
        float taskSpeed = GetTaskSpeed();
        curriculum = new Cv(name, price, taskSpeed, mSpeed);
        nameText.text = name;
        taskSpeedText.text = $"{taskSpeed.ToString("F2")}s";
        mSpeedText.text = $"{mSpeed.ToString("F2")}";
        priceText.text = $"${price.ToString("F0")}/Hour";
    }
    
    private float GetMoveSpeed()
    {
        return UnityEngine.Random.Range(4f, 15f);
    }

    private float GetTaskSpeed()
    {
        return UnityEngine.Random.Range(0.3f, 1f);
    }

    private float GetPrice()
    {
        return UnityEngine.Random.Range(10f, 15f);
    }

    private string GetName()
    {
        return Constants.Names[UnityEngine.Random.Range(0, Constants.Names.Count)];
    }
}
