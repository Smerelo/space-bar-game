using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public bool Paused { get; private set; }

    [SerializeField] GameObject pauseScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            PauseGame();
        }
    }


    public void PauseGame()
    {
        if (!Paused)
        {
            Time.timeScale = 0;
            pauseScreen.SetActive(true);
            Paused = true;
        }
        else
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1;
            Paused = false;
        }

    }
}
