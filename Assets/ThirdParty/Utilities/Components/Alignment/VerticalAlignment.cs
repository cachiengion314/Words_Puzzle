/**
 * Author NBear - nbhung71711 @gmail.com - 2017
 **/

//#define USE_LEANTWEEN
//#define USE_DOTWEEN

using UnityEngine;
using System.Collections;
using System;

namespace Utilities.Components
{
    public enum VerticalType
    {
        TOP,
        BOTTOM,
        CENTER,
    }

    public class VerticalAlignment : MyAlignment
    {
        public VerticalType verticalType;
        public float rowDistance;
        [Header("Optional")]
        public float xOffset;

        private Transform[] mChildren;
        private Coroutine mCoroutine;

        private void Start()
        {
            Align();
        }

        private void Init()
        {
            mChildren = new Transform[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (child.gameObject.activeSelf)
                    mChildren[i] = transform.GetChild(i);
            }
        }

        public override void Align()
        {
            Init();

            switch (verticalType)
            {
                case VerticalType.TOP:
                    for (int i = 0; i < mChildren.Length; i++)
                    {
                        mChildren[i].transform.localPosition = i * new Vector3(xOffset, rowDistance, 0);
                    }
                    break;

                case VerticalType.BOTTOM:
                    for (int i = 0; i < mChildren.Length; i++)
                    {
                        mChildren[i].transform.localPosition = (mChildren.Length - 1 - i) * new Vector3(xOffset, rowDistance, 0) * -1;
                    }
                    break;

                case VerticalType.CENTER:
                    for (int i = 0; i < mChildren.Length; i++)
                    {
                        mChildren[i].transform.localPosition = i * new Vector3(xOffset, rowDistance, 0);
                    }
                    for (int i = 0; i < mChildren.Length; i++)
                    {
                        mChildren[i].transform.localPosition = new Vector3(
                            mChildren[i].transform.localPosition.x + xOffset,
                            mChildren[i].transform.localPosition.y - mChildren[mChildren.Length - 1].transform.localPosition.y / 2,
                            mChildren[i].transform.localPosition.z);
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

            Vector3[] childrenPrePosition = new Vector3[mChildren.Length];
            Vector3[] childrenNewPosition = new Vector3[mChildren.Length];
            switch (verticalType)
            {
                case VerticalType.TOP:
                    for (int i = 0; i < childrenNewPosition.Length; i++)
                    {
                        childrenPrePosition[i] = mChildren[i].localPosition;
                        childrenNewPosition[i] = i * new Vector3(xOffset, rowDistance, 0);
                    }
                    break;

                case VerticalType.BOTTOM:
                    for (int i = 0; i < childrenNewPosition.Length; i++)
                    {
                        childrenPrePosition[i] = mChildren[i].localPosition;
                        childrenNewPosition[i] = (childrenNewPosition.Length - 1 - i) * new Vector3(xOffset, rowDistance, 0) * -1;
                    }
                    break;

                case VerticalType.CENTER:
                    for (int i = 0; i < childrenNewPosition.Length; i++)
                    {
                        childrenPrePosition[i] = mChildren[i].localPosition;
                        childrenNewPosition[i] = i * new Vector3(xOffset, rowDistance, 0);
                    }
                    for (int i = 0; i < childrenNewPosition.Length; i++)
                    {
                        childrenNewPosition[i] = new Vector3(
                            childrenNewPosition[i].x + xOffset,
                            childrenNewPosition[i].y - childrenNewPosition[childrenNewPosition.Length - 1].y / 2,
                            childrenNewPosition[i].z);
                    }
                    break;
            }

#if USE_LEANTWEEN
            LeanTween.cancel(gameObject);
            LeanTween.value(gameObject, 0f, 1f, 0.25f)
                .setOnUpdate((float val) =>
                {
                    for (int j = 0; j < mChildren.Length; j++)
                    {
                        var pos = Vector3.Lerp(childrenPrePosition[j], childrenNewPosition[j], val);
                        mChildren[j].localPosition = pos;
                    }
                });
#elif USE_DOTWEEN
#else
            if (mCoroutine != null)
                StopCoroutine(mCoroutine);
            mCoroutine = StartCoroutine(IEArrangeChildren(childrenPrePosition, childrenNewPosition, 0.25f));
#endif

            yield return new WaitForSeconds(0.25f);

            if (onFinish != null)
                onFinish();
        }

        private IEnumerator IEArrangeChildren(Vector3[] pChildrenPrePosition, Vector3[] pChildrenNewPosition, float pDuration)
        {
            float time = 0;
            while (true)
            {
                yield return null;
                time += Time.deltaTime;
                if (time >= pDuration)
                    time = pDuration;
                float lerp = time / pDuration;

                for (int j = 0; j < mChildren.Length; j++)
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