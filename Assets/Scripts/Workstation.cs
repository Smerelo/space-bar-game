using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workstation : MonoBehaviour
{
    public bool InUse { get; private set; }
    private Transform workerPlacement;
    void Start()
    {
        InUse = false;
        workerPlacement = transform.GetChild(0);
    }

    void Update()
    {
        
    }

    public Transform GetWorkerPlacement()
    {
        return workerPlacement;
    }
}
