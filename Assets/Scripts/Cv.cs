﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cv 
{
    public string name { get; private set; }
    public float price { get; private set; }
    public float taskSpeed { get; private set; }
    public float moveSpeed { get; private set; }
    public int employeeType { get; set; }
    

    public Cv(string m_name, float m_price, float m_prepareSpeed, float m_moveSpeed, int m_employeeType)
    {
        name = m_name;
        price = m_price;
        taskSpeed = m_prepareSpeed;
        moveSpeed = m_moveSpeed;
        employeeType = m_employeeType;
    }

}
