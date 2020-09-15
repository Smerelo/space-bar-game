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

    internal bool TryAvailableTable(out Table table)
    {

        foreach (Table t in tables)
        {
            if (!t.InUse)
            {
                table = t;
                return true;
            }
        }
        table = null;
        return false;
    }
}
