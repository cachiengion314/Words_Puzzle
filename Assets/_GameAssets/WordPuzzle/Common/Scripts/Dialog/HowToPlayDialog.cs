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
    [SerializeField] private Image _iconHint;
    [SerializeField] private Image _iconSelectedHint;
    [SerializeField] private Image _iconMultipleHint;
    [SerializeField] private Image _iconShuffle;
    [SerializeField] private Image _iconBee;
    [SerializeField] private List<Text> _textTitles;
    [SerializeField] private List<TextMeshProUGUI> _textContent;
    [SerializeField] private List<Image> _imageCenter;
    [SerializeField] private List<Image> _cellsEmpty;
    [SerializeField] private List<Image> _cellsOpen;
    [SerializeField] private List<Image> _cellsMultipleHint;
    [SerializeField] private List<Image> _cellsBee;
    [SerializeField] private List<Image> _bees;
    [SerializeField] private List<Image> _overlays;
    [SerializeField] private List<Image> _buttons;
    [SerializeField] private List<Image> _imagesOther;

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
        if (MainController.instance != null)
        {
            var currTheme = ThemesControl.instance.CurrTheme;
            var arrowLeft = arrowLeftObject.GetComponent<Image>();
            var arrowRight = arrowRightObject.GetComponent<Image>();

            _hand.sprite = currTheme.uiData.howtoplayData.hand;
            arrowLeft.sprite = currTheme.uiData.howtoplayData.arrowLeft;
            arrowRight.sprite = currTheme.uiData.howtoplayData.arrowRight;

            _iconHint.sprite = currTheme.uiData.howtoplayData.iconHint;
            _iconSelectedHint.sprite = currTheme.uiData.howtoplayData.iconSelectedHint;
            _iconMultipleHint.sprite = currTheme.uiData.howtoplayData.iconMultipleHint;
            _iconShuffle.sprite = currTheme.uiData.howtoplayData.iconShuffle;
            _iconBee.sprite = currTheme.uiData.howtoplayData.iconBeeBtn;

            _hand.SetNativeSize();
            _iconHint.SetNativeSize();
            _iconSelectedHint.SetNativeSize();
            _iconMultipleHint.SetNativeSize();
            _iconShuffle.SetNativeSize();
            _iconBee.SetNativeSize();
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
                if (image != null)
                    image.sprite = currTheme.uiData.howtoplayData.imagesCenter[indexItem];
                //image.SetNativeSize();
                indexItem++;
            }

            var indexItem2 = 0;
            foreach (var image in _imagesOther)
            {
                if (image != null)
                    image.sprite = currTheme.uiData.howtoplayData.imagesOther[indexItem2];
                //image.SetNativeSize();
                indexItem2++;
            }

            foreach (var image in _cellsEmpty)
            {
                if (image != null)
                    image.sprite = currTheme.uiData.howtoplayData.iconCellsEmpty;
                //image.SetNativeSize();
            }

            foreach (var image in _cellsOpen)
            {
                if (image != null)
                {
                    image.sprite = currTheme.uiData.howtoplayData.iconCellsOpen;
                    if(image.GetComponentInChildren<Image>() != null)
                        image.GetComponentInChildren<Image>().sprite = currTheme.uiData.howtoplayData.iconCellsOpen;
                }
                //image.SetNativeSize();
            }

            foreach (var image in _cellsMultipleHint)
            {
                if (image != null)
                    image.sprite = currTheme.uiData.howtoplayData.iconCellsMultipleHint;
                //image.SetNativeSize();
            }

            foreach (var image in _cellsBee)
            {
                if (image != null)
                    image.sprite = currTheme.uiData.howtoplayData.iconCellsBee;
                //image.SetNativeSize();
            }

            foreach (var image in _bees)
            {
                if (image != null)
                    image.sprite = currTheme.uiData.howtoplayData.iconBee;
                //image.SetNativeSize();
            }

            foreach (var image in _overlays)
            {
                if (image != null)
                    image.sprite = currTheme.uiData.howtoplayData.boardOverlay;
                //image.SetNativeSize();
            }

            foreach (var image in _buttons)
            {
                if (image != null)
                {
                    image.sprite = currTheme.uiData.howtoplayData.iconButton;
                    image.SetNativeSize();
                }
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
