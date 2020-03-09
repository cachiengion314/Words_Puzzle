using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacebookController : MonoBehaviour
{
    public static FacebookController instance;

    public User user;
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
            user = UserData();
        }
        else
        {
            user.id = "";
            user.name = "GUEST";
            user.email = "";
        }
    }

    private User UserData()
    {
        User us = new User();
        var jsonData = JsonUtility.FromJson<User>(CPlayerPrefs.GetString("user"));
        us.id = jsonData.id;
        us.name = jsonData.name;
        us.email = jsonData.email;
        us.unlockedSubWorld = jsonData.unlockedSubWorld;
        us.unlockedLevel = jsonData.unlockedLevel;
        us.unlockedWorld = jsonData.unlockedWorld;
        us.levelProgress = jsonData.levelProgress;
        return us;
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
