﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardedVideoDialog : Dialog
{
    public Text amountText;
    public Text messageText;

    private void Start()
    {
        SetAmount(20);
    }

    public void SetAmount(int amount)
    {
        amountText.text = "X" + amount.ToString();
        messageText.text = "Congratulation! You got " + amount + " free rubies.";
    }

    public void Claim()
    {
        Close();
        Sound.instance.Play(Sound.Others.PopupOpen);
    }

    public void OnConfirmClick()
    {
        Sound.instance.Play(Sound.Others.PopupOpen);
        StartCoroutine(ShowEffectCollect(int.Parse(amountText.text.Replace("X", ""))));
    }

    public override void Close()
    {
        base.Close();
        CurrencyController.CreditBalance(int.Parse(amountText.text.Replace("X", "")));
    }

    private IEnumerator ShowEffectCollect(int value)
    {
        var result = value / 5;
        for (int i = 0; i < value; i++)
        {
            if (i < 5)
            {
                MonoUtils.instance.ShowEffect(result);
            }
            yield return new WaitForSeconds(0.02f);
        }

    }
}
