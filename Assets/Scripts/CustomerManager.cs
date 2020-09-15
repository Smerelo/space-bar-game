using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    private List<CustomerBehaviour> customers;
    private List<Table> tables;
    private List<CustomerBehaviour> queue;
    private TableManager tableManager;
    [SerializeField] private float minEatingTime;
    [SerializeField] private float maxEatingTime;
    [SerializeField] private float minClientDelay;
    [SerializeField] private float maxClientDelay;
    [SerializeField] private GameObject customerPrefab;
    private float clientFrequency;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        queue = new List<CustomerBehaviour>();
        clientFrequency = Random.Range(minClientDelay, maxClientDelay);
        customers = new List<CustomerBehaviour>();
        tables = new List<Table>();
        tableManager = GameObject.Find("TableManager").GetComponent<TableManager>();
    }

    // Update is called once per frame
    void Update()
    {
        SpawnerLogic();   
    }

    private void SpawnerLogic()
    {
        timer += Time.deltaTime;
        if (timer >= clientFrequency)
        {
            clientFrequency = Random.Range(minClientDelay, maxClientDelay);
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
        return Random.Range(minEatingTime, maxEatingTime);
    }
}
