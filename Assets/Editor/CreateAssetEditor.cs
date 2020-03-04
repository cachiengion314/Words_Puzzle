using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(CreateAsset),true)]
public class CreateAssetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var scriptTarget = (CreateAsset)target;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Create"))
        {
            scriptTarget.CreateNewScriptAbleObj();
        }
        if (GUILayout.Button("Load Data"))
        {
            scriptTarget.JsonBuilder();
        }
        GUILayout.EndHorizontal();
    }

    
}
