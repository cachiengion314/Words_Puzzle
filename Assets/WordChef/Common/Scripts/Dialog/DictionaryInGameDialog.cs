using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DictionaryInGameDialog : Dialog
{
    public SnapScrolling snapScrolling;
    public ScrollRect scrollRect;
    [Header("Content")] public RectTransform contentRectTransform;
    [Header("Other Object")] public GameObject itemPrefab;

    public GameObject arrowLeftObject;
    public GameObject arrowRightObject;
    
    void Start()
    {
        base.Start();
        snapScrolling.Init(itemPrefab.GetComponent<RectTransform>().sizeDelta.x);
    }

    void Update()
    {
        SetArrowObject();
    }

    void SetArrowObject()
    {
        if(snapScrolling == null) return;
        if (snapScrolling.listItem.Count <= 1)
        {
            arrowLeftObject.SetActive(false);
            arrowRightObject.SetActive(false);
        }
        else
        {
            if (snapScrolling.selectItemID > 0 && snapScrolling.selectItemID < snapScrolling.listItem.Count - 1)
            {
                arrowLeftObject.SetActive(true);
                arrowRightObject.SetActive(true);
            }
            else
            {
                if (snapScrolling.selectItemID == 0)
                {
                    arrowLeftObject.SetActive(false);
                    arrowRightObject.SetActive(true);
                }
                if (snapScrolling.selectItemID == snapScrolling.listItem.Count - 1)
                {
                    arrowLeftObject.SetActive(true);
                    arrowRightObject.SetActive(false);
                }
            }
        }
    }
    
    [ContextMenu("Instantiate")]
    public void InstantiateMeanItem()
    {
        GameObject item = Instantiate(itemPrefab, contentRectTransform);
        item.GetComponent<MeanItemDictionary>().SetParentNestedScrollRect(scrollRect);
        snapScrolling.AddItemToList(item);
    }
}
