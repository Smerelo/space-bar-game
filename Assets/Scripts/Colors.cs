using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colors : MonoBehaviour
{
    protected static Colors instance;
    public static Colors Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (Colors)FindObjectOfType(typeof(Colors));

                if (instance == null)
                {
                    Debug.LogError("An instance of " + typeof(Colors) +
                       " is needed in the scene, but there is none.");
                }
            }

            return instance;
        }
    }
    public Color positiveBalance;
    public Color negativeBalance;


    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
