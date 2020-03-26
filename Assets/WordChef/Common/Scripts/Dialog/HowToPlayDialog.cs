using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayDialog : Dialog
{
    public static HowToPlayDialog instance;
    public SnapScrolling snapScrolling;
    public GameObject arrowLeftObject;
    public GameObject arrowRightObject;
    [Header("Other Object")] public GameObject item;
    [Header("Other Object")] public List<GameObject> items;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        base.Start();
        snapScrolling.Init(item.GetComponent<RectTransform>().sizeDelta.x);
        for (int i = 0; i < items.Count; i++)
        {
            snapScrolling.AddItemToList(items[i]);
        }
    }

    void Update()
    {
        SetArrowObject();
    }

    void SetArrowObject()
    {
        if (snapScrolling == null) return;
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
    public void ArrowPageButton(bool isNext)
    {
        if (isNext)
        {
            snapScrolling.selectItemID++;
        }
        else
        {
            snapScrolling.selectItemID--;
        }
    }

    public void ShowMeanWordByID(int ID)
    {
        TweenControl.GetInstance().DelayCall(transform, 0.1f, () =>
        {
            snapScrolling.selectItemID = ID;
        });
    }
}
