/**
 * Author NBear - nbhung71711 @gmail.com - 2017
 **/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities.Components
{
    public class OptimizedHorizontalScrollView : MonoBehaviour
    {
        public ScrollRect scrollView;
        public RectTransform container;
        public Mask mask;
        public RectTransform prefab;
        public int total = 1;
        public float spacing;

        private RectTransform mMaskRect;
        private int mTotalVisible;
        private int mTotalBuffer = 2;
        private float mHalftSizeContainer;
        private float mPrefabSize;

        private Dictionary<int, int[]> mItemDict = new Dictionary<int, int[]>();
        private List<RectTransform> mListItemRect = new List<RectTransform>();
        private List<OptimizedScrollItem> mListItem = new List<OptimizedScrollItem>();
        private int mOptimizedTotal = 0;
        private Vector3 mStartPos;
        private Vector3 mOffsetVec;
        private bool mInitialized;

        private void Start()
        {
            Initialize(total);
        }

        public void Initialize(int pTotalItems)
        {
            if (mInitialized && total == pTotalItems)
                return;

            mInitialized = true;
            total = pTotalItems;

            container.anchoredPosition3D = new Vector3(0, 0, 0);

            if (mMaskRect == null)
                mMaskRect = mask.GetComponent<RectTransform>();

            Vector2 prefabScale = prefab.rect.size;
            mPrefabSize = prefabScale.x + spacing;

            container.sizeDelta = new Vector2(mPrefabSize * total, prefabScale.y);
            mHalftSizeContainer = container.rect.size.x * 0.5f;

            mTotalVisible = Mathf.CeilToInt(mMaskRect.rect.size.x / mPrefabSize);

            mOffsetVec = Vector3.right;
            mStartPos = container.anchoredPosition3D - (mOffsetVec * mHalftSizeContainer) + (mOffsetVec * (prefabScale.x * 0.5f));
            mOptimizedTotal = Mathf.Min(total, mTotalVisible + mTotalBuffer);
            for (int i = 0; i < mOptimizedTotal; i++)
            {
                GameObject obj = Instantiate(prefab.gameObject, container.transform);
                RectTransform rt = obj.transform as RectTransform;
                rt.anchoredPosition3D = mStartPos + (mOffsetVec * i * mPrefabSize);
                mListItemRect.Add(rt);
                mItemDict.Add(rt.GetInstanceID(), new int[] { i, i });
                obj.SetActive(true);

                OptimizedScrollItem item = obj.GetComponent<OptimizedScrollItem>();
                mListItem.Add(item);
                item.UpdateContent(i, true);
            }

            prefab.gameObject.SetActive(false);
            container.anchoredPosition3D += mOffsetVec * (mHalftSizeContainer - (mMaskRect.rect.size.x * 0.5f));

            scrollView.horizontalScrollbar.onValueChanged.AddListener(ScrollBarChanged);
        }

        public void ScrollBarChanged(float pNormPos)
        {
            int numOutOfView = Mathf.CeilToInt(pNormPos * (total - mTotalVisible));   //number of elements beyond the left boundary (or top)
            int firstIndex = Mathf.Max(0, numOutOfView - mTotalBuffer);   //index of first element beyond the left boundary (or top)
            int originalIndex = firstIndex % mOptimizedTotal;

            int newIndex = firstIndex;
            for (int i = originalIndex; i < mOptimizedTotal; i++)
            {
                moveItemByIndex(mListItemRect[i], newIndex);
                mListItem[i].UpdateContent(newIndex, false);
                newIndex++;
            }
            for (int i = 0; i < originalIndex; i++)
            {
                moveItemByIndex(mListItemRect[i], newIndex);
                mListItem[i].UpdateContent(newIndex, false);
                newIndex++;
            }
        }

        private void moveItemByIndex(RectTransform item, int index)
        {
            int id = item.GetInstanceID();
            mItemDict[id][0] = index;
            item.anchoredPosition3D = mStartPos + (mOffsetVec * index * mPrefabSize);
        }

        public List<OptimizedScrollItem> GetListItem()
        {
            return mListItem;
        }

        //public void AddNewSlotNextTo(OptimizedScrollItem pItem)
        //{
        //    //#1 Find all current items which are next to 
        //    var itemsUnderIndex = new List<OptimizedScrollItem>();
        //    foreach (var item in mListItem)
        //        if (item.Index > pItem.Index)
        //            itemsUnderIndex.Add(item);
        //    itemsUnderIndex.Sort();
        //    //#2 
        //    var nextItem = itemsUnderIndex[0];
        //}

        public void RefreshCurrents()
        {
            foreach (var item in mListItem)
                item.UpdateContent(item.Index, true);
        }
    }
}