using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerFood : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float cooldown;
    private float timer;
    [SerializeField] private Sprite dirtyPlate;
    private Sprite food;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = 0;
        HideFood();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > cooldown)
        {
            DirtyPlate();
        }
    }

    public void HideFood()
    {
        spriteRenderer.enabled = false;
    }

    public void ShowFood(float eatingTime)
    {
        cooldown = eatingTime;
        timer = 0f;
        spriteRenderer.sprite = food;
        spriteRenderer.enabled = true;
    }
    private void DirtyPlate()
    {
        spriteRenderer.sprite = dirtyPlate;
    }
    public void SetFood(Order.FoodTypes type)
    {
        food = Order.GetFoodTypeAsset(type).FoodSprite;
    }
}
