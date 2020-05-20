using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FreeStarsDialogConfirm : MonoBehaviour
{
    private int _currAmount;
    private int _amount;
    [SerializeField] private TextMeshProUGUI _textAmount;
    [SerializeField] private RewardController _rewardController;
    [SerializeField] private Button _rewardButton;

    public void Setup(int amount, System.Action callback = null)
    {
        _amount = amount;
        _currAmount = 0;
        _textAmount.text = "X" + _amount;
        TweenControl.GetInstance().ScaleFromZero(gameObject, 0.3f, () =>
        {
            callback?.Invoke();
        });
    }

    public void OnClaimClick()
    {
        if (ExtraWord.instance != null)
            ExtraWord.instance.OnClaimed();
        _rewardController.overLay.SetActive(false);
        Sound.instance.Play(Sound.Others.PopupClose);
        BlockScreen.instance.Block(true);
        TweenControl.GetInstance().ScaleFromOne(gameObject, 0.3f, () =>
        {
            //Animate
            //StartCoroutine(CurrencyBalanceUpFx());
            StartCoroutine(ShowEffectCollect(_amount));
            BlockScreen.instance.Block(false);
            _rewardController.gameObject.SetActive(true);
            //==
        });
    }

    //private IEnumerator CurrencyBalanceUpFx()
    //{
    //    while (_currAmount < _amount)
    //    {
    //        Sound.instance.Play(Sound.Collects.CoinCollect);
    //        CurrencyController.CreditBalance(1);
    //        yield return new WaitForSeconds(0.002f);
    //        _currAmount += 1;
    //    }
    //}
    private IEnumerator ShowEffectCollect(int value)
    {
        MonoUtils.instance.ShowTotalStarCollect(value,null);
        var result = value / 5;
        for (int i = 0; i < value; i++)
        {
            if (i < 5)
            {
                MonoUtils.instance.ShowEffect(result, null, null, _rewardButton.transform);
            }
            yield return new WaitForSeconds(0.06f);
        }

    }
}
