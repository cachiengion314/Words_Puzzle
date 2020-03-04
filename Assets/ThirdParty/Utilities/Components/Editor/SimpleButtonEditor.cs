using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using Utilities.Common;

namespace Utilities.Components
{
    [CustomEditor(typeof(JustButton), true)]
    class JustButtonEditor : ButtonEditor
    {
        private JustButton mButton;

        protected override void OnEnable()
        {
            base.OnEnable();

            mButton = (JustButton)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginVertical("box");
            {
                EditorHelper.SerializeField(serializedObject, "mImg");
                EditorHelper.SerializeField(serializedObject, "mPivotForFX");
                EditorHelper.SerializeField(serializedObject, "mEnabledFX");
                EditorHelper.SerializeField(serializedObject, "mRelatedObjects", null, true);
                EditorHelper.SerializeField(serializedObject, "mInitialScale");

                //int size = relatedObjs.arraySize + 1;
                //for (int i = 0; i < size; i++)
                //{
                //    relatedObjs.GetArrayElementAtIndex(i);
                //    var showChildren = EditorGUILayout.PropertyField(relatedObjs);
                //    if (!relatedObjs.NextVisible(showChildren))
                //    {
                //        EditorGUILayout.Space();
                //        break;
                //    }
                //}
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
        }
    }

    //==========================================

    [CustomEditor(typeof(SimpleButton), true)]
    class SimpleButtonEditor : JustButtonEditor
    {
        private SimpleButton mButton;

        protected override void OnEnable()
        {
            base.OnEnable();

            mButton = (SimpleButton)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginVertical("box");
            {
                EditorHelper.SerializeField(serializedObject, "mLabel");

                if (mButton.label != null)
                    mButton.label.text = EditorGUILayout.TextField("Label", mButton.label.text);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
        }
    }

    //==========================================

    [CustomEditor(typeof(SimpleTMPButton), true)]
    class SimpleTMPButtonEditor : JustButtonEditor
    {
        private SimpleTMPButton mButton;

        protected override void OnEnable()
        {
            base.OnEnable();

            mButton = (SimpleTMPButton)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginVertical("box");
            {
                EditorHelper.SerializeField(serializedObject, "mLabelTMP");

                if (mButton.labelTMP != null)
                    mButton.labelTMP.text = EditorGUILayout.TextField("Label TMP", mButton.labelTMP.text);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
        }
    }

    //==========================================

    [CustomEditor(typeof(PriceTMPButton), true)]
    class PriceTMPButtonEditor : SimpleTMPButtonEditor
    {
        private PriceTMPButton mButton;

        protected override void OnEnable()
        {
            base.OnEnable();

            mButton = (PriceTMPButton)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginVertical("box");
            {
                EditorHelper.SerializeField(serializedObject, "mLabelTMPCost");
                EditorHelper.SerializeField(serializedObject, "mImgCurrency");
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
