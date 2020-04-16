using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using static CUtils;

public class CurrencyBallance : MonoBehaviour {
    private void Start()
    {
        UpdateBalance();
        CurrencyController.onBalanceChanged += OnBalanceChanged;
    }

    private void UpdateBalance()
    {
        var currency = AbbrevationUtility.AbbreviateNumber(CurrencyController.GetBalance());
        gameObject.SetText(currency);
    }

    private void OnBalanceChanged()
    {
        UpdateBalance();
    }

    private void OnDestroy()
    {
        CurrencyController.onBalanceChanged -= OnBalanceChanged;
    }
}
