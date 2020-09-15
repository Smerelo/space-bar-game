using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RessourceZone : MonoBehaviour
{
    public int RessourceQuantity { get; set; }

    [Header("Quantity Thresholds")]
    [Space]
    [SerializeField] private int littleRessourceQuantity = 3;
    [SerializeField] private int mediumRessourceQuantity = 5;
    [SerializeField] private int startingRessources;

    private SpriteRenderer spriteRenderer;
    
    [Header("Sprites By Quantity")]
    [Space]
    [SerializeField] private Sprite empty;
    [SerializeField] private Sprite mediumRessources;
    [SerializeField] private Sprite lotsOfRessources;
    public TextMeshProUGUI textGUI;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        RessourceQuantity = startingRessources;
    }

    public bool RemoveRessources(int quantity)
    {
        if (RessourceQuantity - quantity >= 0)
        {
            RessourceQuantity -= quantity;
            CheckForSpriteUpdate();
            return (true);
        }
        else
        {
            return (false);
        }
    }

    public void AddRessources(int quantity)
    {
        RessourceQuantity += quantity;
        CheckForSpriteUpdate();
    }

    private void CheckForSpriteUpdate()
    {
        if (RessourceQuantity == 0)
        {
            spriteRenderer.sprite = empty;
        }
        if (RessourceQuantity > 0 && RessourceQuantity <= littleRessourceQuantity)
        {
            spriteRenderer.sprite = mediumRessources;
        }
        if (RessourceQuantity > littleRessourceQuantity && RessourceQuantity < mediumRessourceQuantity)
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
        textGUI.text = RessourceQuantity.ToString();
    }
}
