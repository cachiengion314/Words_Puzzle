/**
 * Author NBear - Nguyen Ba Hung - nbhung71711@gmail.com 
 **/

using UnityEngine;
using System.Collections;
using UnityEditor;
using Utilities.Common;
using Utilities.Components;

namespace Utilities.Components
{
    [CustomEditor(typeof(MyAlignment), true)]
    public class MyAlignmentEditor : UnityEditor.Editor
    {
        private MyAlignment mScript;

        private void OnEnable()
        {
            mScript = (MyAlignment)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Align"))
            {
                mScript.Align();
            }
        }
    }
}