using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
   public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();

    }
    public void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            Screen.fullScreen = true;
        }
        if (Screen.fullScreen && Input.GetKeyDown("escape"))
        {
            Screen.fullScreen = false;
        }
    }
}
