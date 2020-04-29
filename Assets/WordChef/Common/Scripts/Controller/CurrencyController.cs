using UnityEngine;
using System.Collections;
using System;
using PlayFab;
using PlayFab.ClientModels;

public class CurrencyController
{
    public const string CURRENCY = "ruby";
    public const int DEFAULT_CURRENCY = 200;
    public static Action onBalanceChanged;
    public static Action<int> onBallanceIncreased;

    public const int DEFAULT_HINT_FREE = 0;
    public static Action onHintFreeChanged;
    public static Action<int> onHintFreeIncreased;

    public static void UpdateBalanceAndHintFree()
    {
        GetUserInventoryRequest requestInventory = new GetUserInventoryRequest();
        PlayFabClientAPI.GetUserInventory(requestInventory, (resultInventory) =>
        {
            SetHintFree(resultInventory.VirtualCurrency["HF"]);
            SetBalance(resultInventory.VirtualCurrency["CB"]);
            onHintFreeChanged?.Invoke();
            onBalanceChanged?.Invoke();
        }, null);
    }

    #region CURRENCY BALANCE
    public static int GetBalance()
    {
        int numCurrency = DEFAULT_CURRENCY;
        if (CPlayerPrefs.HasKey(PrefKeys.CURRENCY))
            numCurrency = CPlayerPrefs.GetInt(PrefKeys.CURRENCY);
        return numCurrency;
    }

    public static void SetBalance(int value)
    {
        CPlayerPrefs.SetInt(PrefKeys.CURRENCY, value);
        CPlayerPrefs.Save();
    }

    public static void CreditBalance(int value)
    {
        int current = GetBalance();

        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            AddUserVirtualCurrencyRequest request = new AddUserVirtualCurrencyRequest();
            request.VirtualCurrency = "CB";
            request.Amount = value;
            PlayFabClientAPI.AddUserVirtualCurrency(request, (result) =>
            {
                SetBalance(result.Balance);
                if (onBalanceChanged != null) onBalanceChanged();
                if (onBallanceIncreased != null) onBallanceIncreased(value);
            }, null);
        }
        else
        {
            SetBalance(current + value);
            if (onBalanceChanged != null) onBalanceChanged();
            if (onBallanceIncreased != null) onBallanceIncreased(value);
        }
    }

    public static bool DebitBalance(int value)
    {
        int current = GetBalance();
        if (current < value)
        {
            return false;
        }

        FacebookController.instance.user.maxbank -= value;
        FacebookController.instance.SaveDataGame();
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            SubtractUserVirtualCurrencyRequest request = new SubtractUserVirtualCurrencyRequest();
            request.VirtualCurrency = "CB";
            request.Amount = value;
            PlayFabClientAPI.SubtractUserVirtualCurrency(request, (result) =>
            {
                SetBalance(result.Balance);
                if (onBalanceChanged != null) onBalanceChanged();
            }, null);
        }
        else
        {
            SetBalance(current - value);
            if (onBalanceChanged != null) onBalanceChanged();
        }
        return true;
    }
    #endregion

    #region HINT
    public static int GetHintFree()
    {
        int numHint = DEFAULT_HINT_FREE;
        if (CPlayerPrefs.HasKey(PrefKeys.HINT_FREE))
            numHint = CPlayerPrefs.GetInt(PrefKeys.HINT_FREE);
        return numHint;
    }

    public static void SetHintFree(int value)
    {
        CPlayerPrefs.SetInt(PrefKeys.HINT_FREE, value);
        CPlayerPrefs.Save();
    }

    public static void CreditHintFree(int value)
    {
        int current = GetHintFree();
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            AddUserVirtualCurrencyRequest request = new AddUserVirtualCurrencyRequest();
            request.VirtualCurrency = "HF";
            request.Amount = value;
            PlayFabClientAPI.AddUserVirtualCurrency(request, (result) =>
            {
                SetHintFree(result.Balance);
                if (onBalanceChanged != null) onHintFreeChanged();
                if (onBallanceIncreased != null) onHintFreeIncreased(value);
            }, null);
        }
        else
        {
            SetHintFree(current + value);
            if (onBalanceChanged != null) onHintFreeChanged();
            if (onBallanceIncreased != null) onHintFreeIncreased(value);
        }
    }

    public static bool DebitHintFree(int value)
    {
        int current = GetHintFree();
        if (current < value)
        {
            return false;
        }


        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            SubtractUserVirtualCurrencyRequest request = new SubtractUserVirtualCurrencyRequest();
            request.VirtualCurrency = "HF";
            request.Amount = value;
            PlayFabClientAPI.SubtractUserVirtualCurrency(request, (result) =>
            {
                SetHintFree(result.Balance);
                if (onBalanceChanged != null) onHintFreeChanged();
            }, null);
        }
        else
        {
            SetHintFree(current - value);
            if (onBalanceChanged != null) onHintFreeChanged();
        }
        return true;
    }
    #endregion
}