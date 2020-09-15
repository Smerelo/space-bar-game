using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    private List<CustomerBehaviour> customers;
    private List<Table> tables;
    private List<CustomerBehaviour> queue;
    private TableManager tableManager;
    [SerializeField] private float minEatingTime = 5;
    [SerializeField] private float maxEatingTime = 10;
    [SerializeField] private float minClientDelay = 7;
    [SerializeField] private float maxClientDelay = 15;
    [SerializeField] private GameObject customerPrefab;
    private float clientFrequency;
    private float timer = 0f;
    private CentralTransactionLogic spaceCantina;

    // Start is called before the first frame update
    void Start()
    {
        spaceCantina = GameObject.Find("SpaceCantina").GetComponent<CentralTransactionLogic>();
        queue = new List<CustomerBehaviour>();
        clientFrequency = UnityEngine.Random.Range(minClientDelay, maxClientDelay);
        customers = new List<CustomerBehaviour>();
        tables = new List<Table>();
        tableManager = GameObject.Find("TableManager").GetComponent<TableManager>();
    }

    // Update is called once per frame
    void Update()
    {
        SpawnerLogic();   
    }

    internal void SendOrder(Order order)
    {
        spaceCantina.AddOrder(order);
    }

    private void SpawnerLogic()
    {

        timer += Time.deltaTime;
        if (timer >= clientFrequency)
        {
            clientFrequency = UnityEngine.Random.Range(minClientDelay, maxClientDelay);
            timer = 0f;
            if (tableManager.TryAvailableTable(out Table table))
            {
                CustomerBehaviour customer  = Instantiate(customerPrefab, transform.position , Quaternion.identity, transform).GetComponent<CustomerBehaviour>();
                customer.AssignTable(table);
                customers.Add(customer);
            }
            else
            {
                CustomerBehaviour customer = Instantiate(customerPrefab, transform.position, Quaternion.identity, transform).GetComponent<CustomerBehaviour>();
                customer.WaitForTable();
                customer.IsWatingForTable = true;
                queue.Add(customer);
             }
        }
    }

    internal float GetEatingTime()
    {
        return UnityEngine.Random.Range(minEatingTime, maxEatingTime);
    }
}
