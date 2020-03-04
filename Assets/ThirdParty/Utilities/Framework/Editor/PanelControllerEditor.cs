using UnityEditor;
using UnityEngine;

namespace Utilities.Pattern.UI
{
    [CustomEditor(typeof(PanelController), true)]
    public class PanelControllerEditor : UnityEditor.Editor
    {
        private PanelController mScrypt;

        private void OnEnable()
        {
            mScrypt = (PanelController)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Children Count: " + mScrypt.StackCount, EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Index: " + mScrypt.Index, EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Display Order: " + mScrypt.PanelOrder, EditorStyles.boldLabel);
            if (mScrypt.GetComponent<Canvas>() != null)
                GUILayout.Label("NOTE: sub-panel should not have Canvas component!\nIt should be inherited from parent panel");
            EditorGUILayout.EndVertical();
        }
    }
}