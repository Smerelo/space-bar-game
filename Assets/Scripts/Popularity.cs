using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popularity : MonoBehaviour
{
    [SerializeField] private GameOver gameOverScreen;
    [SerializeField] private Animator animator;

    private DayManagement day;
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        day = GameObject.Find("DayManager").GetComponent<DayManagement>();
        slider = GetComponent<Slider>();
    }

    internal float GetScore()
    {
        return slider.value;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdatePopularity(int popularity)
    {
        slider.value += popularity;
        if (slider.value == 10)
        {
            Debug.Log("here");

            animator.Play("5");
        }
        else if (slider.value <10 && slider.value > 7)
        {
            Debug.Log("here2");

            animator.Play("4");

        }
        else if (slider.value < 8 && slider.value > 5)
        {
            animator.Play("3");

        }
        else if (slider.value < 6 && slider.value > 3)
        {
            animator.Play("2");

        }  
        else if (slider.value < 4 && slider.value > 0)
        {
            animator.Play("1");
        }
        if (slider.value == 0)
        {
            animator.Play("0");
            GameOver();
        }
    }

    private void GameOver()
    {
        day.PauseGame();
        gameOverScreen.gameObject.SetActive(true);
    }
}
