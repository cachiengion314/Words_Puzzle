/**
 * Author NBear - nbhung71711@gmail.com - 2017
 **/

using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Common;

namespace Utilities.Components
{
    public class OptimizedVerticalScrollView : MonoBehaviour
    {
        public ScrollRect scrollView;
        public RectTransform container;
        public Mask mask;
        public OptimizedScrollItem prefab;
        public int total = 1;
        public float spacing;
        public float offset; //phần bù của khung viền (boder)
        public int totalCellOnRow = 1;
        public RectTransform content => scrollView.content;

        private int mTotalVisible;
        private int mTotalBuffer = 2;
        private float mHalftSizeContainer;
        private float mPrefabSizeY;
        private float mPrefabSizeX;

        private List<RectTransform> mListItemRect = new List<RectTransform>();
        private List<OptimizedScrollItem> mListItem = new List<OptimizedScrollItem>();
        private int mOptimizedTotal = 0;
        private Vector3 mStartPos;
        private Vector3 mOffsetVec;
        private Vector2 mPivot;

        public bool autoMatchHeight;
        public float minViewHeight;
        public float maxViewHeight;
        public float itemFixedWidth;
        public float scrollHeight => (scrollView.transform as RectTransform).sizeDelta.y;

        private void Start()
        {
            scrollView.verticalScrollbar.onValueChanged.AddListener(ScrollBarChanged);

            //Initialize(total, true);
        }

        public virtual void Initialize(int pTotalItems, bool pForce)
        {
            if (pTotalItems == total && !pForce)
                return;

            mListItemRect = new List<RectTransform>();

            if (mListItem == null || mListItem.Count == 0)
            {
                mListItem = new List<OptimizedScrollItem>();
                mListItem.Prepare(prefab, container.parent, 5);
            }
            else
            {
                mListItem.Free(container);
            }

            total = pTotalItems;

            container.anchoredPosition3D = new Vector3(0, 0, 0);

            var rectZero = mListItem[0].GetComponent<RectTransform>();
            Vector2 prefabScale = rectZero.rect.size;
            mPrefabSizeY = prefabScale.y + spacing;
            mPrefabSizeX = prefabScale.x + (totalCellOnRow > 1 ? spacing : 0);
            mPivot = rectZero.pivot;
            if (itemFixedWidth > 0)
            {
                var size = rectZero.sizeDelta;
                size.x = itemFixedWidth;
                rectZero.sizeDelta = size;
            }

            container.sizeDelta = new Vector2(prefabScale.x * totalCellOnRow, mPrefabSizeY * Mathf.CeilToInt(total * 1f / totalCellOnRow));
            mHalftSizeContainer = container.rect.size.y * 0.5f;

            var viewRect = mask.transform as RectTransform;
            var scrollRect = scrollView.transform as RectTransform;

            //Re Update min-max view size
            if (autoMatchHeight)
            {
                float preferHeight = container.sizeDelta.y + spacing * 2 + offset;
                if (maxViewHeight > 0 && preferHeight > maxViewHeight)
                    preferHeight = maxViewHeight;
                else if (minViewHeight > 0 && preferHeight < minViewHeight)
                    preferHeight = minViewHeight;

                var size = scrollRect.sizeDelta;
                size.y = preferHeight;
                scrollRect.sizeDelta = size;
            }

            mTotalVisible = Mathf.CeilToInt(viewRect.rect.size.y / mPrefabSizeY) * totalCellOnRow;
            mTotalBuffer *= totalCellOnRow;

            mOffsetVec = Vector3.down;
            mStartPos = container.anchoredPosition3D - (mOffsetVec * mHalftSizeContainer) + (mOffsetVec * (prefabScale.y * 0.5f));
            mOptimizedTotal = Mathf.Min(total, mTotalVisible + mTotalBuffer);

            for (int i = 0; i < mOptimizedTotal; i++)
            {
                int cellIndex = i % totalCellOnRow;
                int rowIndex = Mathf.FloorToInt(i * 1f / totalCellOnRow);

                OptimizedScrollItem item = mListItem.Obtain(container);
                RectTransform rt = item.transform as RectTransform;
                if (itemFixedWidth > 0)
                {
                    var size = rt.sizeDelta;
                    size.x = itemFixedWidth;
                    rt.sizeDelta = size;
                }
                rt.anchoredPosition3D = mStartPos + (mOffsetVec * rowIndex * mPrefabSizeY);
                rt.anchoredPosition3D = new Vector3(-container.rect.size.x / 2 + cellIndex * mPrefabSizeX + mPrefabSizeX * 0.5f,
                    rt.anchoredPosition3D.y,
                    rt.anchoredPosition3D.z);
                mListItemRect.Add(rt);

                item.SetActive(true);
                item.UpdateContent(i, true);
            }

            prefab.gameObject.SetActive(false);
            container.anchoredPosition3D += mOffsetVec * (mHalftSizeContainer - (viewRect.rect.size.y * 0.5f));
        }

