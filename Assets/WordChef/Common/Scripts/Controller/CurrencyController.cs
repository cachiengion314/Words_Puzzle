using UnityEngine;
using System.Collections;
using System;

public class CurrencyController
{
	public const string CURRENCY = "ruby";
	public const int DEFAULT_CURRENCY = 200;
    public static Action onBalanceChanged;
    public static Action<int> onBallanceIncreased;

    public const int DEFAULT_HINT_FREE = 0;
    public static Action onHintFreeChanged;
    public static Action<int> onHintFreeIncreased;

    #region CURRENCY BALANCE
    public static int GetBalance()
    {
        return CPlayerPrefs.GetInt(PrefKeys.CURRENCY, DEFAULT_CURRENCY);
    }

    public static void SetBalance(int value)
    {
        CPlayerPrefs.SetInt(PrefKeys.CURRENCY, value);
        CPlayerPrefs.Save();
    }

    public static void CreditBalance(int value)
    {
        int current = GetBalance();
        SetBalance(current + value);
        if (onBalanceChanged != null ) onBalanceChanged();
        if (onBallanceIncreased != null) onBallanceIncreased(value);
    }

    public static bool DebitBalance(int value)
    {
        int current = GetBalance();
        if (current < value)
        {
            return false;
        }

        SetBalance(current - value);
        if (onBalanceChanged != null) onBalanceChanged();
        return true;
    }
    #endregion

    #region HINT
    public static int GetHintFree()
    {
        return CPlayerPrefs.GetInt(PrefKeys.HINT_FREE, DEFAULT_HINT_FREE);
    }

    public static void SetHintFree(int value)
    {
        CPlayerPrefs.SetInt(PrefKeys.HINT_FREE, value);
        CPlayerPrefs.Save();
    }

    public static void CreditHintFree(int value)
    {
        int current = GetBalance();
        SetHintFree(current + value);
        if (onBalanceChanged != null) onHintFreeChanged();
        if (onBallanceIncreased != null) onHintFreeIncreased(value);
    }

    public static bool DebitHintFree(int value)
    {
        int current = GetHintFree();
        if (current < value)
        {
            return false;
        }

        SetHintFree(current - value);
        if (onBalanceChanged != null) onHintFreeChanged();
        return true;
    }
    #endregion
}