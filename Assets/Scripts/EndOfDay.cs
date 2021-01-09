using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class EndOfDay : MonoBehaviour
{

    [SerializeField] public TextMeshProUGUI moneyText;
    public double balance;
    private CentralTransactionLogic ctl;
    // Start is called before the first frame update
    void Start()
    {
        ctl = GameObject.Find("SpaceCantina").GetComponent<CentralTransactionLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    public void SetGameStatus(double moneyBalance)
    {
        balance = moneyBalance;
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

    internal void UpdateBalance(float m)
    {
        balance -= m;
        int decimals = Mathf.Abs((int)Math.Round(100.0 * (balance - Math.Truncate(balance))));
        moneyText.text = $"${Math.Truncate(balance)}<size=-14>{decimals.ToString("D2")}</size>";
        ctl.CashIn(-m);
        
    }
}
