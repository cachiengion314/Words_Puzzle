using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacebookController : MonoBehaviour
{
    public static FacebookController instance;

    [HideInInspector] public string currUserName;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = this;
        InitFB();
    }

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
            //Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            //Time.timeScale = 1;
        }
    }

    private void CheckLogin()
    {
        if (FB.IsLoggedIn)
        {
            currUserName = CPlayerPrefs.GetString("user_name", GetUserInfo());
        }
        else
        {
            currUserName = "GUEST";
        }
    }

    private string GetUserInfo()
    {
        var username = "";
        var aToken = AccessToken.CurrentAccessToken;
        FB.API("/me?fields=first_name", HttpMethod.GET, (result) =>
        {
            username = "" + result.ResultDictionary["first_name"];
        });
        return username;
    }
}
