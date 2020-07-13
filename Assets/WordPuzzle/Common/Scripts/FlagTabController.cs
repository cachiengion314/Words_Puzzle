using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct FlagItem
{
    public Sprite flagImage;
    public string flagName;
    public bool isLocked;
}
public class FlagTabController : MonoBehaviour
{
    public static FlagTabController instance;

    public List<FlagItem> flagItemList;
    public List<string> allWordsList = new List<string>();
    public GameData gameData;
    private void Awake()
    {
        instance = this;
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
}
