using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Common;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Utilities.Components
{
    /// <summary>
    /// Create a hole at a recttransform target
    /// And everythings outside that hole will not be interable
    /// </summary>
    public class HoledLayerMask : MonoBehaviour
    {
        public RectTransform rectContainer;
        public Image imgHole;
        public Image imgLeft;
        public Image imgRight;
        public Image imgTop;
        public Image imgBot;

        private RectTransform mRectHole;
        private RectTransform mRectBorderLeft;
        private RectTransform mRectBorderRight;
        private RectTransform mRectBorderTop;
        private RectTransform mRectBorderBot;
        private RectTransform mCurrentTarget;
        private bool mTweening;

        private Bounds mBounds;

        private void OnEnable()
        {
            mRectHole = imgHole.rectTransform;
            mRectBorderLeft = imgLeft.rectTransform;
            mRectBorderRight = imgRight.rectTransform;
            mRectBorderTop = imgTop.rectTransform;
            mRectBorderBot = imgBot.rectTransform;

            mBounds = rectContainer.Bounds();
            mRectBorderTop.pivot = new Vector2(0.5f, 1f);
            mRectBorderBot.pivot = new Vector2(0.5f, 0f);
            mRectBorderLeft.pivot = new Vector2(0, 0.5f);
            mRectBorderRight.pivot = new Vector2(1f, 0.5f);
        }

        private void Update()
        {
            //Change size of 4-border following the size and position of hole
            var canvasWidth = mBounds.size.x;
            var canvasHeight = mBounds.size.y;
            var holePosition = mRectHole.anchoredPosition;
            var holeHalfSize = mRectHole.sizeDelta / 2f;
            float borderLeft = mBounds.min.x;
            float borderRight = mBounds.max.x;
            float borderTop = mBounds.max.y;
            float borderBot = mBounds.min.y;
            float layerLeftW = (holePosition.x - holeHalfSize.x) - borderLeft;
            float layerRightW = borderRight - (holePosition.x + holeHalfSize.x);
            float layerLeftH = mRectHole.sizeDelta.y;
            float layerRightH = mRectHole.sizeDelta.y;
            float layerTopW = mBounds.size.x;
            float layerBotW = mBounds.size.x;
            float layerTopH = borderTop - (holePosition.y + holeHalfSize.y);
            float layerBotH = (holePosition.y - holeHalfSize.y) - borderBot;

            mRectBorderLeft.sizeDelta = new Vector2(layerLeftW, layerLeftH);
            mRectBorderRight.sizeDelta = new Vector2(layerRightW, layerRightH);
            mRectBorderTop.sizeDelta = new Vector2(layerTopW, layerTopH);
            mRectBorderBot.sizeDelta = new Vector2(layerBotW, layerBotH);

            var leftLayerPos = mRectBorderLeft.anchoredPosition;
            leftLayerPos.y = mRectHole.anchoredPosition.y;
            mRectBorderLeft.anchoredPosition = leftLayerPos;
            var rightLayerPos = mRectBorderRight.anchoredPosition;
            rightLayerPos.y = mRectHole.anchoredPosition.y;
            mRectBorderRight.anchoredPosition = rightLayerPos;

            if (mCurrentTarget != null && !mTweening)
                mRectHole.sizeDelta = new Vector2(mCurrentTarget.rect.width * mCurrentTarget.localScale.x, mCurrentTarget.rect.height * mCurrentTarget.localScale.y);
        }

        public void Active(bool pValue)
        {
            enabled = pValue;
            gameObject.SetActive(pValue);
        }

        public void SetColor(Color pColor)
        {
            imgLeft.color = pColor;
            imgTop.color = pColor;
            imgRight.color = pColor;
            imgBot.color = pColor;
        }

        public void FocusToTargetImmediately(RectTransform pTarget, bool pPostChecking = true)
        {
            Active(true);

            mBounds = rectContainer.Bounds();
            mCurrentTarget = pTarget;

            var targetPivot = pTarget.pivot;
            mRectHole.position = pTarget.position;
            mRectHole.sizeDelta = new Vector2(pTarget.rect.width, pTarget.rect.height);
            var x = mRectHole.anchoredPosition.x - pTarget.rect.width * targetPivot.x + pTarget.rect.width / 2f;
            var y = mRectHole.anchoredPosition.y - pTarget.rect.height * targetPivot.y + pTarget.rect.height / 2f;
            mRectHole.anchoredPosition = new Vector2(x, y);

            imgHole.raycastTarget = false;

            if (pPostChecking)
                StartCoroutine(IEPostValidating(pTarget));
        }

        /// <summary>
        /// Incase target is in a scrollview or some sort of UI element take one or few frames to refresh
        /// We need to observer target longer
        /// </summary>
        private IEnumerator IEPostValidating(RectTransform pTarget)
        {
            for (int i = 0; i < 5; i++)
            {
                yield return null;
                FocusToTargetImmediately(pTarget, false);
            }
        }

        /// <summary>
        /// Make a clone of sprite and use it as a mask to cover around target
        /// NOTE: condition is cloned sprite texture must be TRUE COLOR
        /// So this method is not so helpful
        /// </summary>
        public void CreateHoleFromSprite(Sprite spriteToClone)
        {
            try
            {
                int posX = (int)spriteToClone.rect.x;
                int posY = (int)spriteToClone.rect.y;
                int sizeX = (int)(spriteToClone.bounds.size.x * spriteToClone.pixelsPerUnit);
                int sizeY = (int)(spriteToClone.bounds.size.y * spriteToClone.pixelsPerUnit);

                Texture2D newTex = new Texture2D(sizeX, sizeY, spriteToClone.texture.format, false);
                Color[] colors = spriteToClone.texture.GetPixels(posX, posY, sizeX, sizeY);

                for (int i = 0; i < colors.Length; i++)
                {
                    if (colors[i].a == 0)
                        colors[i] = Color.white;
                    else
                        colors[i] = Color.clear;
                }
                newTex.SetPixels(colors);
                newTex.Apply();

                Sprite sprite = Sprite.Create(newTex, new Rect(0, 0, newTex.width, newTex.height), spriteToClone.pivot, spriteToClone.pixelsPerUnit, 0, SpriteMeshType.Tight, spriteToClone.border);
                imgHole.sprite = sprite;
                imgHole.color = imgLeft.color;
                if (sprite.border != Vector4.zero)
                    imgHole.type = Image.Type.Sliced;
                else
                    imgHole.type = Image.Type.Simple;
            }
            catch (Exception ex)
            {
                imgHole.sprite = null;
                imgHole.color = Color.clear;
                Debug.LogError(ex.ToString());
            }
        }

        public void ClearSpriteMask()
        {
            imgHole.sprite = null;
            imgHole.color = Color.clear;
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(HoledLayerMask))]
    public class InputMaskerEditor : Editor
    {
        private HoledLayerMask mScript;
        private Sprite mSprite;

        private void OnEnable()
        {
            mScript = (HoledLayerMask)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            mSprite = (Sprite)EditorGUILayout.ObjectField(mSprite, typeof(Sprite), true);

            if (GUILayout.Button("CloneSprite"))
            {
                mScript.CreateHoleFromSprite(mSprite);
            }
        }
    }
#endif
}
