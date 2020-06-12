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
            data.level = lv.level;
            data.letters = lv.letters;
            data.valid_answers = lv.valid_answers;
            data.answers = lv.answers;
            gameLevels.Add(data);
        }
    }

    public int GetTotalAnswers(LevelData levelData)
    {
        var result = levelData.valid_answers.Split(new string[] { "|" }, System.StringSplitOptions.RemoveEmptyEntries);
        return result.Length;
    }
}

[System.Serializable]
public class LevelData
{
    public int level;
    public string letters;
    public string valid_answers;
    public int answers;
}
