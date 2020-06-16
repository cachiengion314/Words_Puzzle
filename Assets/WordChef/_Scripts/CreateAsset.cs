using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAsset : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private JsonBuilder _jsonBuilder;
    public int numStart;
    public int numLevelInChapter = 8;
    public int numChapterInWord = 5;

    private int _numWord;

    private void Start()
    {
        CreateNewScriptAbleObj();
    }

    public void JsonBuilder()
    {
        _gameData.words = new List<Word>();
        _gameData.words = new List<Word>();
        Word word = new Word();
        word.subWords = new List<SubWord>();
        _jsonBuilder.GetGameLevels();
        var numChapter = _jsonBuilder.gameLevels.Count / numLevelInChapter;
        for (int j = 0; j < numChapterInWord; j++)
        {
            SubWord subWord = new SubWord();
            word.subWords.Add(subWord);
        }
        _numWord = numChapter / numChapterInWord;
        for (int i = 0; i < _numWord; i++)
        {
            _gameData.words.Add(word);
        }

        //_word = _gameData.words;
    }

    public void CreateNewScriptAbleObj()
    {
        for (int i = numStart; i < _numWord; i++)
        {
            int index = i;
            JsonBuilder(index);
        }
    }


    private void JsonBuilder(/*GameLevel data, */int index)
    {
        int indexSubword = 0;
        foreach (var subWord in _gameData.words[index].subWords)
        {
            subWord.gameLevels = new List<GameLevel>();
            for (int j = 0; j < numLevelInChapter; j++)
            {
                int indexLevel = j + numLevelInChapter * indexSubword + (index * numChapterInWord * numLevelInChapter);
                var dataFromJson = _jsonBuilder.gameLevels[indexLevel];
                if (indexLevel < _jsonBuilder.gameLevels.Count)
                {
                    var totalAns = _jsonBuilder.GetTotalAnswers(dataFromJson);
                    var data = new GameLevel();
                    data.word = dataFromJson.letters;
                    data.level = dataFromJson.level;
                    data.answers = dataFromJson.valid_answers;
                    data.validWords = "";
                    data.numExtra = totalAns - (dataFromJson.answers == 0 ? totalAns : dataFromJson.answers);
                    subWord.gameLevels.Add(data);
                }
            }
            indexSubword++;
        }

    }


}