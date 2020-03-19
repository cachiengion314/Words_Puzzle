using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapScrolling : MonoBehaviour
{
    [Header("Content")] public RectTransform contentRectTransform;
    [Header("ScrollRect")] public ScrollRect scrollRect;
    [Range(0, 500)] public int itemOffset;
    [Range(0f, 20f)] public float snapSpeed;
    public List<GameObject> listItem;
    public List<Vector2> listItemPos;
    
    private float sizeXItem;
    private Vector2 contentVector;

    public int selectItemID;
    public int previousID;
    bool isScrolling;


    [Header("Panigation")] 
    public GameObject panigationPrefab;
    public RectTransform parentPanigationRect;
    public List<GameObject> listPanigationObject;
    public float panigationOffset;
    private float panigationSize;

    private void Start()
    {
        panigationSize = panigationPrefab.GetComponent<RectTransform>().sizeDelta.x;
    }
    
    private void Update()
    {
        SnapScroll();
    }

    public void Init(float sizeItem)
    {
        sizeXItem = sizeItem;
    }

    public void AddItemToList(GameObject item)
    {
        RectTransform itemRect = item.GetComponent<RectTransform>();

        if (listItem.Count > 0)
        {
            itemRect.anchoredPosition = new Vector2(listItem[listItem.Count-1].GetComponent<RectTransform>().anchoredPosition.x + sizeXItem + itemOffset, 0);
        }
        else
        {
            itemRect.anchoredPosition = Vector2.zero;
        }
        
        listItem.Add(item);
        listItemPos.Add(-itemRect.anchoredPosition);
        
        InstantiatePanigation();
    }
    
    #region Panigation
    void InstantiatePanigation()
    {
        GameObject panigation = Instantiate(panigationPrefab, parentPanigationRect);
        listPanigationObject.Add(panigation);
        SortPanigation();

        if (listPanigationObject.Count <= 1)
        {
            listPanigationObject[selectItemID].GetComponent<Panigation>().SetSprite(true);
        }
    }

    void SortPanigation()
    {
        float longSizeAllPanigation = listPanigationObject.Count * panigationSize + (listPanigationObject.Count - 1) * panigationOffset;
        float distanceBetweenPanigation = panigationSize + panigationOffset;
        for (int i = 0; i < listPanigationObject.Count; i++)
        {
            if (i == 0)
            {
                listPanigationObject[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(parentPanigationRect.anchoredPosition.x - longSizeAllPanigation/2 + panigationSize/2, 0);
            }
            else
            {
                listPanigationObject[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(listPanigationObject[i-1].GetComponent<RectTransform>().anchoredPosition.x + distanceBetweenPanigation, 0);
            }
        }
    }

    void ChangeSpritePanigation()
    {
        listPanigationObject[selectItemID].GetComponent<Panigation>().SetSprite(true);
        listPanigationObject[previousID].GetComponent<Panigation>().SetSprite(false);
    }
    #endregion
    

    public void SnapScroll()
    {
        if (listItem.Count <= 0) return;
        if (isScrolling)
        {
            float nearesPos = float.MaxValue;
            for (int i = 0; i < listItem.Count; i++)
            {
                float distance = Mathf.Abs(contentRectTransform.anchoredPosition.x - listItemPos[i].x);
                if (distance < nearesPos)
                {
                    nearesPos = distance;
                    selectItemID = i;
                }
            }
        }

        if (selectItemID >= listItem.Count)
        {
            selectItemID = listItem.Count - 1;
        }
        else if(selectItemID < 0)
        {
            selectItemID = 0;
        }
        
        if (previousID != selectItemID)
        {
            ChangeSpritePanigation();
            previousID = selectItemID;
        }

        if (isScrolling) return;
        contentVector.x = Mathf.SmoothStep(contentRectTransform.anchoredPosition.x, listItemPos[selectItemID].x, snapSpeed * Time.deltaTime);
        contentRectTransform.anchoredPosition = contentVector;
    }

    public void Scrolling(bool scroll)
    {
        isScrolling = scroll;
    }
}
