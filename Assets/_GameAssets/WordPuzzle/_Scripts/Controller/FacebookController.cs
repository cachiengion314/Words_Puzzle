﻿using Facebook.MiniJSON;
using Facebook.Unity;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class FacebookController : MonoBehaviour
{
    public static FacebookController instance;

    [HideInInspector] public bool newLevel = false;

    public PlayFab.ClientModels.LoginResult result;
    public User user;
    public int bestScore;
    public int bonusNewLevel;
    public List<string> newWordOpenInLevel;
    public List<string> existWord;

    [SerializeField] private List<string> _keysStatic;
    [SerializeField] private GameData _gameData;

    private string _jsondata;
    private List<PlayerLeaderboardEntry> _players;
    public List<PlayerLeaderboardEntry> Players
    {
        get
        {
            return _players;
        }
    }

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        GetUserData();
    }

    void Update()
    {
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            CurrencyController.UpdateBalanceAndHintFree();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            //LogController.Debug("NumWords: " + WordRegion.instance.NumWords);
            //LogController.Debug("Best score: " + FacebookController.instance.bestScore);
            //LogController.Debug("HoneyPoints: " + FacebookController.instance.HoneyPoints);

            //FlagTabController.instance.GetAllWordsList();
            //foreach (var item in WordRegion.instance.listWordInLevel)
            //{
            //    LogController.Debug(item);
            //}
            //FlagTabController.instance.AddToUnlockedWordDictionary("Albania");
            //FlagTabController.instance.SaveUnlockedWordData();
            //foreach (var pair in user.unlockedFlagWords)
            //{
            //    LogController.Debug(pair.Value);
            //}
            //NotificationController.instance.GetCurrentLevelAllLetter();
            foreach (var item in user.unlockedFlagWords)
            {
                LogController.Debug(item.Value);
            }
        }
    }

    [HideInInspector]
    public double HoneyPoints
    {
        get
        {
            if (user.honeyPoint <= 0)
                user.honeyPoint = 0;
            return user.honeyPoint;
        }
        set
        {
            user.honeyPoint = value;
        }
    }
    public Action onChangedHoneyPoints;
    public void UpdateStaticsUser()
    {
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            int numLevels = Superpow.Utils.GetNumLevels(Int32.Parse(user.unlockedWorld), Int32.Parse(user.unlockedSubWorld));
            var request = new UpdatePlayerStatisticsRequest();
            var staticUpdate = new List<StatisticUpdate>();
            //var currlevel = Int32.Parse(user.unlockedLevel) + numLevels * (Int32.Parse(user.unlockedSubWorld) + _gameData.words.Count * Int32.Parse(user.unlockedWorld));
            foreach (var item in _keysStatic)
            {
                staticUpdate.Add(new StatisticUpdate
                {
                    StatisticName = item,
                    Value = bestScore + (int)HoneyPoints
                });
            }
            PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
            {
                Statistics = staticUpdate
            }, null, null);
        }
    }

    public List<string> KeysStatic
    {
        get
        {
            return _keysStatic;
        }
    }

    public void Logout()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        FB.LogOut();
    }

    public void SaveDataGame(Action<UpdateUserDataResult> callback = null)
    {
        //if (!Prefs.IsSaveLevelProgress())
        //    return;
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            user.id = result.PlayFabId;
            user.name = result.InfoResultPayload.AccountInfo.FacebookInfo.FullName;
            _jsondata = SaveDataLocal();
            var dicUserData = new Dictionary<string, string>();
            dicUserData.Add("UserData", _jsondata);
            UpdateUserData(dicUserData, (userResult) =>
            {
                GetDataLeaderBoard(() =>
                {
                    callback?.Invoke(userResult);
                });
            });
        }
        else
        {
            _jsondata = SaveDataLocal();
        }
    }

    public void UpdateUserData(Dictionary<string, string> keyValues, Action<UpdateUserDataResult> callback = null)
    {
        UpdateTitleDisplayName();
        UpdateStaticsUser();
        UpdateDataUser(keyValues, callback);
    }
    public void GetUserData(Action callback = null)
    {
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            GetDataFromPlayfabs(callback);
        }
        else
        {
            ParserJsonData(CPlayerPrefs.GetString("user", JsonConvert.SerializeObject(UserDefault())));
            SetValueUser();
        }
    }
    public void GetLeaderboard(string statisticName, Action<GetLeaderboardResult> callback = null)
    {
        PlayFabClientAPI.GetFriendLeaderboard(new GetFriendLeaderboardRequest
        {
            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowAvatarUrl = true,
                ShowDisplayName = true,
            },
            StatisticName = statisticName,
            MaxResultsCount = 10
        }, (resultLeaderboard) => callback(resultLeaderboard), (result) =>
        {
            Debug.Log("Get Leaderboard Error !!");
        });
    }

    #region Update PlayFab Client API
    private void UpdateTitleDisplayName()
    {
        FB.API("/me/picture?redirect=false", HttpMethod.GET, ProfilePhotoCallback);
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = result.InfoResultPayload.AccountInfo.FacebookInfo.FullName.Length < 25 ? result.InfoResultPayload.AccountInfo.FacebookInfo.FullName : result.InfoResultPayload.AccountInfo.FacebookInfo.FullName.Substring(0, 24),
        }, null, null);
    }

    private void ProfilePhotoCallback(IGraphResult result)
    {
        if (String.IsNullOrEmpty(result.Error) && !result.Cancelled)
        {
            IDictionary data = result.ResultDictionary["data"] as IDictionary;
            string photoURL = data["url"] as String;
            PlayFabClientAPI.UpdateAvatarUrl(new UpdateAvatarUrlRequest
            {
                ImageUrl = photoURL
            }, null, null);
        }
    }

    private void UpdateDataUser(Dictionary<string, string> keyValues, Action<UpdateUserDataResult> callback = null)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = keyValues
        }, (userResult) => { callback?.Invoke(userResult); }, null);
    }
    #endregion

    private string SaveDataLocal()
    {
        if (user.levelProgress.Length == 0)
            user.levelProgress = new string[] { "0" };
        if (user.answerProgress.Length == 0)
            user.answerProgress = new string[] { "0" };
        var jsonData = JsonConvert.SerializeObject(user);
        CPlayerPrefs.SetString("user", jsonData);
        return jsonData;
    }

    private User UserDefault()
    {
        User userDefault = new User();
        userDefault.id = "";
        userDefault.deviceId = "";
        userDefault.name = "";
        userDefault.email = "";
        userDefault.wordPassed = "";
        userDefault.maxbank = 1440;
        userDefault.currBank = 720;
        userDefault.remainBank = 0;
        userDefault.honeyPoint = 0;
        userDefault.unlockedSubWorld = "0";
        userDefault.unlockedLevel = "0";
        userDefault.unlockedWorld = "0";
        userDefault.levelProgress = new string[] { "0" };
        userDefault.answerProgress = new string[] { "0" };
        userDefault.unlockedFlagWords = new Dictionary<string, string>();
        return userDefault;
    }
    private void GetFriendList()
    {
        PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest
        {
            ProfileConstraints = new PlayerProfileViewConstraints { ShowCreated = true },
            IncludeFacebookFriends = true,
        }, (resultFr) =>
        {
            Debug.Log("Friend: " + resultFr.Friends.Count);
            //Debug.Log(resultFr.Friends[0].TitleDisplayName);
        }, null);
    }

    public void GetDataLeaderBoard(Action callback = null)
    {
        GetLeaderboard(KeysStatic[0], (result) =>
        {
            _players = new List<PlayerLeaderboardEntry>();
            foreach (var player in result.Leaderboard)
            {
                _players.Add(player);
            }
            callback?.Invoke();
        });
    }

    private void GetDataFromPlayfabs(Action callback = null)
    {
        GetFriendList();
        GetDataLeaderBoard();
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest
        {
            StatisticNames = KeysStatic
        }, (resultStatistics) =>
        {
            foreach (var rs in resultStatistics.Statistics)
            {
                if (rs.StatisticName == KeysStatic[0])
                {
                    bestScore = rs.Value;
                    break;
                }
            }
        }, null);
        PlayFabClientAPI.GetUserData(new GetUserDataRequest
        {
            PlayFabId = result.PlayFabId,
        }, (result) =>
        {
            foreach (var data in result.Data)
            {
                if (data.Key.ToString() == "UserData")
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
        us.deviceId = jsonData.deviceId;
        us.name = jsonData.name;
        us.email = jsonData.email;
        us.wordPassed = jsonData.wordPassed;
        us.maxbank = jsonData.maxbank;
        us.currBank = jsonData.currBank;
        us.remainBank = jsonData.remainBank;
        us.honeyPoint = jsonData.honeyPoint;
        us.unlockedSubWorld = jsonData.unlockedSubWorld;
        us.unlockedLevel = jsonData.unlockedLevel;
        us.unlockedWorld = jsonData.unlockedWorld;
        us.levelProgress = jsonData.levelProgress;
        us.answerProgress = jsonData.answerProgress;
        us.unlockedFlagWords = jsonData.unlockedFlagWords;
        user = us;
    }
    private void SetValueUser()
    {
        Prefs.unlockedLevel = Int32.Parse(user.unlockedLevel);
        Prefs.unlockedWorld = Int32.Parse(user.unlockedWorld);
        Prefs.unlockedSubWorld = Int32.Parse(user.unlockedSubWorld);
        Prefs.levelProgress = user.levelProgress;
        Prefs.answersProgress = user.answerProgress;
    }
}

[System.Serializable]
public struct User
{
    public string id;
    public string deviceId;
    public string name;
    public string email;
    public string wordPassed;
    public double maxbank;
    public double currBank;
    public double remainBank;
    public double honeyPoint;
    public string unlockedWorld;
    public string unlockedSubWorld;
    public string unlockedLevel;
    public string[] levelProgress;
    public string[] answerProgress;

    public Dictionary<string, string> unlockedFlagWords;
}