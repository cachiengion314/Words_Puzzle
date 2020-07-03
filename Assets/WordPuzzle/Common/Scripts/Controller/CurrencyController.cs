using UnityEngine;
using System.Collections;
using System;
using PlayFab;
using PlayFab.ClientModels;

public class CurrencyController
{
    public const string CURRENCY = "ruby";
    public const int DEFAULT_CURRENCY = 200;
    public static Action<bool> onBalanceChanged;
    public static Action<int> onBallanceIncreased;

    public const int DEFAULT_HINT_FREE = 0;
    public static Action onHintFreeChanged;
    public static Action<int> onHintFreeIncreased;

    public const int DEFAULT_MULTIPLE_HINT_FREE = 0;
    public static Action onMultipleHintFreeChanged;
    public static Action<int> onMultipleHintFreeIncreased;

    public const int DEFAULT_SELECTED_HINT_FREE = 0;
    public static Action onSelectedHintFreeChanged;
    public static Action<int> onSelectedHintFreeIncreased;

    public static void UpdateBalanceAndHintFree()
    {
        GetUserInventoryRequest requestInventory = new GetUserInventoryRequest();
        PlayFabClientAPI.GetUserInventory(requestInventory, (resultInventory) =>
        {
            SetHintFree(resultInventory.VirtualCurrency["HF"]);
            SetBalance(resultInventory.VirtualCurrency["CB"]);
            onHintFreeChanged?.Invoke();
            onBalanceChanged?.Invoke(false);
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
                if (onBalanceChanged != null) onBalanceChanged(true);
                if (onBallanceIncreased != null) onBallanceIncreased(value);
            }, null);
        }
        else
        {
            SetBalance(current + value);
            if (onBalanceChanged != null) onBalanceChanged(true);
            if (onBallanceIncreased != null) onBallanceIncreased(value);
        }

