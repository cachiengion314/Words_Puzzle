using UnityEditor;
#if UNITY_EDITOR
[CustomEditor(typeof(NestedScrollRect))]
[CanEditMultipleObjects]
public class NestedScrollRectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
#endif