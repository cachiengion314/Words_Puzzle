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

    private int _currStarChicken;

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
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
            _currStarChicken = CPlayerPrefs.GetInt("chicken_star", 720);
            return _currStarChicken;
        }
        set
        {
            _currStarChicken = value;
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

    public void AddtoBank()
    {
        Sound.instance.Play(Sound.Collects.CoinCollect);
        CurrStarChicken += _amount;
    }

    public void CollectBank(int value)
    {
        Sound.instance.Play(Sound.Collects.CoinCollect);
        CurrStarChicken -= value;
    }
}
