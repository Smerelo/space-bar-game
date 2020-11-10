using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cv 
{
    public string name { get; private set; }
    public float price { get; private set; }
    public float prepareSpeeed { get; private set; }
    public float cleanSpeed { get; private set; }
    public float serveSpeed { get; private set; }

    public Cv(string m_name, float m_price, float m_prepareSpeed, float m_cleanSpeed, float m_serveSpeed)
    {
        name = m_name;
        price = m_price;
        prepareSpeeed = m_prepareSpeed;
        cleanSpeed = m_cleanSpeed;
        serveSpeed = m_serveSpeed;
    }

}
