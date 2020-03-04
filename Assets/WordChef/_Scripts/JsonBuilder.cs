﻿using System.Collections.Generic;
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
            data.answers = lv.validWords;
            data.validWords = lv.answers;
            gameLevels.Add(data);
        }
    }
}

[System.Serializable]
public class LevelData
{
    public string letters;
    public string answers;
    public string validWords;
}
