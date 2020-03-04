using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities.Components
{
    [ExecuteInEditMode]
    public class ResolutionMatch : MonoBehaviour
    {
        [SerializeField] private Camera mCamera;
        [SerializeField] private List<CanvasScaler> mCanvasScaler;

        private void OnEnable()
        {
            Match();
        }

        public void Match()
        {
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            float screenAspect = screenWidth * 1.0f / screenHeight;
            float milestoneAspect = GameplayConfig.ORIGIN_W / GameplayConfig.ORIGIN_H;
            float scaleFactor = 0f;

            if (screenAspect <= milestoneAspect)
            {
                mCamera.orthographicSize = (GameplayConfig.ORIGIN_W / 100.0f) / (2 * screenAspect);
            }
            else
            {
                mCamera.orthographicSize = 11.4f;
                scaleFactor = 1f;
            }

            foreach (var canvasScaler in mCanvasScaler)
            {
                canvasScaler.matchWidthOrHeight = scaleFactor;
            }
        }
    }

#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(ResolutionMatch))]
    public class ResolutionMatchEditor : UnityEditor.Editor
    {
        private ResolutionMatch mScript;

        private void OnEnable()
        {
            mScript = (ResolutionMatch)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Match")) { mScript.Match(); }
        }
    }
#endif

}