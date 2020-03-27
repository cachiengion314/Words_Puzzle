using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Sound))]
[CanEditMultipleObjects]
public class SoundControllerInspector : BaseInspector
{
    public SerializedProperty buttonClips;
    public SerializedProperty otherClips;
    public SerializedProperty collectClips;
    public SerializedProperty sceneClips;
    private void OnEnable()
    {
        buttonClips = serializedObject.FindProperty("buttonClips");
        otherClips = serializedObject.FindProperty("otherClips");
        collectClips = serializedObject.FindProperty("collectClips");
        sceneClips = serializedObject.FindProperty("sceneClips");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        ShowArrayProperty(buttonClips, typeof(Sound.Button), "Button Clips");
        ShowArrayProperty(otherClips, typeof(Sound.Others), "Other Clips");
        ShowArrayProperty(collectClips, typeof(Sound.Collects), "Collect Clips");
        ShowArrayProperty(sceneClips, typeof(Sound.Scenes), "Scene Clips");

        serializedObject.ApplyModifiedProperties();
    }
}