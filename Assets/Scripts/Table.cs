﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public bool InUse { get;  set; }
    private Transform sittingZone;
    private Transform waiterZone;

    void Start()
    {
        sittingZone = transform.GetChild(0);
        waiterZone = transform.GetChild(1);
        InUse = false;
    }

    public void SetUpBossTable(Transform waitZone)
    {
        waiterZone = waitZone;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetSittingZone()
    {
        return sittingZone.position;
    }

    public Transform GetWaiterZone()
    {
        return waiterZone;
    }
}
