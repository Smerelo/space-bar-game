using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralTransactionLogic : MonoBehaviour
{
    private Dictionary<string, Transform> zones;
    void Start()
    {
        zones = new Dictionary<string, Transform>();
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out ZoneManagment zoneManagment))
            {
                if (zoneManagment.GetName() == "")
                    Debug.Log("Put a name on your zones pls");
                zones.Add(zoneManagment.GetName(), child);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("u"))
        {
            if (zones.TryGetValue("Preparing", out Transform preparingZone))
            {
                preparingZone.GetComponent<ZoneManagment>().HireEmployee();
            }
            else
            {
                print("Failed to hire employee");
            }
        }
    }
}
