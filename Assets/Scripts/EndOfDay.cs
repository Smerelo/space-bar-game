using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class EndOfDay : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI moneyText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    public void SetGameStatus(double moneyBalance)
    {
        Debug.Log(moneyBalance);
        if (moneyBalance >= 0)
        {
            moneyText.color = Colors.Instance.positiveBalance;
        }
        else
        {
            moneyText.color = Colors.Instance.negativeBalance;
        }
        int decimals = Mathf.Abs((int)Math.Round(100.0 * (moneyBalance - Math.Truncate(moneyBalance))));
        moneyText.text = $"${Math.Truncate(moneyBalance)}<size=-14>{decimals.ToString("D2")}</size>";
    }
}
