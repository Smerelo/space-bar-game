using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEditor.Animations;

public class Boss : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private Slider healthBar;
    [SerializeField] private int bossNb;
    [SerializeField] private Slider progressBar;
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private GameObject tablePrefab;
    [HideInInspector] public bool attacking;
    [HideInInspector] public bool isInCinematic;



    private Order.FoodTypes foodPreference;
    private CentralTransactionLogic ctl;
    private float AttackTimer;
    private float startAttackTimer;
    private SlimeBoss slimeBoss;
    private bool bossIsActive;
    private Transform waitZone;
    private Table table;
    private TableManager tableManager;
    private Order order;
    private float orderTimer;
    private List<CustomerBehaviour> orderList;
    private bool eating;

    void Start()
    {
        // BossCinematic();
        orderList = new List<CustomerBehaviour>();
        ctl = GameObject.Find("SpaceCantina").GetComponent<CentralTransactionLogic>();
        waitZone = transform.GetChild(2).transform;
        table = Instantiate(tablePrefab, new Vector3(100f, 100f, 100f),
            Quaternion.identity, transform).GetComponent<Table>();
        foodPreference = Order.RandomFoodType();
        startAttackTimer = 10f;
        progressBar.maxValue = startAttackTimer;
        AttackTimer = startAttackTimer;
        orderTimer = 0f;
    }

  

    // Update is called once per frame
    void Update()
    {
        if (!attacking  && !isInCinematic && bossIsActive)
        {
            progressBar.gameObject.SetActive(true);
            AttackTimer -= Time.deltaTime;

            orderTimer -= Time.deltaTime;
            progressBar.value = AttackTimer;
            if (orderTimer <= 0)
            {
                orderTimer = 5f;
                SendOrder();
            }
            if (AttackTimer <= 0)
            {
                AttackTimer = startAttackTimer;
                slimeBoss.GetAngry();
                attacking = true;
            }
        }

        if (orderList.Count > 0)
        {
            foreach (CustomerBehaviour customer in orderList)
            {
                if (customer.isEating)
                {
                    Eatfood(customer);
                    break;
                }
            }
        }
    }

    private void Eatfood(CustomerBehaviour customer)
    {
        orderList.Remove(customer);
        Destroy(customer.gameObject);
        AttackTimer = startAttackTimer;
        UpdateHealth(-5);
        slimeBoss.Eat();
    }

    private void UpdateHealth(int i)
    {
        healthBar.value += i;
    }

   
    private void SendOrder()
    {
        orderList.Add(Instantiate(customerPrefab, new Vector3(1000f,1000f, 1000f ), 
            Quaternion.identity, transform).GetComponent<CustomerBehaviour>());
        order = new Order(foodPreference, table, orderList[orderList.Count - 1]);
        ctl.AddOrder(order);
    }

    internal void BossCinematic()
    {
        isInCinematic = true;
        bossIsActive = true;
        Debug.Log(table);
        table.SetUpBossTable(waitZone);
        tableManager = GameObject.Find("TableManager").GetComponent<TableManager>();
        tableManager.AddTable(table);
        progressBar.gameObject.SetActive(false);
        switch (bossNb)
        {
            case 0:
                slimeBoss = GetComponent<SlimeBoss>();
                slimeBoss.PlayCinematic();
                break;
            default:
                break;
        }
    }
}
