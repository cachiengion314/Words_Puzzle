﻿#if IAP && UNITY_PURCHASING
using UnityEngine.Purchasing;
#endif
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class ShopDialog2 : Dialog
{
    public Sprite hotSprite;
    public Sprite bestSprite;
    public Sprite candySprite;
    public Sprite adsSprite;

    public TextMeshProUGUI[] numHintTexts;
    public Text[] priceTexts, saleTexts;
    public Image[] hotImages;

    protected override void Start()
    {
        base.Start();

#if IAP && UNITY_PURCHASING
        Purchaser.instance.onItemPurchased += OnItemPurchased;

        for (int i = 0; i < numHintTexts.Length; i++)
        {
            //remove ads roi thi an cac iap ads
            if (Purchaser.instance.hintIapItems[i].removeAds
                && Purchaser.instance.hintIapItems[i].value <= 0
                && CUtils.IsAdsRemoved())
            {
                numHintTexts[i].transform.parent.gameObject.SetActive(false);
                continue;
            }
            else
            {
                numHintTexts[i].transform.parent.gameObject.SetActive(true);
            }

            numHintTexts[i].text = Purchaser.instance.hintIapItems[i].txtValue;
            priceTexts[i].text = Purchaser.instance.hintIapItems[i].price + "$";

            var txtSale = Purchaser.instance.hintIapItems[i].txtSale;
            if (txtSale.Equals("")) saleTexts[i].transform.parent.gameObject.SetActive(false);
            else saleTexts[i].text = txtSale;

            if (hotImages[i] != null)
            {
                if (Purchaser.instance.hintIapItems[i].txtHot.Equals("hot"))
                {
                    hotImages[i].sprite = hotSprite;
                    hotImages[i].gameObject.SetActive(true);
                }
                else if (Purchaser.instance.hintIapItems[i].txtHot.Equals("best"))
                {
                    hotImages[i].sprite = bestSprite;
                    hotImages[i].gameObject.SetActive(true);
                }
                else
                {
                    hotImages[i].gameObject.SetActive(false);
                }
            }
        }
#endif
    }

    private void Update()
    {

    }

    public void OnBuyProduct(int index)
	{
#if IAP && UNITY_PURCHASING
        Sound.instance.PlayButton();
        Purchaser.instance.BuyHintProduct(index);
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
            Toast.instance.ShowMessage("Your purchase is successful");
            if (Purchaser.instance.hintIapItems[index].removeAds)
            {
                CUtils.SetRemoveAds();
                for (int i = 0; i < numHintTexts.Length; i++)
                {
                    if (Purchaser.instance.hintIapItems[i].removeAds
                        && Purchaser.instance.hintIapItems[i].value <= 0)
                    {
                        numHintTexts[i].transform.parent.gameObject.SetActive(false);
                    }
                }
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
