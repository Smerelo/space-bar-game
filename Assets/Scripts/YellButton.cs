using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YellButton : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Player player;
    private Button yellButton;

    void Start()
    {
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
}
