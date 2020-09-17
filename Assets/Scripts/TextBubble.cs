using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBubble : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer imageSpriteRenderer;
    [SerializeField] private List<Sprite> spriteList;
    private float timer;
    private float countdown;

    public int SpritesCount { get; internal set; }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        imageSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        SpritesCount = spriteList.Count;

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

    public void SetSprite(int i)
    {
        imageSpriteRenderer.sprite = spriteList[i];
    }

    public void ShowBubble(float uptime)
    {
        timer = 0f;
        countdown = uptime;
        spriteRenderer.enabled = true;
        imageSpriteRenderer.enabled = true;
    }

}
