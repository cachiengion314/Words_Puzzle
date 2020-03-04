using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAsset : MonoBehaviour
{
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
        for (int i = numStart; i < numEnd; i++)
        {
            InitAsset(i);
        }
    }

    private void InitAsset(int index)
    {
#if UNITY_EDITOR
        ScriptableObject asset = ScriptableObject.CreateInstance<GameLevel>();
        GameLevel data = asset as GameLevel;
        JsonBuilder(data, index);
        CreateAssets(asset, index);
#endif
    }

    private void CreateAssets(ScriptableObject asset, int index)
    {
#if UNITY_EDITOR
        var filePath = path + numWord;
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        AssetDatabase.CreateAsset(asset, filePath + "/Level_" + index + ".asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
#endif
    }

    private void JsonBuilder(GameLevel data, int index)
    {
        data.word = _jsonBuilder.gameLevels[index].letters;
        data.answers = _jsonBuilder.gameLevels[index].answers;
        data.validWords = _jsonBuilder.gameLevels[index].validWords;
    }

}
