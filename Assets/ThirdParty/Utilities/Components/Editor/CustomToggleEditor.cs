using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using Utilities.Common;

namespace Utilities.Components
{
    [CustomEditor(typeof(CustomToggleSlider), true)]
    class CustomToggleEditor : ToggleEditor
    {
        private CustomToggleSlider mToggle;

        protected override void OnEnable()
        {
            base.OnEnable();

            mToggle = (CustomToggleSlider)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical("box");
            {
                EditorHelper.SerializeField(serializedObject, "txtLabel");
                EditorHelper.SerializeField(serializedObject, "onPosition");
                EditorHelper.SerializeField(serializedObject, "offPosition");
                EditorHelper.SerializeField(serializedObject, "toggleTransform");
                EditorHelper.SerializeField(serializedObject, "onObject");
                EditorHelper.SerializeField(serializedObject, "offObject");
                EditorHelper.SerializeField(serializedObject, "sptOnButton");
                EditorHelper.SerializeField(serializedObject, "sptOffButton");

                if (mToggle.txtLabel != null)
                    mToggle.txtLabel.text = EditorGUILayout.TextField("Label", mToggle.txtLabel.text);

                serializedObject.ApplyModifiedProperties();
            }
            EditorGUILayout.EndVertical();

            base.OnInspectorGUI();
        }
    }

    [CustomEditor(typeof(CustomToggleTab), true)]
    class CustomToggleTabEditor : ToggleEditor
    {
        private CustomToggleTab mToggle;

        protected override void OnEnable()
        {
            base.OnEnable();

            mToggle = (CustomToggleTab)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical("box");
            {
                EditorHelper.SerializeField(serializedObject, "txtLabel");
                EditorHelper.SerializeField(serializedObject, "contentsInActive", null, true);
                EditorHelper.SerializeField(serializedObject, "contentsInDeactive", null, true);
                EditorHelper.SerializeField(serializedObject, "sptActiveBackground");
                EditorHelper.SerializeField(serializedObject, "sptInactiveBackground");
                EditorHelper.SerializeField(serializedObject, "colorActiveBackground");
                EditorHelper.SerializeField(serializedObject, "colorInactiveBackground");
                EditorHelper.SerializeField(serializedObject, "sizeActive");
                EditorHelper.SerializeField(serializedObject, "sizeDeactive");

                if (mToggle.txtLabel != null)
                    mToggle.txtLabel.text = EditorGUILayout.TextField("Label", mToggle.txtLabel.text);

                serializedObject.ApplyModifiedProperties();
            }
            EditorGUILayout.EndVertical();

            base.OnInspectorGUI();
        }
    }
}
