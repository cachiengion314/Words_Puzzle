/**
 * Author NBear - nbhung71711 @gmail.com - 2017
 **/

//#define USE_LEANTWEEN
#define USE_DOTWEEN

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if USE_DOTWEEN
using DG.Tweening;
#endif

namespace Utilities.Components
{
    class HorizontalAlignmentUI : MyAlignment
    {
        public float maxContainerWidth;
        public HorizontalType horizontalType;
        public float cellDistance;
        public float tweenTime = 0.25f;
        public bool autoWhenStart;
        [Header("Optional")]
        public float yOffset;

        private List<RectTransform> mChildren = new List<RectTransform>();
        private List<bool> mIndexesChanged = new List<bool>();
        private List<int> mChildrenId = new List<int>();
        private Coroutine mCoroutine;

#if USE_DOTWEEN
        private Tweener mTweener;
#endif

        private void Start()
        {
            if (autoWhenStart)
                Align();
        }

        public override void Align()
        {
            Init();

            if (Math.Abs(mChildren.Count * cellDistance) > maxContainerWidth && maxContainerWidth > 0)
            {
                cellDistance *= maxContainerWidth / (Math.Abs(mChildren.Count * cellDistance));
            }

            switch (horizontalType)
            {
                case HorizontalType.LEFT:
                    for (int i = 0; i < mChildren.Count; i++)
                    {
                        mChildren[i].anchoredPosition = i * new Vector3(cellDistance, yOffset, 0);
                    }
                    break;

                case HorizontalType.RIGHT:
                    for (int i = 0; i < mChildren.Count; i++)
                    {
                        mChildren[i].anchoredPosition = (mChildren.Count - 1 - i) * new Vector3(cellDistance, yOffset, 0) * -1;
                    }
                    break;

                case HorizontalType.CENTER:
                    for (int i = 0; i < mChildren.Count; i++)
                    {
                        mChildren[i].anchoredPosition = i * new Vector3(cellDistance, yOffset, 0);
                    }
                    for (int i = 0; i < mChildren.Count; i++)
                    {
                        mChildren[i].anchoredPosition = new Vector3(
                            mChildren[i].anchoredPosition.x - mChildren[mChildren.Count - 1].localPosition.x / 2,
                            mChildren[i].anchoredPosition.y + yOffset);
                    }
                    break;
            }
        }

        public override void AlignByTweener(Action onFinish)
        {
            StartCoroutine(IEAlignByTweener(onFinish));
        }

        private IEnumerator IEAlignByTweener(Action onFinish)
        {
            Init();

            if (Math.Abs(mChildren.Count * cellDistance) > maxContainerWidth && maxContainerWidth > 0)
            {
                cellDistance *= maxContainerWidth / (Math.Abs(mChildren.Count * cellDistance));
            }

            Vector2[] childrenNewPosition = new Vector2[mChildren.Count];
            Vector2[] childrenPrePosition = new Vector2[mChildren.Count];
            switch (horizontalType)
            {
                case HorizontalType.LEFT:
                    for (int i = 0; i < mChildren.Count; i++)
                    {
                        childrenPrePosition[i] = mChildren[i].anchoredPosition;
                        childrenNewPosition[i] = i * new Vector2(cellDistance, yOffset);
                    }
                    break;

                case HorizontalType.RIGHT:
                    for (int i = 0; i < mChildren.Count; i++)
                    {
                        childrenPrePosition[i] = mChildren[i].anchoredPosition;
                        childrenNewPosition[i] = (mChildren.Count - 1 - i) * new Vector2(cellDistance, yOffset) * -1;
                    }
                    break;

                case HorizontalType.CENTER:
                    for (int i = 0; i < mChildren.Count; i++)
                    {
                        childrenPrePosition[i] = mChildren[i].anchoredPosition;
                        childrenNewPosition[i] = i * new Vector2(cellDistance, yOffset);
                    }
                    for (int i = 0; i < mChildren.Count; i++)
                    {
                        childrenNewPosition[i] = new Vector2(
                            childrenNewPosition[i].x - childrenNewPosition[mChildren.Count - 1].x / 2,
                            childrenNewPosition[i].y + yOffset);
                    }
                    break;
            }

#if USE_LEANTWEEN
            LeanTween.cancel(gameObject);
            LeanTween.value(gameObject, 0, 1, tweenTime)
                .setOnUpdate((float val) =>
                {
                    for (int j = 0; j < mChildren.Count; j++)
                    {
                        Vector2 pos = Vector2.Lerp(childrenPrePosition[j], childrenNewPosition[j], val);
                        mChildren[j].anchoredPosition = pos;
                    }
                });
#elif USE_DOTWEEN
            if (mTweener != null)
                mTweener.Kill();
            float lerp = 0;
            mTweener = DOTween.To(tweenVal => lerp = tweenVal, 0f, 1f, tweenTime)
                .OnUpdate(() =>
                {
                    for (int j = 0; j < mChildren.Count; j++)
                    {
                        Vector2 pos = Vector2.Lerp(childrenPrePosition[j], childrenNewPosition[j], lerp);
                        mChildren[j].anchoredPosition = pos;
                    }
                })
                .SetEase(Ease.InQuint);
#else
            if (mCoroutine != null)
                StopCoroutine(mCoroutine);
            mCoroutine = StartCoroutine(IEArrangeChildren(childrenPrePosition, childrenNewPosition, tweenTime));
#endif

            yield return new WaitForSeconds(tweenTime);

            if (onFinish != null)
                onFinish();
        }

        private void Init()
        {
            var childrenTemp = new List<Transform>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (child.gameObject.activeSelf)
                    childrenTemp.Add(child);
            }
            for (int i = 0; i < childrenTemp.Count; i++)
            {
                if (i > mChildren.Count - 1)
                    mChildren.Add(null);

                if (i > mIndexesChanged.Count - 1)
                    mIndexesChanged.Add(true);

                if (i > mChildrenId.Count - 1)
                    mChildrenId.Add(0);

                if (mChildrenId[i] != childrenTemp[i].gameObject.GetInstanceID())
                {
                    mChildrenId[i] = childrenTemp[i].gameObject.GetInstanceID();
                    mIndexesChanged[i] = true;
                }
                else
                {
                    mIndexesChanged[i] = false;
                }
            }
            for (int i = mChildrenId.Count - 1; i >= 0; i--)
            {
                if (i > childrenTemp.Count - 1)
                {
                    mChildrenId.RemoveAt(i);
                    mChildren.RemoveAt(i);
                    mIndexesChanged.RemoveAt(i);
                    continue;
                }
                if (mIndexesChanged[i] || mChildren[i] == null)
                {
                    mChildren[i] = childrenTemp[i].GetComponent<RectTransform>();
                    mIndexesChanged[i] = false;
                }
            }
        }

        private IEnumerator IEArrangeChildren(Vector2[] pChildrenPrePosition, Vector2[] pChildrenNewPosition, float pDuration)
        {
            float time = 0;
            while (true)
            {
                yield return null;
                time += Time.deltaTime;
                if (time >= pDuration)
                    time = pDuration;
                float lerp = time / pDuration;

                for (int j = 0; j < mChildren.Count; j++)
                {
                    var pos = Vector3.Lerp(pChildrenPrePosition[j], pChildrenNewPosition[j], lerp);
                    mChildren[j].localPosition = pos;
                }

                if (lerp >= 1)
                    break;
            }
        }
    }
}
