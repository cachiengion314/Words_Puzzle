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

    void Awake()
    {
        InitFB();
    }

    #region Login Facebook
    private void InitFB()
    {
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
            CheckLogin();

        }
    }
    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
            CheckLogin();
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

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

    void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            FB.API("/me?fields=id,first_name", HttpMethod.GET, ShowInfoUser);
            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions)
            {
                Debug.Log(perm);
            }
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    void ShowInfoUser(IGraphResult result)
    {
        CheckLogin();
        if (result.Error == null)
        {
            _userName = "" + result.ResultDictionary["first_name"];
            ShowTextNameUser();
        }
        else
        {
            FB.API("/me?fields=id,first_name", HttpMethod.GET, ShowInfoUser);
            return;
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
            FB.API("/me?fields=first_name", HttpMethod.GET, ShowInfoUser);
            ShowBtnLogin(false);
        }
        else
        {
            _txtNameUser.text = "GUEST";
            ShowBtnLogin(true);
        }
    }
}
