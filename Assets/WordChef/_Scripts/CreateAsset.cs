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
    public string path = "Assets/Resources/DataLevel/SubWorld_";

    public void JsonBuilder()
    {
        _jsonBuilder.GetGameLevels();
    }

    public void CreateNewScriptAbleObj()
    {
        _gameData.gameLevels = new List<LevelsData>();
        for (int i = numStart; i < _jsonBuilder.gameLevels.Count; i++)
        {
            InitAsset(i);
        }
    }

    private void InitAsset(int index)
    {
#if UNITY_EDITOR
        //ScriptableObject asset = ScriptableObject.CreateInstance<GameLevel>();
        LevelsData data = /*asset as */new LevelsData();
        JsonBuilder(data, index);
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

    private void JsonBuilder(LevelsData data, int index)
    {
        data.word = _jsonBuilder.gameLevels[index].letters;
        data.answers = _jsonBuilder.gameLevels[index].answers;
        data.validWords = _jsonBuilder.gameLevels[index].validWords;
        _gameData.gameLevels.Add(data);
    }

}