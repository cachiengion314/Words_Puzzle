using DG.Tweening.Plugins.Core.PathCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlayFab.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[Serializable]
public class FlagItem
{
    public int flagSmallImageIndex;
    public int flagBigImageIndex;
    public string flagName;
    public string flagUnlockWord;
    public string flagPopulation;
}
public class FlagTabController : MonoBehaviour
{
    public static FlagTabController instance;

    public Dictionary<string, object> countryInfo = new Dictionary<string, object>();
    public List<FlagItem> flagItemList;
    public List<string> allWordsList = new List<string>();
    public Sprite[] smallFlags;
    public Sprite[] bigFlags;
    public GameData gameData;

    [HideInInspector] public readonly int priceToUnlockFlag = 500;

    // Hashset to speedup searching
    public HashSet<string> flagItemWordHashset = new HashSet<string>();
    public HashSet<string> unlockedWordHashset = new HashSet<string>();

    private readonly string COUNTRY_NAME = "name";
    private readonly string SUB_REGION = "subregion";
    private readonly string CAPITAL = "capital";
    private readonly string AREA = "area";
    private readonly string POPULATION = "population";

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
        LogController.Debug("wordIsChecking: " + wordIsChecking);
        string checkWord = wordIsChecking.ToLower();
        if (wordIsChecking == null || wordIsChecking == string.Empty || !flagItemWordHashset.Contains(checkWord)) return;

        //string flagName = FindCountryNameWithUnlockedWord(wordIsChecking);
        //Toast.instance.ShowMessage(flagName.ToUpper() + " Country found");
        AddToUnlockedWordDictionary(checkWord);
        SaveUnlockedWordData();
    }
    public void GetAllWordsList()
    {
        string allWordsString = null;
        for (int i = 0; i < gameData.words.Count; i++)
        {
            for (int ii = 0; ii < gameData.words[i].subWords.Count; ii++)
            {
                for (int iii = 0; iii < gameData.words[i].subWords[ii].gameLevels.Count; iii++)
                {
                    allWordsString += "|" + gameData.words[i].subWords[ii].gameLevels[iii].answers;
                }
            }
        }
        allWordsList.Clear();
        List<string> tempAllWordsList;
        tempAllWordsList = allWordsString.Split(new string[1] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        tempAllWordsList.Sort();
        tempAllWordsList = tempAllWordsList.Distinct().ToList();

        allWordsList.AddRange(tempAllWordsList);
    }
    [HideInInspector] public bool isGetCountryRequestDone;
    [HideInInspector] public bool haveInternet;

    public IEnumerator GetCountryInfo(string countryName, string countryPopulation = "0")
    {
        isGetCountryRequestDone = false;
        using (UnityWebRequest request = UnityWebRequest.Get("https://restcountries.eu/rest/v2/name/" + countryName))
        {
            yield return request.SendWebRequest();
            while (!request.isDone)
                yield return null;
            isGetCountryRequestDone = true;

            if (haveInternet && !countryName.Equals("Wales", StringComparison.OrdinalIgnoreCase))
            {
                byte[] result = request.downloadHandler.data;
                string countryJSON = System.Text.Encoding.Default.GetString(result);
                JArray jsonArr = JArray.Parse(countryJSON);
                Dictionary<string, object> tempDic = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonArr[0].ToString());
                countryInfo.Clear();
                countryInfo = tempDic;
                if (countryName.Equals("United Kingdom", StringComparison.OrdinalIgnoreCase))
                {
                    countryInfo[COUNTRY_NAME] = "United Kingdom";
                }                
                countryInfo[POPULATION] = countryPopulation;
            }
            else if (haveInternet && countryName.Equals("Wales", StringComparison.OrdinalIgnoreCase))
            {
                countryInfo.Clear();
                countryInfo = GetWalesCountryInfo();
                countryInfo[POPULATION] = countryPopulation;
            }          
        }
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
    private Dictionary<string, object> GetWalesCountryInfo()
    {
        Dictionary<string, object> tempDic = new Dictionary<string, object>
        {
            [COUNTRY_NAME] = "Wales",
            [SUB_REGION] = "Northern Europe",
            [CAPITAL] = "Cardiff",
            [AREA] = "20779"
        };
        return tempDic;
    }
    private string FindCountryNameWithUnlockedWord(string unlockedWord)
    {
        FlagItem flagItem = flagItemList.Find(flag => flag.flagUnlockWord.Equals(unlockedWord, StringComparison.OrdinalIgnoreCase));
        if (flagItem != null)
        {
            return flagItem.flagName;
        }
        else
        {
            return "nation";
        }
    }
}
