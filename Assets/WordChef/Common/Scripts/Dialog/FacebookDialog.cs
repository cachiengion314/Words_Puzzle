﻿using Facebook.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
public class FacebookDialog : Dialog
{
    [SerializeField] private Button _btnFbLogin;
    [SerializeField] private Button _btnFbLogout;
    [SerializeField] private TextMeshProUGUI _txtNameUser;
    [SerializeField] private GameObject _notifyLogin;
    [SerializeField] private RankingController _rankingPfb;
    [SerializeField] private Transform _rootRanking;

    void Start()
    {
        InitUserFB();
    }

    private void InitUserFB()
    {
        CheckLogin();
    }

    private void ClearUser()
    {
        FacebookController.instance.user.id = "";
        FacebookController.instance.user.name = "GUEST";
        FacebookController.instance.user.email = "";
    }

    #region Dialog Control

    public void Logout()
    {
        CheckLogin();
        FB.LogOut();
    }

    public void Login()
    {
        _txtNameUser.text = "Loading...";
        _btnFbLogin.gameObject.SetActive(false);
        FB.Init(OnFacebookInitialized);
    }

    #region Facebook Init
    void OnFacebookInitialized()
    {
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, OnFacebookLoggedIn);
    }
    void OnFacebookLoggedIn(ILoginResult result)
    {
        if (result == null || string.IsNullOrEmpty(result.Error))
        {
            PlayFabClientAPI.LoginWithFacebook(new LoginWithFacebookRequest
            {
                TitleId = PlayFabSettings.TitleId,
                CreateAccount = true,
                AccessToken = result.AccessToken.TokenString,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true,
                    GetUserAccountInfo = true,
                    GetTitleData = true,
                    GetPlayerStatistics = true
                }
            }, OnPlayfabFacebookAuthComplete, OnPlayfabFacebookAuthFailed);
        }
        else
        {
            Debug.Log("Facebook Auth Failed: " + result.Error + "\n" + result.RawResult);
        }
    }

    void OnPlayfabFacebookAuthComplete(PlayFab.ClientModels.LoginResult result)
    {
        FacebookController.instance.result = result;

        if (result.NewlyCreated)
        {
            FacebookController.instance.user.id = result.PlayFabId;
            FacebookController.instance.user.name = result.InfoResultPayload.AccountInfo.FacebookInfo.FullName;
            FacebookController.instance.SaveDataGame((userResult) =>
            {
                FacebookController.instance.user.name = result.InfoResultPayload.PlayerProfile.DisplayName;
                ShowTextNameUser();
                ShowBtnLogin(false);
                ShowLeaderboard();
            });
        }
        else
        {
            FacebookController.instance.GetUserData();
            ShowTextNameUser();
            ShowBtnLogin(false);
            ShowLeaderboard();
        }
        //_user.email = "";
        //_user.unlockedLevel = Prefs.unlockedLevel.ToString();
        //_user.unlockedWorld = Prefs.unlockedWorld.ToString();
        //_user.unlockedSubWorld = Prefs.unlockedSubWorld.ToString();
        //_user.levelProgress = Prefs.levelProgress;
        //FacebookController.instance.user = _user;
    }

    void OnPlayfabFacebookAuthFailed(PlayFabError error)
    {
        Debug.Log("PlayFab Facebook Auth Failed ");
    }
    #endregion


    public void InviteFriends()
    {
        FB.AppRequest("Play with me !", null, null, null, null, null, "Word Puzzle", (result) =>
        {

        });
    }

    #endregion

    private void ShowLeaderboard()
    {
        _rootRanking.gameObject.SetActive(true);
        for (int i = 0; i < _rootRanking.childCount; i++)
        {
            Destroy(_rootRanking.GetChild(i).gameObject);
        }
        _notifyLogin.gameObject.SetActive(false);
        FacebookController.instance.GetLeaderboard("DailyRanking",(result)=> {
            foreach (var player in result.Leaderboard)
            {
                var ranking = Instantiate(_rankingPfb,_rootRanking);
                ranking.UpdateRankingPlayer(player.DisplayName,player.StatValue);
            }
        });
    }

    private void ShowTextNameUser()
    {
        _txtNameUser.text = FacebookController.instance.user.name;
    }

    private void ShowBtnLogin(bool isLogin)
    {
        _btnFbLogin.gameObject.SetActive(isLogin);
        _btnFbLogout.gameObject.SetActive(!isLogin);
    }

    private void CheckLogin()
    {
        if (FB.IsLoggedIn)
        {
            ShowLeaderboard();
            ShowBtnLogin(false);
        }
        else
        {
            _rootRanking.gameObject.SetActive(false);
            _notifyLogin.gameObject.SetActive(true);
            ClearUser();
            ShowBtnLogin(true);
        }
        ShowTextNameUser();
    }
}