        private void FixWidth()
        {
            for (int i = 0; i < mListItemRect.Count; i++)
            {

            }
        }

        /// <summary>
        /// Update size without refresh or change position
        /// </summary>
        public void UpdateNewTotal(int pNewTotal)
        {
            if (pNewTotal <= mOptimizedTotal)
                return;

            float preBarRatio = 1f - mCurrentBarRatio;
            int preTotal = total;
            int bonus = pNewTotal - preTotal;
            float ratioPerPos = 1f / pNewTotal;
            float newRatio = pNewTotal * preBarRatio / preTotal;
            Initialize(pNewTotal, true);
            StartCoroutine(IEClampToOldPosition(1f - newRatio));
        }

        private IEnumerator IEClampToOldPosition(float pBarRatio)
        {
            yield return null;
            scrollView.verticalScrollbar.value = pBarRatio;
            ScrollBarChanged(pBarRatio);
        }

        public void RefreshCurrents()
        {
            foreach (var item in mListItem)
                item.UpdateContent(item.Index, true);
        }

        private float mCurrentBarRatio;

        public void ScrollBarChanged(float pNormPos)
        {
            if (mOptimizedTotal <= 0)
                return;

            mCurrentBarRatio = pNormPos;

            if (totalCellOnRow > 1)
                pNormPos = 1f - pNormPos + 0.06f;
            else
                pNormPos = 1f - pNormPos;
            if (pNormPos > 1)
                pNormPos = 1;

            int numOutOfView = Mathf.CeilToInt(pNormPos * (total - mTotalVisible));   //number of elements beyond the left boundary (or top)
            int firstIndex = Mathf.Max(0, numOutOfView - mTotalBuffer);   //index of first element beyond the left boundary (or top)
            int originalIndex = firstIndex % mOptimizedTotal;

            int newIndex = firstIndex;
            for (int i = originalIndex; i < mOptimizedTotal; i++)
            {
                MoveItemByIndex(mListItemRect[i], newIndex);
                mListItem[i].UpdateContent(newIndex, false);
                newIndex++;
            }
            for (int i = 0; i < originalIndex; i++)
            {
                MoveItemByIndex(mListItemRect[i], newIndex);
                mListItem[i].UpdateContent(newIndex, false);
                newIndex++;
            }
        }

        private void MoveItemByIndex(RectTransform item, int index)
        {
            int cellIndex = index % totalCellOnRow;
            int rowIndex = Mathf.FloorToInt(index * 1f / totalCellOnRow);
            item.anchoredPosition3D = mStartPos + (mOffsetVec * rowIndex * mPrefabSizeY);
            item.anchoredPosition3D = new Vector3(-container.rect.size.x / 2 + cellIndex * mPrefabSizeX + mPrefabSizeX * 0.5f,
                   item.anchoredPosition3D.y,
                   item.anchoredPosition3D.z);
            if (itemFixedWidth > 0)
            {
                var size = item.sizeDelta;
                size.x = itemFixedWidth;
                item.sizeDelta = size;
            }
        }

        public List<OptimizedScrollItem> GetListItem()
        {
            return mListItem;
        }

        public void MoveToTargetIndex(int pIndex, bool pTween = false)
        {
            int rowIndex = Mathf.FloorToInt(pIndex * 1f / totalCellOnRow);

            float contentHeight = content.rect.height;
            float viewHeight = scrollView.viewport.rect.height;
            float scrollLength = contentHeight - viewHeight;
            float targetPosition = rowIndex * mPrefabSizeY;

            float offsetY = mPrefabSizeY * (0.5f - mPivot.y);
            targetPosition -= offsetY;

            if (targetPosition > scrollLength)
                targetPosition = scrollLength;

            scrollView.StopMovement();

            if (!pTween)
                content.anchoredPosition = new Vector2(0, -(scrollLength / 2 - targetPosition));
            else
            {
                float fromY = content.anchoredPosition.y;
                float toY = -(scrollLength / 2 - targetPosition);
                float time = Mathf.Abs(fromY - toY) / 5000f;
                if (time == 0)
                    content.anchoredPosition = new Vector2(0, -(scrollLength / 2 - targetPosition));
                else if (time < 0.04f)
                    time = 0.04f;
                else if (time > 0.4f)
                    time = 0.4f;

                content.anchoredPosition = new Vector2(0, fromY);
                content.DOLocalMoveY(toY, time, true);
            }
        }
    }
}