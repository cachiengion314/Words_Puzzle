using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class FlagItem
{
    public Sprite flagImage;
    public string flagName;
    public string flagUnlockWord;
    public bool isLocked;
}
public class FlagTabController : MonoBehaviour
{
    public static FlagTabController instance;

    public Dictionary<string, object> countryInfo = new Dictionary<string, object>();
    public List<FlagItem> flagItemList;
    public List<string> allWordsList = new List<string>();
    public GameData gameData;

    public Action foundWordThatMathchFlagAction;
    [HideInInspector] public readonly int priceToUnlockFlag = 3;
    private void Awake()
    {
        instance = this;
    }
    public void CheckUnlockWord()
    {
        for (int i = 0; i < flagItemList.Count; i++)
        {
            bool isUnlockWordFound = WordRegion.instance.Lines.All(lineWord => lineWord.answer == flagItemList[i].flagUnlockWord);
            if (isUnlockWordFound)
            {
                LogController.Debug("Player have found the suitable country word");

            }
        }

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
    public bool isGetCountryRequestDone;
    public IEnumerator GetCountryInfo(string countryName)
    {
        isGetCountryRequestDone = false;
        using (UnityWebRequest request = UnityWebRequest.Get("https://restcountries.eu/rest/v2/name/" + countryName))
        {
            yield return request.SendWebRequest();
            while (!request.isDone)
                yield return null;
            isGetCountryRequestDone = true;
            byte[] result = request.downloadHandler.data;
            string countryJSON = System.Text.Encoding.Default.GetString(result);
            countryJSON = countryJSON.TrimStart(new char[] { '[' }).TrimEnd(new char[] { ']' });           
            JObject jsonObject = JObject.Parse(countryJSON);
            countryInfo.Clear();
            countryInfo = jsonObject.ToObject<Dictionary<string, object>>();

            LogController.Debug("name: " + countryInfo["name"]);
            LogController.Debug("subregion: " + countryInfo["subregion"]);
            LogController.Debug("capital: " + countryInfo["capital"]);
            LogController.Debug("area: " + countryInfo["area"]);
            LogController.Debug("population: " + countryInfo["population"]);

        }
    }
}
