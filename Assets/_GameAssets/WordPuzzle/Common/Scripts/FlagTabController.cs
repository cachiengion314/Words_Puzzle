using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class FlagItem
{
    public int flagSmallImageIndex;
    public int flagBigImageIndex;
    public string flagUnlockWord;
    public string flagName;
    public string subRegion;
    public string capital;
    public string population;
    public string area;
}
public class FlagTabController : MonoBehaviour
{
    public static FlagTabController instance;

    public Dictionary<string, object> countryInfo = new Dictionary<string, object>();
    public List<FlagItem> flagItemList;
    public Sprite[] smallFlags;
    public Sprite[] bigFlags;
    public GameData gameData;

    [HideInInspector] public readonly int priceToUnlockFlag = 500;

    // Hashset to speedup searching
    public HashSet<string> flagItemWordHashset = new HashSet<string>();
    public HashSet<string> unlockedWordHashset = new HashSet<string>();

    private bool isLoaded;
    private void Awake()
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
    private void OnSceneWasLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0) return;
        if (!isLoaded)
        {
            LoadHashsetData();
        }
    }
    public void CheckAndSaveCountrykWord(string wordIsChecking)
    {
        string checkWord = wordIsChecking.ToLower();
        if (wordIsChecking == null || wordIsChecking == string.Empty || !flagItemWordHashset.Contains(checkWord)) return;
        AddToUnlockedWordDictionary(checkWord);
        SaveUnlockedWordData();
    }
    public void AddToUnlockedWordDictionary(string wordIsChecking)
    {
        if (unlockedWordHashset.Add(wordIsChecking.ToLower()))
        {
            FacebookController.instance.user.unlockedFlagWords.Add(wordIsChecking.ToLower(), wordIsChecking.ToLower());
        }
        else
        {
            Debug.Log("Cannot add due to dublicate word");
        }
    }
    public void SaveUnlockedWordData()
    {
        FacebookController.instance.SaveDataGame();
    }
    private void LoadHashsetData()
    {
        foreach (var pair in FacebookController.instance.user.unlockedFlagWords)
        {
            unlockedWordHashset.Add(pair.Value.ToLower());
        }
        foreach (var item in flagItemList)
        {
            flagItemWordHashset.Add(item.flagUnlockWord.ToLower());
        }
        isLoaded = true;
    }
}
