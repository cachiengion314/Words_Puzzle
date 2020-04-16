using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FreeStarsDialogConfirm : MonoBehaviour
{
    private int _currAmount;
    private int _amount;
    [SerializeField] private TextMeshProUGUI _textAmount;
    [SerializeField] private RewardController _rewardController;

    public void Setup(int amount, System.Action callback = null)
    {
        _amount = amount;
        _currAmount = 0;
        _textAmount.text = "X" + _amount;
        Sound.instance.Play(Sound.Others.PopupOpen);
        TweenControl.GetInstance().ScaleFromZero(gameObject, 0.3f, () =>
        {
            callback?.Invoke();
        });
    }

    public void OnClaimClick()
    {
        _rewardController.overLay.SetActive(false);
        Sound.instance.Play(Sound.Others.PopupClose);
        BlockScreen.instance.Block(true);
        TweenControl.GetInstance().ScaleFromOne(gameObject, 0.3f, () =>
        {
            //Animate
            StartCoroutine(CurrencyBalanceUpFx());
            BlockScreen.instance.Block(false);
            _rewardController.gameObject.SetActive(true);
            //==
        });
    }

    private IEnumerator CurrencyBalanceUpFx()
    {
        while (_currAmount < _amount)
        {
            Sound.instance.Play(Sound.Collects.CoinCollect);
            CurrencyController.CreditBalance(1);
            yield return new WaitForSeconds(0.002f);
            _currAmount += 1;
        }
    }
}
