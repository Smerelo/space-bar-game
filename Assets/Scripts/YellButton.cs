using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YellButton : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Player player;
    [SerializeField] private Sprite[] sprites;
    private List<Image> images; 
    private Button yellButton;
    private Image image;
    private int mode;

    void Start()
    {
        image = GetComponent<Image>();
        yellButton = GetComponent<Button>();
    }

    void Update()
    {
        if (player.CanYell)
        {
            yellButton.interactable = true;
        }
        else
        {
            yellButton.interactable = false;
        }
    }

    internal void ChangeSprite(int i)
    {
        image.sprite = sprites[i];
    }
}
