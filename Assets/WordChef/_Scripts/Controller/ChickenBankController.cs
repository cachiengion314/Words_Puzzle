using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenBankController : MonoBehaviour
{
    public static ChickenBankController instance;
    [SerializeField] private int _amount = 20;

    private bool _chieckbank;
    private int _maxstar;
    private int _currStarChicken;

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public bool IsChickenBank
    {
        get
        {
            _chieckbank = FacebookController.instance.user.isChickenBank;
            return _chieckbank;
        }
        set
        {
            _chieckbank = value;
            FacebookController.instance.user.isChickenBank = _chieckbank;
            FacebookController.instance.SaveDataGame();
        }
    }

    public int MaxStar
    {
        get
        {
            return _maxstar;
        }
        set
        {
            _maxstar = value;
        }
    }

    public int CurrStarChicken
    {
        get
        {
            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), (resultInventory) =>
                {
                    CPlayerPrefs.SetInt("chicken_star", resultInventory.VirtualCurrency["CK"]);
                }, null);
            }
            _currStarChicken = CPlayerPrefs.GetInt("chicken_star", 0);
            return _currStarChicken;
        }
    }

    public void RewardStar()
    {
        if (_currStarChicken >= _maxstar)
            return;
        CurrencyController.CreditBalance(_amount);
        Sound.instance.Play(Sound.Collects.CoinCollect);
        _currStarChicken += _amount;
        CPlayerPrefs.SetInt("chicken_star", _currStarChicken);
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            AddUserVirtualCurrencyRequest request = new AddUserVirtualCurrencyRequest();
            request.VirtualCurrency = "CK";
            request.Amount = _currStarChicken;
            PlayFabClientAPI.AddUserVirtualCurrency(request, null, null);
        }
    }
}
