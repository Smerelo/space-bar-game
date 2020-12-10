using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    List<Table> tables;
    void Start()
    {
        tables = new List<Table>();
        foreach(Transform child in transform)
        {
            if (child.TryGetComponent(out Table table))
            {
                tables.Add(table);
            }
        }
    }

    void Update()
    {
        
    }

    public void AddTable(Table table)
    {
        tables.Add(table);
    }

    public void FreeTables()
    {
        foreach (Table table in tables)
        {
            table.InUse = false;
        }
    }

    internal bool TryAvailableTable(out Table table)
    {
        table = DrawRandomAvailable<Table>(tables, (Table t) => !t.InUse);
        return (table != null);
    }
    private T DrawRandomAvailable<T>(List<T> list, Func<T, bool> check)
    {
        List<int> indexes = new List<int>(list.Count);
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = UnityEngine.Random.Range(0, list.Count);
            while (indexes.Contains(rnd))
            {
                rnd = UnityEngine.Random.Range(0, list.Count);
            }
            indexes.Add(rnd);
        }
        foreach (int index in indexes)
        {
            if (check(list[index]))
            {
                return list[index];
            }
        }
        return default(T);
    }
}
