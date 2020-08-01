using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AndroidUniqueId : MonoBehaviour
{
    public static AndroidUniqueId instance;
    public static DatabaseReference deviceIdData;

    private Dictionary<string, object> playerInfoDic;

    private bool isLoaded;
    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneWasLoaded;
    }

    private void OnSceneWasLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene.buildIndex == 0) return;
        if (!isLoaded)
        {
            GetDeviceIdInfo();
        }
        else
        {
            UnsubcribeSceneLoaded();
        }
    }
    private void UnsubcribeSceneLoaded()
    {
        Destroy(gameObject);
        SceneManager.sceneLoaded -= OnSceneWasLoaded;
    }
    public void GetDeviceIdInfo()
    {
        FacebookController.instance.user.deviceId = SystemInfo.deviceUniqueIdentifier;

        deviceIdData = FirebaseDatabase.DefaultInstance.GetReference("device_id");
        Dictionary<string, object> idPlayerDataDic = new Dictionary<string, object>
        {
            ["/" + FacebookController.instance.user.deviceId] = CreatePlayerDictionaryInfo(FacebookController.instance.user)
        };

        deviceIdData.UpdateChildrenAsync(idPlayerDataDic);
        isLoaded = true;
    }
    public Dictionary<string, object> CreatePlayerDictionaryInfo(User user)
    {
        playerInfoDic = new Dictionary<string, object>
        {
            ["id"] = user.id,
            ["deviceId"] = user.deviceId,
            ["name"] = user.name,
            ["email"] = user.email,
            ["word_passed"] = user.wordPassed,
            ["max_bank"] = user.maxbank,
            ["current_bank"] = user.currBank,
            ["remain_bank"] = user.remainBank,
            ["honey_point"] = user.honeyPoint,
            ["unlocked_world"] = user.unlockedWorld,
            ["unlocked_subWorld"] = user.unlockedSubWorld,
            ["unlocked_level"] = user.unlockedLevel,
            ["leve_progress"] = user.levelProgress,
            ["answer_progress"] = user.answerProgress,
            ["unlocked_flag_words"] = user.unlockedFlagWords
        };
        return playerInfoDic;
    }
}
