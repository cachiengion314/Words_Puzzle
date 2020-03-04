/**
 * Author NBear - nbhung71711 @gmail.com - 2017
 **/

using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utilities.Common
{
    public class CustomPool<T> where T : Component
    {
        #region Members

        public Action<T> onSpawn;

        public T prefab { get; private set; }
        public Transform parent { get; private set; }
        public int initialCount { get; private set; }
        public string name { get; private set; }
        public List<T> activeList { get; private set; }
        public List<T> inactiveList { get; private set; }
        public bool pushToLastSibling;
        public bool autoRelocate;

        #endregion

        //====================================

        #region Public

        public CustomPool(T pPrefab, int pInitialCount, Transform pParent, bool pBuildinPrefab, string pName = "", bool pAutoRelocate = true)
        {
            prefab = pPrefab;
            initialCount = pInitialCount;
            parent = pParent;
            name = pName;
            autoRelocate = pAutoRelocate;

            if (string.IsNullOrEmpty(name))
                name = prefab.name;

            if (parent == null)
            {
                GameObject temp = new GameObject();
                temp.name = string.Format("Pool_{0}", name);
                parent = temp.transform;
            }

            activeList = new List<T>();
            inactiveList = new List<T>();
            inactiveList.Prepare(prefab, pParent, pInitialCount, pPrefab.name);
            if (pBuildinPrefab)
            {
                inactiveList.Add(prefab);
                pPrefab.SetParent(pParent);
                pPrefab.transform.SetAsLastSibling();
                pPrefab.SetActive(false);
            }
        }

        public CustomPool(GameObject pPrefab, int pInitialCount, Transform pParent, bool pBuildinPrefab, string pName = "", bool pAutoRelocate = true)
        {
            prefab = pPrefab.GetComponent<T>();
            initialCount = pInitialCount;
            parent = pParent;
            name = pName;
            autoRelocate = pAutoRelocate;

            if (string.IsNullOrEmpty(name))
                name = prefab.name;

            if (parent == null)
            {
                GameObject temp = new GameObject();
                temp.name = string.Format("Pool_{0}", name);
                parent = temp.transform;
            }

            activeList = new List<T>();
            inactiveList = new List<T>();
            inactiveList.Prepare(prefab, pParent, pInitialCount, pPrefab.name);
            if (pBuildinPrefab)
            {
                inactiveList.Add(prefab);
                pPrefab.transform.SetParent(pParent);
                pPrefab.transform.SetAsLastSibling();
                pPrefab.SetActive(false);
            }
        }

        public T Spawn()
        {
            bool pReused = true;
            return Spawn(Vector3.zero, false, ref pReused);
        }

        public T Spawn(ref bool pReused)
        {
            return Spawn(Vector3.zero, false, ref pReused);
        }

        public T Spawn(Vector3 position, bool pIsWorldPosition)
        {
            bool isOldObject = true;
            return Spawn(position, pIsWorldPosition, ref isOldObject);
        }

        public T Spawn(Vector3 position, bool pIsWorldPosition, ref bool pReused)
        {
            if (inactiveList.Count == 0)
                RelocateInactive();
            if (inactiveList.Count > 0)
            {
                var item = inactiveList[0];
                if (pIsWorldPosition)
                    item.transform.position = position;
                else
                    item.transform.localPosition = position;
                Active(item, true);

                if (onSpawn != null)
                    onSpawn(item);

                if (pushToLastSibling)
                    item.transform.SetAsLastSibling();
                return item;
            }

            T newItem = Object.Instantiate(prefab, parent);
            newItem.name = name;
            inactiveList.Add(newItem);
            pReused = false;

            return Spawn(position, pIsWorldPosition, ref pReused);
        }

        public T Release(T pItem)
        {
            Active(pItem, false);
            return pItem;
        }

        public T FindComponent(GameObject pObj)
        {
            for (int i = 0; i < activeList.Count; i++)
            {
                if (activeList[i].gameObject == pObj)
                {
                    var temp = activeList[i];
                    return temp;
                }
            }
            return null;
        }

        public void ReleaseAll()
        {
            foreach (var item in activeList)
            {
                inactiveList.Add(item);
                item.SetActive(false);
            }
            activeList.Clear();
        }

        public void Destroy(T pItem)
        {
            activeList.Remove(pItem);
            inactiveList.Remove(pItem);
            Object.Destroy(pItem.gameObject);
        }

        public T FindFromActive(T t)
        {
            foreach (var item in activeList)
                if (item == t)
                    return item;

            return null;
        }

        public T GetFromActive(int pIndex)
        {
            if (pIndex < 0 || pIndex >= activeList.Count)
                return null;
            return activeList[pIndex];
        }

        public T FindFromInactive(T t)
        {
            foreach (var item in inactiveList)
                if (item == t)
                    return item;

            return null;
        }

        public void RelocateInactive()
        {
            for (int i = activeList.Count - 1; i >= 0; i--)
                if (!activeList[i].gameObject.activeSelf)
                    Active(activeList[i], false);
        }

        #endregion

        //========================================

        #region Private

        private void Active(T pItem, bool pValue)
        {
            if (pValue)
            {
                activeList.Add(pItem);
                inactiveList.Remove(pItem);
            }
            else
            {
                inactiveList.Add(pItem);
                activeList.Remove(pItem);
            }
            if (pItem != null)
            {
                pItem.SetActive(pValue);
            }
        }

        #endregion
    }
}