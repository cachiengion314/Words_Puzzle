using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardedVideoDialog : Dialog
{
    [SerializeField] private Button _btnReward;
    [SerializeField] private int _amount = 20;
    public TextMeshProUGUI amountText;
    public Text messageText;

    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        SetAmount(_amount);
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
        StartCoroutine(ShowEffectCollect(_amount));
        TweenControl.GetInstance().DelayCall(transform, 0.2f,()=> {
            Close();
        });
    }

    //public override void Close()
    //{
    //    base.Close();
    //    CurrencyController.CreditBalance(_amount);
    //}

    private IEnumerator ShowEffectCollect(int value)
    {
        MonoUtils.instance.ShowTotalStarCollect(value,null);
        var result = value / 5;
        for (int i = 0; i < value; i++)
        {
            if (i < 5)
            {
                MonoUtils.instance.ShowEffect(result,null,null,_btnReward.transform);
            }
            yield return new WaitForSeconds(0.06f);
        }
    }
}
