using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardedVideoDialog : Dialog {
    public Text amountText;
    public Text messageText;

    private void Start()
    {
        SetAmount(40);
    }

    public void SetAmount(int amount)
    {
        amountText.text = "x " + amount.ToString();
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
        MonoUtils.instance.ShowEffect(int.Parse(amountText.text.Replace("x ","")));
    }

    public override void Close ()
	{
		base.Close ();
		CurrencyController.CreditBalance(int.Parse(amountText.text.Replace("x ", "")));
	}
}
