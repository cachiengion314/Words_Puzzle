﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeManager : MonoBehaviour
{
    private int _currBee;
    public static BeeManager instance;

    public int CurrBee
    {
        get
        {
            return CPlayerPrefs.GetInt("amount_bee", 0);
        }
    }
    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void Load(int amount)
    {
        _currBee = amount;
    }

    public void SetAmountBee(int number)
    {
        _currBee += number;
        if (_currBee <= 0)
            _currBee = 0;
        CPlayerPrefs.SetInt("amount_bee", _currBee);
    }
}
