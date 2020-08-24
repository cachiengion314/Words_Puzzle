using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayDialog : Dialog
{
    public static HowToPlayDialog instance;
    public SnapScrolling snapScrolling;
    public GameObject arrowLeftObject;
    public GameObject arrowRightObject;
    [Header("Other Object")] public GameObject item;
    [Header("Other Object")] public List<GameObject> items;
    [Header("THEME UI CHANGE")]
    [SerializeField] private Image _hand;
    [SerializeField] private List<Text> _textTitles;
    [SerializeField] private List<TextMeshProUGUI> _textContent;
    [SerializeField] private List<Image> _imageCenter;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        base.Start();
        CheckTheme();
        snapScrolling.Init(item.GetComponent<RectTransform>().sizeDelta.x);
        for (int i = 0; i < items.Count; i++)
        {
            snapScrolling.AddItemToList(items[i]);
        }
    }

    private void CheckTheme()
    {
        if(MainController.instance != null)
        {
            var currTheme = ThemesControl.instance.CurrTheme;
            var arrowLeft = arrowLeftObject.GetComponent<Image>();
            var arrowRight = arrowRightObject.GetComponent<Image>();

            _hand.sprite = currTheme.uiData.howtoplayData.hand;
            arrowLeft.sprite = currTheme.uiData.howtoplayData.arrowLeft;
            arrowRight.sprite = currTheme.uiData.howtoplayData.arrowRight;

            _hand.SetNativeSize();
            arrowLeft.SetNativeSize();
            arrowRight.SetNativeSize();

            foreach (var text in _textTitles)
            {
                text.color = currTheme.fontData.colorContentDialog;
            }

            foreach (var text in _textContent)
            {
                text.color = currTheme.fontData.colorContentDialog;
            }

            var indexItem = 0;
            foreach (var image in _imageCenter)
            {
                image.sprite = currTheme.uiData.howtoplayData.imagesCenter[indexItem];
                image.SetNativeSize();
                indexItem++;
            }
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
