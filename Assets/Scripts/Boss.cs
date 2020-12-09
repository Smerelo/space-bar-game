using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEditor.Animations;

public class Boss : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private Slider healthBar;
    [SerializeField] private int bossNb;

    private float AttackTimer;
    private SlimeBoss slimeBoss;


    void Start()
    {
        BossCinematic();
        AttackTimer = 30f;
    }

  

    // Update is called once per frame
    void Update()
    {
       /* AttackTimer -= Time.deltaTime;
        if (AttackTimer <= 0)
        {
            slimeBoss.Attack();
            AttackTimer = 30f;
        }*/
    }

    

    private void UpadateHealth(int i)
    {

    }

   

    internal void BossCinematic()
    {
        switch (bossNb)
        {
            case 0:
                slimeBoss = GetComponent<SlimeBoss>();
                slimeBoss.PlayCinematic();
                break;
            default:
                break;
        }
    }
}
