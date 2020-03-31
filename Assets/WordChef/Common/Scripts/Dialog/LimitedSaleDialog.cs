using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class LimitedSaleDialog : Dialog
{
    public TextMeshProUGUI numRubyTexts;
    public TextMeshProUGUI priceTexts;
    [SerializeField] private int _indexIAPItem = 6;

    private float _currentTimeVipPack;
    private float _maxTimeVipPacks;

    protected override void Start()
    {
        base.Start();

#if IAP && UNITY_PURCHASING
        CUtils.SetRunVipPack();
        long span = (long)DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        _currentTimeVipPack = (float)(span - CUtils.GetTimeVipPackStarted());

        Purchaser.instance.onItemPurchased += OnItemPurchased;

        _maxTimeVipPacks = 1;
        //vip pack la limitTime > 0
        if (Purchaser.instance.iapItems[_indexIAPItem].limitTime > 0)
        {
            _maxTimeVipPacks = Purchaser.instance.iapItems[_indexIAPItem].limitTime;

            if (_currentTimeVipPack > _maxTimeVipPacks || CUtils.IsBuyVipPack(_indexIAPItem))
            {
                numRubyTexts.transform.parent.gameObject.SetActive(false);
            }
        }

        //remove ads roi thi an cac iap ads
        if (Purchaser.instance.iapItems[_indexIAPItem].removeAds
            && Purchaser.instance.iapItems[_indexIAPItem].value <= 0
            && CUtils.IsAdsRemoved())
        {
            numRubyTexts.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            numRubyTexts.transform.parent.gameObject.SetActive(true);
        }

        numRubyTexts.text = Purchaser.instance.iapItems[_indexIAPItem].txtValue;
        priceTexts.text = Purchaser.instance.iapItems[_indexIAPItem].price + "$";
#endif
    }

    public void OnBuyProduct()
    {
#if IAP && UNITY_PURCHASING
        Sound.instance.Play(Sound.Others.PopupOpen);
        Purchaser.instance.BuyProduct(_indexIAPItem);
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
            CurrencyController.CreditBalance(item.value);
            CurrencyController.CreditHintFree(5);
            Toast.instance.ShowMessage("Your purchase is successful");
            if (Purchaser.instance.iapItems[index].removeAds)
            {
                CUtils.SetRemoveAds();
                if (Purchaser.instance.iapItems[_indexIAPItem].removeAds
                    && Purchaser.instance.iapItems[_indexIAPItem].value <= 0)
                {
                    numRubyTexts.transform.parent.gameObject.SetActive(false);
                }
            }
            if (_maxTimeVipPacks > 0)
            {
                CUtils.SetBuyVipPack(index);
                numRubyTexts.transform.parent.gameObject.SetActive(false);
            }
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
