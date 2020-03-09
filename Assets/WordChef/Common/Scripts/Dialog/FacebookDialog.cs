using Facebook.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FacebookDialog : Dialog
{
    User _user;

    [SerializeField] private Button _btnFbLogin;
    [SerializeField] private Button _btnFbLogout;
    [SerializeField] private TextMeshProUGUI _txtNameUser;

    void Start()
    {
        InitUserFB();
    }

    private void InitUserFB()
    {
        _user = FacebookController.instance.user;
        CheckLogin();
    }

    private void ClearUser()
    {
        FacebookController.instance.user.id = "";
        FacebookController.instance.user.name = "GUEST";
        FacebookController.instance.user.email = "";
        _user = FacebookController.instance.user;
    }

    #region Login Facebook

    public void Logout()
    {
        FB.LogOut();
        ClearUser();
        CheckLogin();
    }

    public void Login()
    {
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }

    public void InviteFriends()
    {
        FB.AppRequest("Play with me !", null, null, null, null, null, "Word Puzzle", (result) =>
        {

        });
    }

    void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions)
            {
                Debug.Log(perm);
            }
            FB.API("/me?fields=id,first_name,email", HttpMethod.GET, ShowInfoUser);
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    void ShowInfoUser(IGraphResult result)
    {
        if (result.Error == null)
        {
            _user.id = "" + result.ResultDictionary["id"];
            _user.name = "" + result.ResultDictionary["first_name"];
            _user.email = "" + result.ResultDictionary["email"];
            _user.unlockedLevel = Prefs.unlockedLevel.ToString();
            _user.unlockedWorld = Prefs.unlockedWorld.ToString();
            _user.unlockedSubWorld = Prefs.unlockedSubWorld.ToString();
            _user.levelProgress = Prefs.levelProgress;
            FacebookController.instance.user = _user;
            var jsonData = JsonUtility.ToJson(_user);
            CPlayerPrefs.SetString("user", jsonData);
            ShowTextNameUser();
            ShowBtnLogin(false);
        }
        else
        {
            Debug.Log("Error GetInfo !");
        }
    }
    #endregion

    private void ShowTextNameUser()
    {
        _txtNameUser.text = _user.name;
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
            ShowBtnLogin(false);
        }
        else
        {
            ShowBtnLogin(true);
        }
        ShowTextNameUser();
    }
}
