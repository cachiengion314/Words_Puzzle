using Facebook.Unity;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class FacebookController : MonoBehaviour
{
    public static FacebookController instance;

    public PlayFab.ClientModels.LoginResult result;
    public User user;
    
    [SerializeField] private List<string> _keysStatic;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = this;
        GetUserData();
    }

    public void SaveDataGame(Action<UpdateUserDataResult> callback = null)
    {
        if(user.levelProgress.Length == 0)
            user.levelProgress = new string[] { "0" };
        var jsonData = JsonConvert.SerializeObject(user);
        CPlayerPrefs.SetString("user", jsonData);
        if (FB.IsLoggedIn)
        {
            var dicUserData = new Dictionary<string, string>();
            dicUserData.Add("UserData", jsonData);
            UpdateUserData(dicUserData,(userResult) => { callback?.Invoke(userResult); });
        }
    }

    public void UpdateUserData(Dictionary<string, string> keyValues, Action<UpdateUserDataResult> callback = null)
    {
        UpdateTitleDisplayName();
        UpdateStaticsUser();
        UpdateDataUser(keyValues,callback);
    }
    public void GetUserData(Action callback = null)
    {
        ParserJsonData(CPlayerPrefs.GetString("user", JsonConvert.SerializeObject(UserDefault())));
        if (FB.IsLoggedIn)
        {
            GetDataFromPlayfabs(callback);
        }
        else
        {
            SetValueUser();
        }
    }
    public void GetLeaderboard(string statisticName, Action<GetLeaderboardResult> callback = null)
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            Version = 0,
            StatisticName = statisticName,
            MaxResultsCount = 10
        }, (resultLeaderboard) => { callback?.Invoke(resultLeaderboard); }, null);
    }

    #region Update PlayFab Client API
    private void UpdateTitleDisplayName()
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = result.InfoResultPayload.AccountInfo.FacebookInfo.FullName
        }, null, null);
    }

    private void UpdateStaticsUser()
    {
        var request = new UpdatePlayerStatisticsRequest();
        var staticUpdate = new List<StatisticUpdate>();
        foreach (var item in _keysStatic)
        {
            staticUpdate.Add(new StatisticUpdate
            {
                StatisticName = item,
                Value = Int32.Parse(user.unlockedLevel)
            });
        }
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = staticUpdate
        }, null, null);
    }

    private void UpdateDataUser(Dictionary<string, string> keyValues, Action<UpdateUserDataResult> callback = null)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = keyValues
        }, (userResult) => { callback?.Invoke(userResult); }, null);
    }
    #endregion

    private User UserDefault()
    {
        User userDefault = new User();
        userDefault.id = "";
        userDefault.name = "";
        userDefault.email = "";
        userDefault.unlockedSubWorld = "0";
        userDefault.unlockedLevel = "0";
        userDefault.unlockedWorld = "0";
        userDefault.levelProgress = new string[] { "0" };
        return userDefault;
    }

    private void GetDataFromPlayfabs(Action callback = null)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest
        {
            PlayFabId = result.PlayFabId,
        }, (result) =>
        {
            foreach (var data in result.Data)
            {
                if(data.Key.ToString() == "UserData")
                {
                    ParserJsonData(data.Value.Value);
                }
            }
            SetValueUser();
            callback?.Invoke();
        }, null);
    }

    private void ParserJsonData(string value)
    {
        User us = new User();
        var jsonData = JsonConvert.DeserializeObject<User>(value);
        us.id = jsonData.id;
        us.name = jsonData.name;
        us.email = jsonData.email;
        us.unlockedSubWorld = jsonData.unlockedSubWorld;
        us.unlockedLevel = jsonData.unlockedLevel;
        us.unlockedWorld = jsonData.unlockedWorld;
        us.levelProgress = jsonData.levelProgress;
        user = us;
    }
    private void SetValueUser()
    {
        Prefs.unlockedLevel = Int32.Parse(user.unlockedLevel);
        Prefs.unlockedWorld = Int32.Parse(user.unlockedWorld);
        Prefs.unlockedSubWorld = Int32.Parse(user.unlockedSubWorld);
        Prefs.levelProgress = user.levelProgress;
    }
}

[System.Serializable]
public struct User
{
    public string id;
    public string name;
    public string email;

    public string unlockedWorld;
    public string unlockedSubWorld;
    public string unlockedLevel;
    public string[] levelProgress;
}
