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
    [SerializeField] private float startMinEatingTime = 5;
    [SerializeField] private float startMaxEatingTime = 10;
    [SerializeField] private float endMinEatingTime = 5;
    [SerializeField] private float endMaxEatingTime = 10;
    [SerializeField] private float startMinClientDelay = 7;
    [SerializeField] private float startMaxClientDelay = 15;
    [SerializeField] private float endMinClientDelay = 7;
    [SerializeField] private float endMaxClientDelay = 15;
    [SerializeField] private float startMaxCustomers = 3;
    [SerializeField] private float endMaxCustomers = 10;
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private TextMeshProUGUI text;
    private float minClientDealy;
    public float dayLenght = 8;
    private float maxClientDelay;
    private float maxDifficultyIncrease;
    private float minDifficultyIncrease;
    private float clientDifficultyIncrease;
    private float maxEatingSpeedIncrease;
    private float minEatingSpeedIncrease;
    private float minEatingTime;
    private float maxEatingTime;
    private float clientFrequency;
    private float maxCustomers;
    private float globalTimer;
    private float timer = 0f;
    public float minutes = 8 * 60;
    private CentralTransactionLogic spaceCantina;
    public Transform waitZone;
    private bool difficultyIncreased = false;
    private bool timeStop;
    internal void StopTimer()
    {
        timeStop = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        maxDifficultyIncrease = (startMaxClientDelay - endMaxClientDelay) / dayLenght;
        minDifficultyIncrease = (startMinClientDelay - endMinClientDelay) / dayLenght;
        clientDifficultyIncrease = (endMaxCustomers - startMaxCustomers) / dayLenght;
        maxEatingSpeedIncrease = (startMaxEatingTime - endMaxEatingTime) / dayLenght;
        minEatingSpeedIncrease = (startMinEatingTime - endMinEatingTime) / dayLenght;
        minClientDealy = startMinClientDelay;
        maxClientDelay = startMaxClientDelay;
        maxCustomers = startMaxCustomers;
        minEatingTime = startMinEatingTime;
        maxEatingTime = startMaxEatingTime;
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
        if (!timeStop)
        {
            globalTimer += 3 * Time.deltaTime;
            if (Mathf.FloorToInt(globalTimer) % 60 == 0 && !difficultyIncreased && globalTimer >= 1)
            {
                IncreaseDifficulty();
                difficultyIncreased = true;
            }
            if (Mathf.FloorToInt(globalTimer) % 60 != 0)
            {
                difficultyIncreased = false;
            }
            UpdateClock();
            SpawnerLogic();
        }
    }

    private void UpdateClock()
    {
        string hour;
        string minute;
        minutes += 3 * Time.deltaTime;
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
        minEatingTime -= minEatingSpeedIncrease;
        maxEatingTime -= maxEatingSpeedIncrease;
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
