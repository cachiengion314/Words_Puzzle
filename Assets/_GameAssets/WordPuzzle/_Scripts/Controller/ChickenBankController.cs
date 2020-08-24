using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class ChickenBankController : MonoBehaviour
{
    public static ChickenBankController instance;
    [SerializeField] private int _amount = 20;

    [SerializeField] private double _currStarChicken;

    [SerializeField] private double _remainChicken;

    public int Amount
    {
        get
        {
            return _amount;
        }
    }

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        _currStarChicken = CurrStarChicken;
        _remainChicken = FacebookController.instance.user.remainBank;
      
    }

    public double CurrStarChicken
    {
        get
        {
            //var result = FacebookController.instance.user.maxbank - CurrencyController.GetBalance();
            _currStarChicken = FacebookController.instance.user.currBank;/* + Mathf.Abs(ConfigController.instance.config.gameParameters.maxBank - Mathf.Abs((float)result));*/
            return _currStarChicken;
        }
    }

    public void AddtoBank()
    {
        if (Prefs.IsLastLevel())
            return;
        if (CurrStarChicken < ConfigController.instance.config.gameParameters.maxBank)
        {
            FacebookController.instance.user.currBank += _amount;
            if(CurrStarChicken > ConfigController.instance.config.gameParameters.maxBank)
            {
                var remainCurrBank = CurrStarChicken - ConfigController.instance.config.gameParameters.maxBank;
                FacebookController.instance.user.currBank = ConfigController.instance.config.gameParameters.maxBank;
                FacebookController.instance.user.remainBank += remainCurrBank;                
            }
            FacebookController.instance.SaveDataGame();
        }
        else
        {
            var remainCurrBank = CurrStarChicken - ConfigController.instance.config.gameParameters.maxBank;
            FacebookController.instance.user.currBank = ConfigController.instance.config.gameParameters.maxBank;
            FacebookController.instance.user.remainBank += _amount + remainCurrBank;
            FacebookController.instance.SaveDataGame();
        }
    }

    public void CollectBank(int value)
    {
        Sound.instance.Play(Sound.Collects.CoinCollect);
        if (CurrStarChicken >= ConfigController.instance.config.gameParameters.maxBank)
            CurrencyController.CreditBalance(ConfigController.instance.config.gameParameters.maxBank);
        else
            CurrencyController.CreditBalance((int)CurrStarChicken);
        FacebookController.instance.user.maxbank = /*CurrencyController.GetBalance() + */ConfigController.instance.config.gameParameters.maxBank;
        var nextCurrChicken = ConfigController.instance.config.gameParameters.minBank + FacebookController.instance.user.remainBank;
        if (nextCurrChicken > ConfigController.instance.config.gameParameters.maxBank)
        {
            FacebookController.instance.user.currBank = ConfigController.instance.config.gameParameters.maxBank;
            FacebookController.instance.user.remainBank = nextCurrChicken - ConfigController.instance.config.gameParameters.maxBank;
        }
        else
        {
            FacebookController.instance.user.currBank = ConfigController.instance.config.gameParameters.minBank + FacebookController.instance.user.remainBank;
            FacebookController.instance.user.remainBank = 0;
        }
        FacebookController.instance.SaveDataGame();
        if (HomeController.instance != null)
            HomeController.instance.ShowChickenBank();
    }
}
