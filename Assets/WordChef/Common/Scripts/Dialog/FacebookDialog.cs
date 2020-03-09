using Facebook.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FacebookDialog : Dialog
{
    private string _userID;
    private string _userName;
    private string _userEmail;

    [SerializeField] private Button _btnFbLogin;
    [SerializeField] private Button _btnFbLogout;
    [SerializeField] private TextMeshProUGUI _txtNameUser;

    void Start()
    {
        InitUserFB();
    }

    private void InitUserFB()
    {
        _userName = FacebookController.instance.currUserName;
        CheckLogin();
    }

    #region Login Facebook

    public void Logout()
    {
        FB.LogOut();
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
            FB.API("/me?fields=first_name", HttpMethod.GET, ShowInfoUser);
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
            _userName = "" + result.ResultDictionary["first_name"];
            CPlayerPrefs.SetString("user_name", _userName);
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
        _txtNameUser.text = _userName;
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
