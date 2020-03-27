using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardedVideoDialog : Dialog {
    public Text amountText;
    public Text messageText;

	public void SetAmount(int amount)
    {
        amountText.text = amount.ToString();
        messageText.text = "Congratulation! You got " + amount + " free rubies.";
    }

    public void Claim()
    {
        Close();
        Sound.instance.Play(Sound.Others.PopupOpen);
    }

	public override void Close ()
	{
		base.Close ();
		CurrencyController.CreditBalance(int.Parse(amountText.text));
	}
}
