using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EffectController : MonoBehaviour
{
    public static EffectController instance;

    private readonly string IS_EFFECT_ON = "Is_Effect_On";

    private bool isEffectOn;
    public bool IsEffectOn
    {
        get
        {
            int intToBool = PlayerPrefs.GetInt(IS_EFFECT_ON); // if (intToBool == 0) its the first time installed the game, always isEffectOn == true
            isEffectOn = true;
            if (intToBool == -1) { isEffectOn = false; }
            return isEffectOn;
        }
        set
        {
            isEffectOn = value;
            int intTobool = -1;
            if (isEffectOn) { intTobool = 1; }
            PlayerPrefs.SetInt(IS_EFFECT_ON, intTobool);
        }
    }

    private void Awake()
    {
        instance = this;
    }

}
