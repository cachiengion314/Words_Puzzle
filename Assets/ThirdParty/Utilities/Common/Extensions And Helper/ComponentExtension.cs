﻿/**
 * Author NBear - nbhung71711 @gmail.com - 2017
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utilities.Common
{
    public static class ComponentExtension
    {
        public static void SetActive(this Component target, bool value)
        {
            try
            {
                target.gameObject.SetActive(value);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.ToString());
            }
        }

        public static bool IsActive(this Component target)
        {
            try
            {
                return target.gameObject.activeSelf;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.ToString());
                return false;
            }
        }

        public static List<T> SortByName<T>(this List<T> objects) where T : UnityEngine.Object
        {
            return objects = objects.OrderBy(m => m.name).ToList();
        }

        public static void SetParent(this Component target, Transform parent)
        {
            target.transform.SetParent(parent);
        }

        public static T FindComponentInChildren<T>(this GameObject objRoot) where T : Component
        {
            // if we don't find the component in this object 
            // recursively iterate children until we do
            T component = objRoot.GetComponent<T>();

            if (null == component)
            {
                // transform is what makes the hierarchy of GameObjects, so 
                // need to access it to iterate children
                Transform trnsRoot = objRoot.transform;
                int iNumChildren = trnsRoot.childCount;

                // could have used foreach(), but it causes GC churn
                for (int iChild = 0; iChild < iNumChildren; ++iChild)
                {
                    // recursive call to this function for each child
                    // break out of the loop and return as soon as we find 
                    // a component of the specified type
                    component = FindComponentInChildren<T>(trnsRoot.GetChild(iChild).gameObject);
                    if (null != component)
                    {
                        break;
                    }
                }
            }

            return component;
        }

        public static List<T> FindComponentsInChildren<T>(this GameObject objRoot) where T : Component
        {
            var list = new List<T>();

            T component = objRoot.GetComponent<T>();

            if (component != null)
                list.Add(component);

            foreach (Transform t in objRoot.transform)
            {
                var components = FindComponentsInChildren<T>(t.gameObject);
                if (components != null)
                    list.AddRange(components);
            }

            return list;
        }

        public static T Instantiate<T>(T original, Transform parent, string pName) where T : UnityEngine.Object
        {
            var obj = UnityEngine.Object.Instantiate(original, parent);
            obj.name = pName;
            return obj;
        }

        #region Simple Pool

        public static T Obtain<T>(this List<T> pool, GameObject prefab, Transform parent, string name = null) where T : Component
        {
            for (int i = 0; i < pool.Count; i++)
            {
                if (!pool[i].gameObject.activeSelf)
                {
                    pool[i].SetParent(parent);
                    pool[i].transform.localPosition = Vector3.zero;
                    return pool[i];
                }
            }

            GameObject temp = UnityEngine.Object.Instantiate(prefab, parent);
            temp.name = name == null ? prefab.name : name;
            temp.transform.localPosition = Vector3.zero;
            var t = temp.GetComponent<T>();
            pool.Add(t);

            return t;
        }

        public static T Obtain<T>(this List<T> pool, Transform parent, string name = null) where T : Component
        {
            for (int i = 0; i < pool.Count; i++)
            {
                if (!pool[i].gameObject.activeSelf)
                {
                    pool[i].SetParent(parent);
                    pool[i].transform.localPosition = Vector3.zero;
                    return pool[i];
                }
            }

            GameObject temp = UnityEngine.Object.Instantiate(pool[0].gameObject, parent);
            temp.name = name == null ? string.Format("{0}_{1}", pool[0].name, (pool.Count() + 1)) : name;
            temp.transform.localPosition = Vector3.zero;
            var t = temp.GetComponent<T>();
            pool.Add(t);

            return t;
        }

        public static T Obtain<T>(this List<T> pool, Transform pParent, int max, string pName = null) where T : Component
        {
            for (int i = 0; i < pool.Count; i++)
            {
                if (!pool[i].gameObject.activeSelf)
                {
                    var obj = pool[i];
                    pool.Remove(obj);
                    obj.transform.SetParent(pParent);
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.localScale = Vector3.one;
                    pool.Add(obj);
                    return obj;
                }
            }

            if (max > 1 && max > pool.Count)
            {
                T temp = UnityEngine.Object.Instantiate(pool[0], pParent);
                pool.Add(temp);
                temp.transform.localPosition = Vector3.zero;
                temp.transform.localScale = Vector3.one;
                if (!string.IsNullOrEmpty(pName))
                    temp.name = pName;
                return temp;
            }
            else
            {
                var obj = pool[0];
                pool.Remove(obj);
                pool.Add(obj);
                return obj;
            }
        }

        public static void Free<T>(this List<T> pool) where T : Component
        {
            foreach (var t in pool)
                t.SetActive(false);
        }

        public static void Free<T>(this List<T> pool, Transform pParent) where T : Component
        {
            for (int i = 0; i < pool.Count; i++)
            {
                pool[i].SetParent(pParent);
                pool[i].SetActive(false);
            }
        }

        public static void Prepare<T>(this List<T> pool, GameObject prefab, Transform parent, int count) where T : Component
        {
            for (int i = 0; i < count; i++)
            {
                GameObject temp = UnityEngine.Object.Instantiate(prefab, parent);
                temp.SetActive(false);
                var t = temp.GetComponent<T>();
                pool.Add(t);
            }
        }

        public static T Obtain<T>(this List<T> pool, T prefab, Transform parent) where T : Component
        {
            for (int i = 0; i < pool.Count; i++)
            {
                if (!pool[i].gameObject.activeSelf)
                    return pool[i];
            }

            T temp = UnityEngine.Object.Instantiate(prefab, parent);
            temp.name = prefab.name;
            pool.Add(temp);

            return temp;
        }

        public static void Prepare<T>(this List<T> pool, T prefab, Transform parent, int count, string name = "") where T : Component
        {
            for (int i = 0; i < count; i++)
            {
                var temp = UnityEngine.Object.Instantiate(prefab, parent);
                temp.SetActive(false);
                if (!string.IsNullOrEmpty(name))
                    temp.name = name;
                pool.Add(temp);
            }
        }

        #endregion

        public static T Find<T>(this List<T> pList, string pName) where T : Component
        {
            for (int i = 0; i < pList.Count; i++)
            {
                if (pList[i].name == pName)
                    return pList[i];
            }
            return null;
        }

        public static T Find<T>(this T[] pList, string pName) where T : Component
        {
            for (int i = 0; i < pList.Length; i++)
            {
                if (pList[i].name == pName)
                    return pList[i];
            }
            return null;
        }

        public static bool IsPrefab(this GameObject target)
        {
            return target.scene.name == null;
        }

        public static Vector2 NativeSize(this Sprite pSrite)
        {
            if (pSrite == null)
                return Vector2.zero;
            var sizeX = pSrite.bounds.size.x * pSrite.pixelsPerUnit;
            var sizeY = pSrite.bounds.size.y * pSrite.pixelsPerUnit;
            return new Vector2(sizeX, sizeY);
        }

        #region Image

        /// <summary>
        /// Sketch image following prefered with
        /// </summary>
        public static Vector2 SketchByHeight(this UnityEngine.UI.Image pImage, float pPreferedHeight, bool pPreferNative = false)
        {
            if (pImage.sprite == null)
                return new Vector2(pPreferedHeight, pPreferedHeight);

            var nativeSizeX = pImage.sprite.bounds.size.x * pImage.sprite.pixelsPerUnit;
            var nativeSizeY = pImage.sprite.bounds.size.y * pImage.sprite.pixelsPerUnit;
            float coeff = pPreferedHeight / nativeSizeY;
            float sizeX = nativeSizeX * coeff;
            float sizeY = nativeSizeY * coeff;
            if (pPreferNative && sizeY > nativeSizeY)
            {
                sizeX = nativeSizeX;
                sizeY = nativeSizeY;
            }
            pImage.rectTransform.sizeDelta = new Vector2(sizeX, sizeY);
            return pImage.rectTransform.sizeDelta;
        }

        /// <summary>
        /// Sketch image following prefered with
        /// </summary>
        public static Vector2 SketchByWidth(this UnityEngine.UI.Image pImage, float pPreferedWith, bool pPreferNative = false)
        {
            if (pImage.sprite == null)
                return new Vector2(pPreferedWith, pPreferedWith);

            var nativeSizeX = pImage.sprite.bounds.size.x * pImage.sprite.pixelsPerUnit;
            var nativeSizeY = pImage.sprite.bounds.size.y * pImage.sprite.pixelsPerUnit;
            float coeff = pPreferedWith / nativeSizeX;
            float sizeX = nativeSizeX * coeff;
            float sizeY = nativeSizeY * coeff;
            if (pPreferNative && sizeX > nativeSizeX)
            {
                sizeX = nativeSizeX;
                sizeY = nativeSizeY;
            }
            pImage.rectTransform.sizeDelta = new Vector2(sizeX, sizeY);
            return pImage.rectTransform.sizeDelta;
        }

        public static Vector2 SketchByFixedHeight(this UnityEngine.UI.Image pImage, float pFixedHeight)
        {
            if (pImage.sprite == null)
                return new Vector2(pFixedHeight, pFixedHeight);

            var nativeSizeY = pImage.sprite.bounds.size.y * pImage.sprite.pixelsPerUnit;
            var nativeSizeX = pImage.sprite.bounds.size.x * pImage.sprite.pixelsPerUnit;
            float sizeX = pFixedHeight * nativeSizeX / nativeSizeY;
            pImage.rectTransform.sizeDelta = new Vector2(sizeX, pFixedHeight);
            return pImage.rectTransform.sizeDelta;
        }

        public static Vector2 SketchByFixedWidth(this UnityEngine.UI.Image pImage, float pFixedWidth)
        {
            if (pImage.sprite == null)
                return new Vector2(pFixedWidth, pFixedWidth);

            var nativeSizeX = pImage.sprite.bounds.size.x * pImage.sprite.pixelsPerUnit;
            var nativeSizeY = pImage.sprite.bounds.size.y * pImage.sprite.pixelsPerUnit;
            float sizeY = nativeSizeX * nativeSizeY / pFixedWidth;
            pImage.rectTransform.sizeDelta = new Vector2(pFixedWidth, sizeY);
            return pImage.rectTransform.sizeDelta;
        }

        public static Vector2 Sketch(this UnityEngine.UI.Image pImage, Vector2 pPreferedSize, bool pPreferNative = false)
        {
            if (pImage.sprite == null)
                return pPreferedSize;

            var nativeSizeX = pImage.sprite.bounds.size.x * pImage.sprite.pixelsPerUnit;
            var nativeSizeY = pImage.sprite.bounds.size.y * pImage.sprite.pixelsPerUnit;
            float coeffX = pPreferedSize.x / nativeSizeX;
            float coeffY = pPreferedSize.y / nativeSizeY;
            float sizeX = nativeSizeX * coeffX;
            float sizeY = nativeSizeY * coeffY;
            if (coeffX > coeffY)
            {
                sizeX *= coeffY;
                sizeY *= coeffY;
            }
            else
            {
                sizeX *= coeffX;
                sizeY *= coeffX;
            }
            if (pPreferNative && (sizeX > nativeSizeX || sizeY > nativeSizeY))
            {
                sizeX = nativeSizeX;
                sizeY = nativeSizeY;
            }
            pImage.rectTransform.sizeDelta = new Vector2(sizeX, sizeY);
            return pImage.rectTransform.sizeDelta;
        }

        public static Vector2 SetNativeSize(this UnityEngine.UI.Image pImage, Vector2 pMaxSize)
        {
            if (pImage.sprite == null)
            {
                pImage.rectTransform.sizeDelta = pMaxSize;
                return pImage.rectTransform.sizeDelta;
            }
            var nativeSizeX = pImage.sprite.bounds.size.x * pImage.sprite.pixelsPerUnit;
            var nativeSizeY = pImage.sprite.bounds.size.y * pImage.sprite.pixelsPerUnit;
            if (nativeSizeX > pMaxSize.x)
                nativeSizeX = pMaxSize.x;
            if (nativeSizeY > pMaxSize.y)
                nativeSizeY = pMaxSize.y;
            pImage.rectTransform.sizeDelta = new Vector2(nativeSizeX, nativeSizeY);
            return pImage.rectTransform.sizeDelta;
        }

        #endregion

        //====================
    }
}