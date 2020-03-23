using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAsset : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private JsonBuilder _jsonBuilder;
    public int numEnd;
    public int numStart;
    public int numWord;
    public int numSubWord;
    public string path = "Assets/Resources/DataLevel/SubWorld_";

    public void JsonBuilder()
    {
        _gameData.words = new List<Word>();
        Word word = new Word();
        word.subWords = new List<SubWord>();
        for (int j = 0; j < numSubWord; j++)
        {
            SubWord subWord = new SubWord();
            word.subWords.Add(subWord);
        }
        _jsonBuilder.GetGameLevels();
        for (int i = 0; i < numWord; i++)
        {
            _gameData.words.Add(word);
        }
    }

    public void CreateNewScriptAbleObj()
    {
        for (int i = numStart; i < numWord; i++)
        {
            InitAsset(i);
        }
    }

    private void InitAsset(int index)
    {
#if UNITY_EDITOR
        //ScriptableObject asset = ScriptableObject.CreateInstance<GameLevel>();
        //GameLevel data = /*asset as *GameLevel();
        JsonBuilder(/*data, */index);
        //CreateAssets(asset, index);
#endif
    }

    private void CreateAssets(ScriptableObject asset, int index)
    {
#if UNITY_EDITOR
        //var filePath = path + numWord;
        //if (!Directory.Exists(filePath))
        //{
        //    Directory.CreateDirectory(filePath);
        //}
        //AssetDatabase.CreateAsset(asset, filePath + "/Level_" + index + ".asset");
        //AssetDatabase.SaveAssets();
        //EditorUtility.FocusProjectWindow();
        //Selection.activeObject = asset;
#endif
    }

    private void JsonBuilder(/*GameLevel data, */int index)
    {
        for (int i = 0; i < _gameData.words[index].subWords.Count; i++)
        {
            int indexSubword = i;
            var subWord = _gameData.words[index].subWords[indexSubword];
            subWord.gameLevels = new List<GameLevel>();
            for (int j = 0; j < 7; j++)
            {
                int indexLevel = j + 7 * indexSubword;
                if (indexLevel < _jsonBuilder.gameLevels.Count)
                {
                    var data = new GameLevel();
                    data.word = _jsonBuilder.gameLevels[indexLevel].letters;
                    data.answers = _jsonBuilder.gameLevels[indexLevel].answers;
                    data.validWords = _jsonBuilder.gameLevels[indexLevel].validWords;
                    subWord.gameLevels.Add(data);
                }
            }
        }
    }

}