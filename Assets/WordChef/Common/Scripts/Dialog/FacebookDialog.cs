using Facebook.Unity;
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
    [SerializeField] private LoadingController _animLoading;
    [SerializeField] private RankingController _rankingPfb;
    [SerializeField] private Transform _rootRanking;
    [SerializeField] private GameObject _leaderBoard;
    [SerializeField] private GameObject _mainUI;

    void Start()
    {
        InitUserFB();
    }

    private void InitUserFB()
    {
        _animLoading.gameObject.SetActive(false);
        _leaderBoard.transform.localScale = Vector3.zero;
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
        PlayFabClientAPI.ForgetAllCredentials();
        FB.LogOut();
        CheckLogin();
    }

    public void Login()
    {
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            FacebookController.instance.GetUserData(() =>
            {
                ShowTextNameUser();
                ShowBtnLogin(false);
                ShowLeaderboard();
            });
        }
        else
        {
            _txtNameUser.text = "Loading...";
            _btnFbLogin.gameObject.SetActive(false);
            if (FB.IsInitialized)
                OnFacebookInitialized();
            else
                FB.Init(OnFacebookInitialized);
        }
    }

    #region Facebook Init
    void OnFacebookInitialized()
    {
        _animLoading.gameObject.SetActive(true);
        var perms = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(perms, OnFacebookLoggedIn);
    }
    void OnFacebookLoggedIn(ILoginResult result)
    {
        if (result == null || string.IsNullOrEmpty(result.Error))
        {
            _animLoading.PlayLoading();
            PlayFabClientAPI.LoginWithFacebook(new LoginWithFacebookRequest
            {
                TitleId = PlayFabSettings.TitleId,
                CreateAccount = true,
                //FacebookInstantGamesSignature = result.AccessToken.UserId,
                AccessToken = result.AccessToken.TokenString,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true,
                    GetUserAccountInfo = true,
                    GetTitleData = true,
                    GetPlayerStatistics = true,
                    GetUserVirtualCurrency = true
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
            FacebookController.instance.SaveDataGame((userResult) =>
            {
                ShowTextNameUser();
                ShowBtnLogin(false);
                ShowLeaderboard();
                _animLoading.gameObject.SetActive(false);
                
            });
        }
        else
        {
            FacebookController.instance.GetUserData(() =>
            {
                ShowTextNameUser();
                ShowBtnLogin(false);
                ShowLeaderboard();
                _animLoading.gameObject.SetActive(false);

            });
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
        ShowBtnLogin(true);
        _animLoading.gameObject.SetActive(false);
        _leaderBoard.transform.localScale = Vector3.zero;
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
        _mainUI.transform.localScale = Vector3.zero;
        _leaderBoard.SetActive(true);
        _rootRanking.gameObject.SetActive(true);
        var vertical = _rootRanking.GetComponent<VerticalLayoutGroup>();
        vertical.enabled = false;
        for (int i = 0; i < _rootRanking.childCount; i++)
        {
            Destroy(_rootRanking.GetChild(i).gameObject);
        }
        _notifyLogin.gameObject.SetActive(false);
        FacebookController.instance.GetLeaderboard("DailyRanking", (result) =>
        {
            foreach (var player in result.Leaderboard)
            {
                var ranking = Instantiate(_rankingPfb, _rootRanking);
                ranking.UpdateRankingPlayer(player.DisplayName, player.StatValue, player.Profile.AvatarUrl);
            }
            TweenControl.GetInstance().ScaleFromZero(_leaderBoard, 0.3f, () =>
            {
                vertical.enabled = true;
            });
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
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            ShowLeaderboard();
            ShowBtnLogin(false);
        }
        else
        {
            _mainUI.transform.localScale = Vector3.one;
            _rootRanking.gameObject.SetActive(false);
            _notifyLogin.gameObject.SetActive(true);
            ClearUser();
            ShowBtnLogin(true);
        }
        ShowTextNameUser();
    }
}
