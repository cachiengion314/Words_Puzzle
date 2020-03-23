using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Create Data", order = 0)]
public class GameData : ScriptableObject
{
    public List<Word> words;
}

[Serializable]
public class Word
{
    public List<SubWord> subWords;
}

[Serializable]
public class SubWord
{
    public List<GameLevel> gameLevels;
}