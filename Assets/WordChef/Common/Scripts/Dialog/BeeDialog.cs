#if IAP && UNITY_PURCHASING
using UnityEngine.Purchasing;
#endif
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using Utilities.Components;

public class BeeDialog : Dialog
{

    public Sprite hotSprite;
    public Sprite bestSprite;

    public TextMeshProUGUI[] numHintTexts;
    public Text[] priceTexts, saleTexts;
    public Image[] hotImages;


    public GameObject contentItemShop;
    public GameObject[] shopItemObject;


    //public GameObject shadowPanelHowToPlay;
    protected override void Start()
    {
        base.Start();

#if IAP && UNITY_PURCHASING
        Purchaser.instance.onItemPurchased += OnItemPurchased;

        for (int i = 0; i < numHintTexts.Length; i++)
        {
            //remove ads roi thi an cac iap ads
            if (Purchaser.instance.beeIapItems[i].removeAds
                && Purchaser.instance.beeIapItems[i].value <= 0
                && CUtils.IsAdsRemoved())
            {
                numHintTexts[i].transform.parent.gameObject.SetActive(false);
                continue;
            }
            else
            {
                numHintTexts[i].transform.parent.gameObject.SetActive(true);
            }

            numHintTexts[i].text = Purchaser.instance.beeIapItems[i].txtValue;
            priceTexts[i].text = Purchaser.instance.beeIapItems[i].price + "$";

            var txtSale = Purchaser.instance.beeIapItems[i].txtSale;
            if (txtSale.Equals("")) saleTexts[i].transform.parent.gameObject.SetActive(false);
            else saleTexts[i].text = txtSale;

            if (hotImages[i] != null)
            {
                if (Purchaser.instance.beeIapItems[i].txtHot.Equals("hot"))
                {
                    hotImages[i].sprite = hotSprite;
                    hotImages[i].gameObject.SetActive(true);
                }
                else if (Purchaser.instance.beeIapItems[i].txtHot.Equals("best"))
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
        Sound.instance.Play(Sound.Others.PopupOpen);
        Purchaser.instance.BuyBeeProduct(index);
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
            BeeManager.instance.CreaditAmountBee(item.value);
            Toast.instance.ShowMessage("Your purchase is successful");
            if (Purchaser.instance.beeIapItems[index].removeAds)
            {
                CUtils.SetRemoveAds();
                for (int i = 0; i < numHintTexts.Length; i++)
                {
                    if (Purchaser.instance.beeIapItems[i].removeAds
                        && Purchaser.instance.beeIapItems[i].value <= 0)
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

    void GetShopItemInContent()
    {
        int count = 0;
        shopItemObject = new GameObject[contentItemShop.transform.childCount];
        for (int i = 0; i < contentItemShop.transform.childCount; i++)
        {
            shopItemObject[i] = contentItemShop.transform.GetChild(i).gameObject;
            shopItemObject[i].transform.localScale = Vector3.zero;
        }

        for (int i = 0; i < shopItemObject.Length; i++)
        {
            if (shopItemObject[i].activeInHierarchy)
            {
                StartCoroutine(DelayPlayAnimation(shopItemObject[i], count * 0.1f));
                count++;
            }
        }
    }

    IEnumerator DelayPlayAnimation(GameObject item, float time)
    {
        yield return new WaitForSeconds(time);
        item.GetComponent<DOTweenAnimation>().DORestart();
        yield return new WaitForSeconds(item.GetComponent<DOTweenAnimation>().duration);
        item.GetComponent<SimpleTMPButton>().enabled = true;
    }

    public void OnClickHowToPlayButton(int selectID)
    {
        //shadowPanelHowToPlay.SetActive(true);
        //TweenControl.GetInstance().ScaleFromZero(panelHowToPlay, 0.3f);
        DialogController.instance.ShowDialog(DialogType.HowtoPlay, DialogShow.STACK_DONT_HIDEN);
        Sound.instance.Play(Sound.Others.PopupOpen);
        HowToPlayDialog.instance.ShowMeanWordByID(selectID);
    }

    public void OnClickCloseHowToPlayPanelButton()
    {
        //shadowPanelHowToPlay.SetActive(false);
        //TweenControl.GetInstance().ScaleFromOne(panelHowToPlay, 0.3f);
    }

}
