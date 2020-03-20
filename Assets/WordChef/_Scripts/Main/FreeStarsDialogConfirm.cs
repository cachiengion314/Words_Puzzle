using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FreeStarsDialogConfirm : MonoBehaviour
{
    private int _amount;
    [SerializeField] private TextMeshProUGUI _textAmount;

    public void Setup(int amount, System.Action callback = null)
    {
        _amount = amount;
        _textAmount.text = "x " + _amount;
        TweenControl.GetInstance().ScaleFromZero(gameObject, 0.3f,()=> {
            callback?.Invoke();
        });
    }

    public void OnClaimClick()
    {
        BlockScreen.instance.Block(true);
        TweenControl.GetInstance().Scale(gameObject, Vector3.zero, 0.3f, () =>
        {
            //Animate
            CurrencyController.CreditBalance(_amount);
            BlockScreen.instance.Block(false);
            //==
        });
    }

    public void Close()
    {
        TweenControl.GetInstance().Scale(gameObject, Vector3.zero, 0.3f, () =>
        {

        });
    }
}
