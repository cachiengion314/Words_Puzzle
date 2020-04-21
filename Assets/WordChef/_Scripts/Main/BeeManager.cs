using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeManager : MonoBehaviour
{
    public Action onBeeChanged;
    private int _currBee;
    public static BeeManager instance;

    public int CurrBee
    {
        get
        {
            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), (resultInventory) =>
                {
                    CPlayerPrefs.SetInt("amount_bee", resultInventory.VirtualCurrency["BE"]);
                }, null);
            }
            _currBee = CPlayerPrefs.GetInt("amount_bee", 0);
            return _currBee;
        }
    }
    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void Load(int amount)
    {
        _currBee = amount;
    }

    public void SetAmountBee(int number)
    {
        _currBee += number;
        if (_currBee <= 0)
            _currBee = 0;
        CPlayerPrefs.SetInt("amount_bee", _currBee);
        onBeeChanged?.Invoke();
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            AddUserVirtualCurrencyRequest request = new AddUserVirtualCurrencyRequest();
            request.VirtualCurrency = "BE";
            request.Amount = _currBee;
            PlayFabClientAPI.AddUserVirtualCurrency(request, null, null);
        }
    }
}
