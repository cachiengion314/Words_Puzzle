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

    private double _currStarChicken;

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _currStarChicken = CurrStarChicken;
    }

    public double CurrStarChicken
    {
        get
        {
            var result = FacebookController.instance.user.maxbank - CurrencyController.GetBalance();
            _currStarChicken = FacebookController.instance.user.currBank + Mathf.Abs(ConfigController.instance.config.gameParameters.maxBank - Mathf.Abs((float)result));
            return _currStarChicken;
        }
    }

    public void AddtoBank()
    {
        if (CurrStarChicken < ConfigController.instance.config.gameParameters.maxBank)
        {
            Sound.instance.Play(Sound.Collects.CoinCollect);
            FacebookController.instance.user.currBank += _amount;
            FacebookController.instance.SaveDataGame();
        }
    }

    public void CollectBank(int value)
    {
        Sound.instance.Play(Sound.Collects.CoinCollect);
        FacebookController.instance.user.maxbank = CurrencyController.GetBalance() + ConfigController.instance.config.gameParameters.maxBank;
        FacebookController.instance.user.currBank = 0;
        _currStarChicken = CurrStarChicken;
        FacebookController.instance.SaveDataGame();
    }
}
