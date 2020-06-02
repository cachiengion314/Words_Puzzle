using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class JsonBuilder : MonoBehaviour
{
    [HideInInspector] public List<LevelData> gameLevels;
    public TextAsset jsonFile;
    public void GetGameLevels()
    {
        if (gameLevels != null)
            gameLevels.Clear();
        gameLevels = new List<LevelData>();
        var level = JsonConvert.DeserializeObject<List<LevelData>>(jsonFile.text);
        foreach (var lv in level)
        {
            LevelData data = new LevelData();
            data.letters = lv.letters;
            data.answers = lv.answers;
            data.validWords = lv.validWords;
            data.numAnswers = lv.numAnswers;
            gameLevels.Add(data);
        }
    }

    public int GetTotalAnswers(LevelData levelData)
    {
        var result = levelData.answers.Split(new string[] { "|" },System.StringSplitOptions.RemoveEmptyEntries);
        return result.Length;
    }
}

[System.Serializable]
public class LevelData
{
    public string letters;
    public string answers;
    public string validWords;
    public int numAnswers;
}
