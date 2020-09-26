using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RessourceZone : MonoBehaviour
{
    public int RessourceQuantity { get; private set; }
    public int AvailableRessources { get; private set; }

    public Transform Input { get; private set; }
    public Transform Output { get; private set; }

    [Header("Quantity Thresholds")]
    [Space]
    [SerializeField] private int littleRessourceQuantity = 3;
    [SerializeField] private int mediumRessourceQuantity = 5;
    [SerializeField] private int startingRessources;

    private SpriteRenderer spriteRenderer;
    
    [Header("Sprites By Quantity")]
    [Space]
    [SerializeField] private Sprite empty;
    [SerializeField] private Sprite smallRessources;
    [SerializeField] private Sprite mediumRessources;
    [SerializeField] private Sprite lotsOfRessources;
    void Start()
    {
        Input = transform.GetChild(0);
        Output = transform.GetChild(1);
        spriteRenderer = GetComponent<SpriteRenderer>();
        RessourceQuantity = startingRessources;
        AvailableRessources = startingRessources;
    }
    public void SetAsideRessources(int quantity)
    {
        AvailableRessources -= quantity;
    }
    public bool RemoveRessources(int quantity)
    {
        if (RessourceQuantity - quantity >= 0)
        {
            RessourceQuantity -= quantity;
            return (true);
        }
        else
        {
            return (false);
        }
    }

    public void AddRessources(int quantity)
    {
        AvailableRessources += quantity;
        RessourceQuantity += quantity;
    }

    private void CheckForSpriteUpdate()
    {
        if (RessourceQuantity == 0)
        {
            spriteRenderer.sprite = empty;
        }
        if (RessourceQuantity > 0 && RessourceQuantity <= littleRessourceQuantity)
        {
            spriteRenderer.sprite = smallRessources;
        }
        if (RessourceQuantity > littleRessourceQuantity && RessourceQuantity <= mediumRessourceQuantity)
        {
            spriteRenderer.sprite = mediumRessources;
        }
        if (RessourceQuantity > mediumRessourceQuantity)
        {
            spriteRenderer.sprite = lotsOfRessources;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckForSpriteUpdate();
    }
}