        Firebase.Analytics.FirebaseAnalytics.LogEvent(
          Firebase.Analytics.FirebaseAnalytics.EventEarnVirtualCurrency,
          new Firebase.Analytics.Parameter[] {
            new Firebase.Analytics.Parameter(
              Firebase.Analytics.FirebaseAnalytics.ParameterValue, value),
            new Firebase.Analytics.Parameter(
              Firebase.Analytics.FirebaseAnalytics.ParameterVirtualCurrencyName, "StarCredit"),
          }
        );
    }

    public static bool DebitBalance(int value)
    {
        int current = GetBalance();
        if (current < value)
        {
            return false;
        }

        //FacebookController.instance.user.maxbank -= value;
        //FacebookController.instance.SaveDataGame();
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            SubtractUserVirtualCurrencyRequest request = new SubtractUserVirtualCurrencyRequest();
            request.VirtualCurrency = "CB";
            request.Amount = value;
            PlayFabClientAPI.SubtractUserVirtualCurrency(request, (result) =>
            {
                SetBalance(result.Balance);
                if (onBalanceChanged != null) onBalanceChanged(true);
            }, null);
        }
        else
        {
            SetBalance(current - value);
            if (onBalanceChanged != null) onBalanceChanged(true);
        }

        Firebase.Analytics.FirebaseAnalytics.LogEvent(
          Firebase.Analytics.FirebaseAnalytics.EventSpendVirtualCurrency,
          new Firebase.Analytics.Parameter[] {
            new Firebase.Analytics.Parameter(
              Firebase.Analytics.FirebaseAnalytics.ParameterItemName, "Star"),
            new Firebase.Analytics.Parameter(
              Firebase.Analytics.FirebaseAnalytics.ParameterValue, value),
            new Firebase.Analytics.Parameter(
              Firebase.Analytics.FirebaseAnalytics.ParameterVirtualCurrencyName, "StarDebit"),
          }
        );
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
                if (onHintFreeChanged != null) onHintFreeChanged();
                if (onHintFreeIncreased != null) onHintFreeIncreased(value);
            }, null);
        }
        else
        {
            SetHintFree(current + value);
            if (onHintFreeChanged != null) onHintFreeChanged();
            if (onHintFreeIncreased != null) onHintFreeIncreased(value);
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
                if (onHintFreeChanged != null) onHintFreeChanged();
            }, null);
        }
        else
        {
            SetHintFree(current - value);
            if (onHintFreeChanged != null) onHintFreeChanged();
        }
        return true;
    }
    #endregion

    #region Multiple Hints
    public static int GetMultipleHintFree()
    {
        int numHint = DEFAULT_MULTIPLE_HINT_FREE;
        if (CPlayerPrefs.HasKey(PrefKeys.MULTIPLE_HINT_FREE))
            numHint = CPlayerPrefs.GetInt(PrefKeys.MULTIPLE_HINT_FREE);
        return numHint;
    }

    public static void SetMultipleHintFree(int value)
    {
        CPlayerPrefs.SetInt(PrefKeys.MULTIPLE_HINT_FREE, value);
        CPlayerPrefs.Save();
    }

    public static void CreditMultipleHintFree(int value)
    {
        int current = GetMultipleHintFree();
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            AddUserVirtualCurrencyRequest request = new AddUserVirtualCurrencyRequest();
            request.VirtualCurrency = "MH";
            request.Amount = value;
            PlayFabClientAPI.AddUserVirtualCurrency(request, (result) =>
            {
                SetMultipleHintFree(result.Balance);
                if (onMultipleHintFreeChanged != null) onMultipleHintFreeChanged();
                if (onMultipleHintFreeIncreased != null) onMultipleHintFreeIncreased(value);
            }, null);
        }
        else
        {
            SetMultipleHintFree(current + value);
            if (onMultipleHintFreeChanged != null) onMultipleHintFreeChanged();
            if (onMultipleHintFreeIncreased != null) onMultipleHintFreeIncreased(value);
        }
    }

    public static bool DebitMultipleHintFree(int value)
    {
        int current = GetMultipleHintFree();
        if (current < value)
        {
            return false;
        }


        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            SubtractUserVirtualCurrencyRequest request = new SubtractUserVirtualCurrencyRequest();
            request.VirtualCurrency = "MH";
            request.Amount = value;
            PlayFabClientAPI.SubtractUserVirtualCurrency(request, (result) =>
            {
                SetMultipleHintFree(result.Balance);
                if (onMultipleHintFreeChanged != null) onMultipleHintFreeChanged();
            }, null);
        }
        else
        {
            SetMultipleHintFree(current - value);
            if (onMultipleHintFreeChanged != null) onMultipleHintFreeChanged();
        }
        return true;
    }
    #endregion

    #region Selected Hints
    public static int GetSelectedHintFree()
    {
        int numHint = DEFAULT_SELECTED_HINT_FREE;
        if (CPlayerPrefs.HasKey(PrefKeys.SELECTED_HINT_FREE))
            numHint = CPlayerPrefs.GetInt(PrefKeys.SELECTED_HINT_FREE);
        return numHint;
    }

    public static void SetSelectedHintFree(int value)
    {
        CPlayerPrefs.SetInt(PrefKeys.SELECTED_HINT_FREE, value);
        CPlayerPrefs.Save();
    }

    public static void CreditSelectedHintFree(int value)
    {
        int current = GetSelectedHintFree();
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            AddUserVirtualCurrencyRequest request = new AddUserVirtualCurrencyRequest();
            request.VirtualCurrency = "SH";
            request.Amount = value;
            PlayFabClientAPI.AddUserVirtualCurrency(request, (result) =>
            {
                SetSelectedHintFree(result.Balance);
                if (onSelectedHintFreeChanged != null) onSelectedHintFreeChanged();
                if (onSelectedHintFreeIncreased != null) onSelectedHintFreeIncreased(value);
            }, null);
        }
        else
        {
            SetSelectedHintFree(current + value);
            if (onSelectedHintFreeChanged != null) onSelectedHintFreeChanged();
            if (onSelectedHintFreeIncreased != null) onSelectedHintFreeIncreased(value);
        }
    }

    public static bool DebitSelectedHintFree(int value)
    {
        int current = GetSelectedHintFree();
        if (current < value)
        {
            return false;
        }


        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            SubtractUserVirtualCurrencyRequest request = new SubtractUserVirtualCurrencyRequest();
            request.VirtualCurrency = "SH";
            request.Amount = value;
            PlayFabClientAPI.SubtractUserVirtualCurrency(request, (result) =>
            {
                SetSelectedHintFree(result.Balance);
                if (onSelectedHintFreeChanged != null) onSelectedHintFreeChanged();
            }, null);
        }
        else
        {
            SetSelectedHintFree(current - value);
            if (onSelectedHintFreeChanged != null) onSelectedHintFreeChanged();
        }
        return true;
    }
    #endregion
}