using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public bool InUse { get;  set; }
    private Transform sittingZone;

    void Start()
    {
        sittingZone = transform.GetChild(0);
        InUse = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal Vector3 GetSittingZone()
    {
        return sittingZone.position;
    }
}
