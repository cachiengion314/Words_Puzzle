#if IAP && UNITY_PURCHASING
using UnityEngine.Purchasing;
#endif
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using System.Collections;
using Utilities.Components;

public class ShopDialog : Dialog
{
    public Sprite hotSprite;
    public Sprite bestSprite;
    public Sprite candySprite;
    public Sprite adsSprite;

    public TextMeshProUGUI[] numRubyTexts;
    public Text[] priceTexts, saleTexts;
    public Image[] hotImages, candyImages;
    public ScrollRect scroll;
    public RectTransform scrollRT;
    public GameObject btnMore;
    public Text[] limitTimeTexts;

    private float currentTimeVipPack;
    private float[] maxTimeVipPacks;

    public GameObject contentItemShop;
    public GameObject[] shopItemObject;

    protected override void Start()
    {
        base.Start();

#if IAP && UNITY_PURCHASING
        CUtils.SetRunVipPack();
        long span = (long)DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        currentTimeVipPack = (float)(span - CUtils.GetTimeVipPackStarted());
        
        Purchaser.instance.onItemPurchased += OnItemPurchased;

        maxTimeVipPacks = new float[numRubyTexts.Length];
        for (int i = 0; i < numRubyTexts.Length; i++)
        {
            //vip pack la limitTime > 0
            if (Purchaser.instance.iapItems[i].limitTime > 0)
            {
                maxTimeVipPacks[i] = Purchaser.instance.iapItems[i].limitTime;

                if (currentTimeVipPack > maxTimeVipPacks[i] || CUtils.IsBuyVipPack(i))
                {
                    numRubyTexts[i].transform.parent.gameObject.SetActive(false);
                    continue;
                }
            }

            //remove ads roi thi an cac iap ads
            if (Purchaser.instance.iapItems[i].removeAds
                && Purchaser.instance.iapItems[i].value <= 0
                && CUtils.IsAdsRemoved())
            {
                numRubyTexts[i].transform.parent.gameObject.SetActive(false);
                continue;
            }
            else
            {
                numRubyTexts[i].transform.parent.gameObject.SetActive(true);
            }

            numRubyTexts[i].text = Purchaser.instance.iapItems[i].txtValue;
            priceTexts[i].text = Purchaser.instance.iapItems[i].price + "$";

            var txtSale = Purchaser.instance.iapItems[i].txtSale;
            if (txtSale.Equals("")) saleTexts[i].transform.parent.gameObject.SetActive(false);
            else saleTexts[i].text = txtSale;

            if (hotImages[i] != null)
            {
                if (Purchaser.instance.iapItems[i].txtHot.Equals("hot"))
                {
                    hotImages[i].sprite = hotSprite;
                    hotImages[i].gameObject.SetActive(true);
                }
                else if (Purchaser.instance.iapItems[i].txtHot.Equals("best"))
                {
                    hotImages[i].sprite = bestSprite;
                    hotImages[i].gameObject.SetActive(true);
                }
                else
                {
                    hotImages[i].gameObject.SetActive(false);
                }
            }

            if (candyImages[i] != null)
            {
                if (Purchaser.instance.iapItems[i].txtValue.Equals("remove ads"))
                {
                    candyImages[i].sprite = adsSprite;
                }
                else
                {
                    candyImages[i].sprite = candySprite;
                }
            }
        }
#endif
        GetShopItemInContent();
    }

    private void Update()
    {
        /*if(scroll.verticalNormalizedPosition >= 0.9f && scroll.content.sizeDelta.y > scrollRT.sizeDelta.y)
        {
            btnMore.SetActive(true);
        }
        else
        {
            btnMore.SetActive(false);
        }*/

        currentTimeVipPack += Time.deltaTime;
        for (int i = 0; i < numRubyTexts.Length; i++)
        {
            //vip pack la limitTime > 0
            if (maxTimeVipPacks[i] > 0)
            {
                int time = (int) (maxTimeVipPacks[i] - currentTimeVipPack);
                int hours = time / 3600;
                int mins = (time % 3600) / 60;
                int secs = time % 60;
                limitTimeTexts[i].text = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, mins, secs);

                if (currentTimeVipPack > maxTimeVipPacks[i])
                {
                    numRubyTexts[i].transform.parent.gameObject.SetActive(false);
                }
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
            CurrencyController.CreditBalance(item.value);
            Toast.instance.ShowMessage("Your purchase is successful");
            if (Purchaser.instance.iapItems[index].removeAds)
            {
                CUtils.SetRemoveAds();
                for (int i = 0; i < numRubyTexts.Length; i++)
                {
                    if (Purchaser.instance.iapItems[i].removeAds
                        && Purchaser.instance.iapItems[i].value <= 0)
                    {
                        numRubyTexts[i].transform.parent.gameObject.SetActive(false);
                    }
                }
            }
            if(maxTimeVipPacks[index] > 0)
            {
                CUtils.SetBuyVipPack(index);
                numRubyTexts[index].transform.parent.gameObject.SetActive(false);
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

    public void More()
    {
        //scroll.DOVerticalNormalizedPos(0f, 1f);
        int count = 0;
        btnMore.gameObject.SetActive(false);

        for (int i = 0; i < shopItemObject.Length; i++)
        {
            if (i > 1)
            {
                shopItemObject[i].GetComponent<SimpleTMPButton>().enabled = false;
                shopItemObject[i].transform.localScale = Vector3.zero;
                if (shopItemObject[i] != btnMore)
                {
                    shopItemObject[i].SetActive(true);
                    StartCoroutine(DelayPlayAnimation(shopItemObject[i], count * 0.1f));
                    count++;
                }
            }
        }
        scroll.GetComponent<ScrollRect>().enabled = true;
    }

    void GetShopItemInContent()
    {
        int count = 0;
        shopItemObject = new GameObject[contentItemShop.transform.childCount];
        //btnMore.transform.localScale = Vector3.zero;
        for (int i = 0; i < contentItemShop.transform.childCount; i++)
        {
            shopItemObject[i] = contentItemShop.transform.GetChild(i).gameObject;
            if (i > 1)
            {
                shopItemObject[i].transform.localScale = Vector3.zero;
                if (shopItemObject[i].activeInHierarchy)
                {
                    StartCoroutine(DelayPlayAnimation(shopItemObject[i], count * 0.1f + 0.5f));
                    count++;
                } 
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
}
