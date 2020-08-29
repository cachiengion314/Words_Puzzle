using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class DontLikeAdsDialog : Dialog
{
    [Header("THEME UI CHANGE")]
    [SerializeField] private Image _itemTop;
    [SerializeField] private Image _bgItemSmall;
    [SerializeField] private Image _iconStarItem2;
    [SerializeField] private Image _iconStarItem3;
    [SerializeField] private Image _iconHintItem2;
    [SerializeField] private Image _btnMore;
    [SerializeField] private List<Image> _iconsNoAds;
    [SerializeField] private List<Image> _btnPrice;
    [SerializeField] private List<Text> _textPrice;
    [SerializeField] private List<Text> _textContent;

    void Start()
    {
#if IAP && UNITY_PURCHASING
        Purchaser.instance.onItemPurchased += OnItemPurchased;

#endif
        CheckTheme();
    }

    private void CheckTheme()
    {
        if (MainController.instance != null)
        {
            var currTheme = ThemesControl.instance.CurrTheme;
            _itemTop.sprite = currTheme.uiData.dontLikeAdsData.itemTop;
            //_bgItemSmall.sprite = currTheme.uiData.dontLikeAdsData.bgItemSmall;
            //_iconStarItem2.sprite = currTheme.uiData.dontLikeAdsData.iconStar;
            //_iconStarItem3.sprite = currTheme.uiData.dontLikeAdsData.iconStar;
            //_iconHintItem2.sprite = currTheme.uiData.dontLikeAdsData.iconHint;
            //_btnMore.sprite = currTheme.uiData.dontLikeAdsData.btnMore;

            //_itemTop.SetNativeSize();
            //_bgItemSmall.SetNativeSize();
            //_iconStarItem2.SetNativeSize();
            //_iconStarItem3.SetNativeSize();
            //_iconHintItem2.SetNativeSize();
            //_btnMore.SetNativeSize();

            var indexIcon = 0;
            foreach (var icon in _iconsNoAds)
            {
                if (icon != null)
                {
                    icon.sprite = currTheme.uiData.dontLikeAdsData.iconsNoAds[indexIcon];
                    //icon.SetNativeSize();
                    indexIcon++;
                }
            }

            foreach (var btn in _btnPrice)
            {
                if (btn != null)
                {
                    btn.sprite = currTheme.uiData.dontLikeAdsData.btnPrice;
                    //btn.SetNativeSize();
                }
            }

            foreach (var text in _textPrice)
            {
                if (text != null)
                    text.color = currTheme.uiData.dontLikeAdsData.colorTextBtn;
            }

            foreach (var text in _textContent)
            {
                if (text != null)
                    text.color = currTheme.fontData.colorContentDialog;
            }
        }
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
            if (item.value > 0)
                CurrencyController.CreditBalance(item.value);
            if (item.valueBeehive > 0)
                BeeManager.instance.CreaditAmountBee(item.valueBeehive);
            if (item.valueHint > 0)
                CurrencyController.CreditHintFree(item.valueHint);
            if (item.valueMultipleHint > 0)
                CurrencyController.CreditMultipleHintFree(item.valueMultipleHint);
            if (item.valueSelectedHint > 0)
                CurrencyController.CreditSelectedHintFree(item.valueSelectedHint);

            Toast.instance.ShowMessage("Your purchase is successful");
            if (Purchaser.instance.iapItems[index].removeAds)
            {
                CUtils.SetRemoveAds();
            }

            Firebase.Analytics.FirebaseAnalytics.LogEvent(
              Firebase.Analytics.FirebaseAnalytics.EventSpendVirtualCurrency,
              new Firebase.Analytics.Parameter[] {
                new Firebase.Analytics.Parameter(
                  Firebase.Analytics.FirebaseAnalytics.ParameterItemName, item.productID),
                new Firebase.Analytics.Parameter(
                  Firebase.Analytics.FirebaseAnalytics.ParameterValue, item.value),
                new Firebase.Analytics.Parameter(
                  Firebase.Analytics.FirebaseAnalytics.ParameterVirtualCurrencyName, item.productID),
              }
            );
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
