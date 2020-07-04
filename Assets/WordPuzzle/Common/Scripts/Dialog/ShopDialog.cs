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
using UnityEngine.SceneManagement;

public class ShopDialog : Dialog
{
    public Sprite hotSprite;
    public Sprite bestSprite;
    public Sprite candySprite;
    public Sprite adsSprite;

    public ItemShop[] _items;
    public TextMeshProUGUI[] numRubyTexts;
    public TextMeshProUGUI[] _numBeehiveTexts;
    public TextMeshProUGUI[] _numHintTexts;
    public TextMeshProUGUI[] _numMultipleHintTexts;
    public TextMeshProUGUI[] _numSelectedHintTexts;
    public Text[] priceTexts, saleTexts, bonusTexts;
    public Image[] hotImages, candyImages;
    public ScrollRect scroll;
    public RectTransform scrollRT;
    public GameObject btnMore;
    public TextMeshProUGUI[] limitTimeTexts;

    private float currentTimeVipPack;
    private float[] maxTimeVipPacks;

    public GameObject contentItemShop;
    public GameObject[] shopItemObject;
    public GameObject chickenBank;

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
            if (numRubyTexts[i] != null)
            {
                _items[i].idProduct = i;
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
                    if (i != 0)
                        numRubyTexts[i].transform.parent.gameObject.SetActive(true);
                }

                //var currStar = CurrencyController.GetBalance();
                //var currValue = FacebookController.instance.user.maxbank - currStar;
                var resultValue = ChickenBankController.instance.CurrStarChicken >= ConfigController.instance.config.gameParameters.maxBank ?
                    ConfigController.instance.config.gameParameters.maxBank : /*currValue*/ChickenBankController.instance.CurrStarChicken;
                if (i == 7)
                {
                    Purchaser.instance.iapItems[i].value = (int)resultValue;
                    Purchaser.instance.iapItems[i].txtValue = resultValue.ToString();
                }
                numRubyTexts[i].text = Purchaser.instance.iapItems[i].txtValue;
                var priceLocalize = "";
                try
                {
                    priceLocalize = Purchaser.instance.GetLocalizePrice(Purchaser.instance.iapItems[i].productID);
                    priceTexts[i].text = (priceLocalize == "" || priceLocalize == null) ? Purchaser.instance.iapItems[i].price + "$" : priceLocalize;
                }
                catch (Exception)
                {
                    Debug.Log("GetLocalizePrice Error");
                }
                priceTexts[i].text = (priceLocalize == "" || priceLocalize == null) ? Purchaser.instance.iapItems[i].price + "$" : priceLocalize;

                if (_numBeehiveTexts[i] != null) _numBeehiveTexts[i].text = "" + Purchaser.instance.iapItems[i].valueBeehive;
                if (_numHintTexts[i] != null) _numHintTexts[i].text = "" + Purchaser.instance.iapItems[i].valueHint;
                if (_numMultipleHintTexts[i] != null) _numMultipleHintTexts[i].text = "" + Purchaser.instance.iapItems[i].valueMultipleHint;
                if (_numSelectedHintTexts[i] != null) _numSelectedHintTexts[i].text = "" + Purchaser.instance.iapItems[i].valueSelectedHint;

                var txtSale = Purchaser.instance.iapItems[i].txtSale;
                if (txtSale.Equals("")) saleTexts[i].transform.parent.gameObject.SetActive(false);
                else saleTexts[i].text = txtSale;

                if (Purchaser.instance.iapItems[i].isBonus && Purchaser.instance.iapItems[i].productID == "chicken_bank")
                {
                    var bonus = (ChickenBankController.instance.CurrStarChicken - ConfigController.instance.config.gameParameters.minBank) / ConfigController.instance.config.gameParameters.minBank * 100;
                    if (saleTexts[i] != null)
                        saleTexts[i].transform.parent.gameObject.SetActive(false);
                    if (bonusTexts[i] != null && bonus > 0)
                    {
                        bonusTexts[i].transform.parent.gameObject.SetActive(true);
                        bonusTexts[i].text = "+" + (int)bonus + "%";
                    }
                }
                else
                {
                    if (bonusTexts[i] != null)
                        bonusTexts[i].transform.parent.gameObject.SetActive(false);
                }

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
        }
#endif
        GetShopItemInContent();
        //More();
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
                int time = (int)(maxTimeVipPacks[i] - currentTimeVipPack);
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
            if (item.productID == "chicken_bank")
            {
                ChickenBankController.instance.CollectBank(item.value);
                var resultValue = ChickenBankController.instance.CurrStarChicken >= ConfigController.instance.config.gameParameters.maxBank ?
                    ConfigController.instance.config.gameParameters.maxBank : /*currValue*/ChickenBankController.instance.CurrStarChicken;
                item.value = (int)resultValue;
                item.txtValue = resultValue.ToString();
                numRubyTexts[index].text = item.txtValue;
                var valueShow = (ConfigController.instance.config.gameParameters.minBank * 10 / 100) + ConfigController.instance.config.gameParameters.minBank;
                var currStarBank = ChickenBankController.instance.CurrStarChicken;
                if (currStarBank < valueShow)
                    chickenBank.SetActive(false);
            }
            else
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
                for (int i = 0; i < numRubyTexts.Length; i++)
                {
                    if (Purchaser.instance.iapItems[i].removeAds
                        && Purchaser.instance.iapItems[i].value <= 0)
                    {
                        numRubyTexts[i].transform.parent.gameObject.SetActive(false);
                    }
                }
            }
            if (maxTimeVipPacks[index] > 0)
            {
                CUtils.SetBuyVipPack(index);
                numRubyTexts[index].transform.parent.gameObject.SetActive(false);
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
                    StartCoroutine(DelayPlayAnimation(shopItemObject[i], count * 0.1f + 0.5f));
                    count++;
                }
            }
        }
        scroll.GetComponent<ScrollRect>().enabled = true;
    }

    void GetShopItemInContent()
    {
        var valueShow = (ConfigController.instance.config.gameParameters.minBank * 10 / 100) + ConfigController.instance.config.gameParameters.minBank;
        var currStarBank = ChickenBankController.instance.CurrStarChicken;
        int count = 0;
        btnMore.gameObject.SetActive(false);
        shopItemObject = new GameObject[contentItemShop.transform.childCount];
        //btnMore.transform.localScale = Vector3.zero;
        for (int i = 0; i < contentItemShop.transform.childCount; i++)
        {
            if (currStarBank < valueShow)
                chickenBank.SetActive(false);
            else
            {
                chickenBank.SetActive(true);
                if (!CPlayerPrefs.HasKey("OPEN_CHICKEN"))
                    CPlayerPrefs.SetBool("OPEN_CHICKEN", true);
            }


            shopItemObject[i] = contentItemShop.transform.GetChild(i).gameObject;
            var itemShop = shopItemObject[i].gameObject.GetComponent<ItemShop>().idProduct;
            if (i > 0)
            {
                shopItemObject[i].transform.localScale = Vector3.zero;
                if (ConfigController.instance.isShopHint)
                {
                    if (Purchaser.instance.iapItems[itemShop].valueHint > 0 || Purchaser.instance.iapItems[itemShop].valueMultipleHint > 0 || Purchaser.instance.iapItems[itemShop].valueSelectedHint > 0)
                        shopItemObject[i].gameObject.SetActive(true);
                    else
                        shopItemObject[i].gameObject.SetActive(false);
                }

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
    public void LoadAllBannerWhenClose()
    {
        AudienceNetworkBanner.instance.LoadBanner();
    }
}
