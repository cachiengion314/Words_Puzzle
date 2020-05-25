﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;

public class ChickenBankDialog : Dialog
{
    [SerializeField] private TextMeshProUGUI _textPrice;
    [SerializeField] private TextMeshProUGUI _textReward;
    [SerializeField] private TextMeshProUGUI _textMaxOut;
    [SerializeField] private int _indexItem = 8;

    protected override void Start()
    {
        base.Start();
        var resultValue = ChickenBankController.instance.CurrStarChicken >= ConfigController.instance.config.gameParameters.maxBank ?
                    ConfigController.instance.config.gameParameters.maxBank : /*currValue*/ChickenBankController.instance.CurrStarChicken;
        _textPrice.text = Purchaser.instance.iapItems[_indexItem].price + "$";
        _textReward.text = "X" + resultValue.ToString();
        if (ChickenBankController.instance.CurrStarChicken >= ConfigController.instance.config.gameParameters.maxBank)
            _textMaxOut.gameObject.SetActive(true);
        else
            _textMaxOut.gameObject.SetActive(false);
#if IAP && UNITY_PURCHASING
        Purchaser.instance.onItemPurchased += OnItemPurchased;
#endif
    }

    public void OnBuyProduct(int index)
    {
#if IAP && UNITY_PURCHASING
        Sound.instance.Play(Sound.Others.PopupOpen);
        Purchaser.instance.BuyProduct(index);
#else
        Debug.LogError("Please enable, import and install Unity IAP to use this function");
#endif
    }

#if IAP && UNITY_PURCHASING
    private void OnItemPurchased(IAPItem item, int index)
    {
        // A consumable product has been purchased by this user.
        if (item.productType == ProductType.Consumable)
        {
            if (item.productID == "word.chickenbank")
                ChickenBankController.instance.CollectBank(item.value);
            Toast.instance.ShowMessage("Your purchase is successful");
            _textReward.text = "X" + ChickenBankController.instance.CurrStarChicken.ToString();
            Close();
        }
        // Or ... a non-consumable product has been purchased by this user.
        else if (item.productType == ProductType.NonConsumable)
        {
            // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
        }
        // Or ... a subscription product has been purchased by this user.
        else if (item.productType == ProductType.Subscription)
        {
            // TODO: The subscription item has been successfully purchased, grant this to the player.
        }
    }
#endif

#if IAP && UNITY_PURCHASING
    private void OnDestroy()
    {
        Purchaser.instance.onItemPurchased -= OnItemPurchased;
    }
#endif
}
