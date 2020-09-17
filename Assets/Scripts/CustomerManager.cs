using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomerManager : MonoBehaviour
{
    public List<CustomerBehaviour> customers;
    private List<Table> tables;
    private List<CustomerBehaviour> queue;
    private TableManager tableManager;
    private float difficultyMultiplier = 1;
    [SerializeField] private float minEatingTime = 5;
    [SerializeField] private float maxEatingTime = 10;
    [SerializeField] private float startMinClientDelay = 7;
    [SerializeField] private float startMaxClientDelay = 15;
    [SerializeField] private float endMinClientDelay = 7;
    [SerializeField] private float endMaxClientDelay = 15;
    [SerializeField] private float startMaxCustomers = 3;
    [SerializeField] private float endMaxCustomers = 10;
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private float dayLenght = 8;
    [SerializeField] private TextMeshProUGUI text;
    private float minClientDealy;
    private float maxClientDelay;
    private float maxDifficultyIncrease;
    private float minDifficultyIncrease;
    private float clientDifficultyIncrease;
    private float clientFrequency;
    private float maxCustomers;
    private float globalTimer;
    private float timer = 0f;
    DateTime time;
    private int hours = 8;
    private float minutes = 8 * 60;
    private CentralTransactionLogic spaceCantina;
    public Transform waitZone;


    // Start is called before the first frame update
    void Start()
    {
        maxDifficultyIncrease = (startMaxClientDelay - endMaxClientDelay) / dayLenght;
        minDifficultyIncrease = (startMinClientDelay - endMinClientDelay) / dayLenght;
        clientDifficultyIncrease = (startMaxCustomers - endMaxCustomers) / dayLenght;
        minClientDealy = startMinClientDelay;
        maxClientDelay = startMaxClientDelay;
        maxCustomers = startMaxCustomers;
        spaceCantina = GameObject.Find("SpaceCantina").GetComponent<CentralTransactionLogic>();
        queue = new List<CustomerBehaviour>();
        clientFrequency = UnityEngine.Random.Range(startMinClientDelay, startMaxClientDelay);
        customers = new List<CustomerBehaviour>();
        tables = new List<Table>();
        tableManager = GameObject.Find("TableManager").GetComponent<TableManager>();
    }

    // Update is called once per frame
    void Update()
    {
        globalTimer += Time.deltaTime;
        if (globalTimer % 60 == 0)
        {
            IncreaseDifficulty();
        }
        UpdateClock();
        SpawnerLogic();
    }

    private void UpdateClock()
    {
        string hour;
        string minute;
        minutes += Time.deltaTime;
        Debug.Log(minutes);
        hour = ZeroPadding(Mathf.FloorToInt(minutes / 60));
        minute = ZeroPadding(Mathf.FloorToInt(minutes) % 60);
        text.text = hour + ':' + minute;
    }

    private string ZeroPadding(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }

    private void IncreaseDifficulty()
    {
        minClientDealy -= minDifficultyIncrease;
        maxClientDelay -= maxDifficultyIncrease;
        maxCustomers += clientDifficultyIncrease;
    }

    internal void SendOrder(Order order)
    {
        spaceCantina.AddOrder(order);
    }

    private void SpawnerLogic()
    {
        timer += Time.deltaTime;
        if (queue.Count > 0 && tableManager.TryAvailableTable(out Table queueTable))
        {

            queue[0].AssignTable(queueTable);
            queue[0].IsWatingForTable = false;
            customers.Add(queue[0]);
            queue.RemoveAt(0);
            UpdateQueuePositions();
        }
        if (timer >= clientFrequency && queue.Count + customers.Count < Mathf.CeilToInt(maxCustomers))
        {
            clientFrequency = UnityEngine.Random.Range(minClientDealy, maxClientDelay);
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
                customer.IsWatingForTable = true;
                customer.numberInQueue = queue.Count;
                queue.Add(customer);
                customer.WaitForTable();
            }
        }
    }

    private void UpdateQueuePositions()
    {
        foreach (CustomerBehaviour customer in queue)
        {
            customer.MoveUp();
        }
    }

    internal float GetEatingTime()
    {
        return UnityEngine.Random.Range(minEatingTime, maxEatingTime);
    }
}
