using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI moneyText;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    internal void GetGameStatus(double moneyBalance, float minutes)
    {
        int decimals = (int)Math.Round(100.0 * (moneyBalance - Math.Truncate(moneyBalance)));
        moneyText.text = $"${Math.Truncate(moneyBalance)}<size=-14>{decimals.ToString("D2")}</size>";
        string hour = ZeroPadding(Mathf.FloorToInt(minutes / 60));
        string minute = ZeroPadding(Mathf.FloorToInt(minutes) % 60);
        timeText.text = hour + ':' + minute;

    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ResetScene()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }
    private string ZeroPadding(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }
}
