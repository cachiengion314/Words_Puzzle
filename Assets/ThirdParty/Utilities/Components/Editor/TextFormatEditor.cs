using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Utilities.Common;

namespace Utilities.Components
{
    [CustomEditor(typeof(TextFormat))]
    public class TextFormatEditor : UnityEditor.Editor
    {
        private TextFormat mScript;

        private void OnEnable()
        {
            mScript = (TextFormat)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorHelper.Button("Format", () =>
            {
                mScript.Format();
            });

            EditorHelper.Button("Format All", () =>
            {
                mScript.FormatAllChilren();
            });
        }
    }
}