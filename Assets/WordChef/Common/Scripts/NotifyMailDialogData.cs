using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NotifyMailDialogData : MonoBehaviour
{
    public string tittle;
    public string contain;

    public static NotifyMailDialogData instance;
    public List<string> notifyData = new List<string>();
    public bool isGameVersionChange;
    public string currentGameVersion;
    public RectTransform tinyRedNotifyPrefb;
    public RectTransform ingameNotifyPrefb;
    public GameObject scrollViewContent;
    public MailDialog mailDialog;
    public IngameNotify ingameNotify;
    public bool IsShowBefore
    {
        get
        {
            if (PlayerPrefs.GetInt("Is_Show_Before") == 0)
                isShowBefore = false;
            else
                isShowBefore = true;
            return isShowBefore;
        }
        set
        {
            isShowBefore = value;
            if (value)
                PlayerPrefs.SetInt("Is_Show_Before", 1);
            else
                PlayerPrefs.SetInt("Is_Show_Before", 0);
        }
    }
    private bool isShowBefore;
    public static Action MailBttAction;

    private string notifyName = "Notify_";
    private string totalNotifyNumberString = "Total_Notify_Number";
    private int totalNotifyNumber;
    private void Awake()
    {
        instance = this;

#if UNITY_ANDROID && !UNITY_EDITOR
        currentGameVersion = Application.version;
#elif UNITY_EDITOR
        currentGameVersion = PlayerSettings.bundleVersion;
#endif

        if (currentGameVersion != PlayerPrefs.GetString("Current_Game_Version"))
        {
            PlayerPrefs.SetString("Current_Game_Version", currentGameVersion);
            isGameVersionChange = true;
        }
        else
        {
            isGameVersionChange = false;
        }

        totalNotifyNumber = PlayerPrefs.GetInt(totalNotifyNumberString);

        for (int i = 0; i < totalNotifyNumber; i++)
        {
            notifyData.Add(PlayerPrefs.GetString(notifyName + i.ToString()));
        }
    }
    public void CreatePlayerPrefsNotify(string notifyContain)
    {
        int currentNameIndex = notifyData.Count;
        string currentName = notifyName + currentNameIndex.ToString();
        notifyData.Add(notifyContain);
        PlayerPrefs.SetString(currentName, notifyContain);
        PlayerPrefs.SetInt(totalNotifyNumberString, currentNameIndex + 1);
    }
    public bool RemovePlayerPrefsNotifyAt(int notifyIndex)
    {
        if (notifyIndex >= notifyData.Count || notifyIndex < 0) return false;
        else
        {
            notifyData.RemoveAt(notifyIndex);
            ReCreateAllPlayerPrefsNotifyKey();
            return true;
        }
    }
    private void ClearAllPlayerPrefsNotifyKey()
    {
        for (int i = 0; i < totalNotifyNumber; i++)
        {
            PlayerPrefs.DeleteKey(notifyName + i.ToString());
        }
        PlayerPrefs.SetInt(totalNotifyNumberString, 0);
    }
    private void ReCreateAllPlayerPrefsNotifyKey()
    {
        ClearAllPlayerPrefsNotifyKey();
        int firstIndex = 0;
        foreach (var notifyBody in notifyData)
        {
            string currentName = notifyName + firstIndex.ToString();
            PlayerPrefs.SetString(currentName, notifyBody);
            firstIndex++;
        }
        PlayerPrefs.SetInt(totalNotifyNumberString, firstIndex);
    }
}
