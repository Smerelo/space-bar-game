using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBubble : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer imageSpriteRenderer;
    private float timer;
    private float countdown;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        imageSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        HideBubble();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > countdown)
        {
            HideBubble();
        }
    }

    private void HideBubble()
    {
        spriteRenderer.enabled = false;
        imageSpriteRenderer.enabled = false;
    }

    public void SetSprite(Order.FoodTypes type)
    {
        imageSpriteRenderer.sprite = Order.GetFoodTypeAsset(type).FoodSprite;
    }

    public void ShowBubble(float uptime)
    {
        timer = 0f;
        countdown = uptime;
        spriteRenderer.enabled = true;
        imageSpriteRenderer.enabled = true;
    }

}
