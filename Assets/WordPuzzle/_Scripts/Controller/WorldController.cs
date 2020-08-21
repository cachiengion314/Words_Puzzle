using Superpow;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class WorldController : BaseController
{
    public RectTransform mainUI, scrollContent;
    //public VerticalLayoutGroup contentLayoutGroup;
    //public SnapScrollRect snapScroll;
    public ContentSizeFitter contentSize;
    public VerticalLayoutGroup verticalLayoutGroup;

    public GameObject title;
    [HideInInspector] public List<WorldItem> worldItems;
    [SerializeField] private GameData _data;
    [SerializeField] private WorldItem _wordItemPfb;
    [SerializeField] private Transform _root;
    [SerializeField] private ScrollRect _scroll;
    private ScrollView scrollView;

    private bool _isCheckItem;
    private float _heightItem;
    private float _heightRoot;
    private Vector2 posTarget;
    public int target;
    [SerializeField] private RectTransform posFirst;
    [SerializeField] private RectTransform posLast;
    [SerializeField] private int countChapterMax = 650;

    private int countItem = 0;
    private int countChapter;
    private int wordNew;
    private Camera _camera;
    private Vector3 _posEnd;
    private Vector3 _posStart;

    protected override void Awake()
    {
        base.Awake();
        _scroll.onValueChanged.AddListener(ScrollRectCallBack);

        FirstCreateWord();
    }

    protected override void Start()
    {
        base.Start();
        //CUtils.ShowBannerAd();
        //SetlayoutItem();
        _camera = Camera.main;
        _posEnd = _camera.ScreenToWorldPoint(posLast.position);
        _posStart = _camera.ScreenToWorldPoint(posFirst.position);

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float screenAspect = screenWidth * 1.0f / screenHeight;

        for (int i = 0; i < worldItems.Count; i++)
        {
            if (!worldItems[i].gameObject.activeInHierarchy)
            {
                worldItems[i].gameObject.SetActive(true);
            }
        }
        //var numlevels = Utils.GetNumLevels(Prefs.unlockedSubWorld, Prefs.unlockedWorld);
        if (Prefs.unlockedWorld >= _data.words.Count)
            Prefs.unlockedWorld = _data.words.Count - 1;
        target = Prefs.unlockedSubWorld + Prefs.unlockedWorld * _data.words[0].subWords.Count;
        _heightItem = (_wordItemPfb.transform as RectTransform).rect.height;
        _heightRoot = _heightItem * worldItems.Count;
        mainUI.anchoredPosition = scrollContent.anchoredPosition;
        SetPosScroll(() =>
        {
            CheckShowItem();
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SetPosScroll();
    }

    private void SetPosScroll(Action callback = null)
    {
        if (target > 0)
        {
            var sizeDeltaYItem = (worldItems[target].transform as RectTransform).sizeDelta.y;
            var contentY = mainUI.anchoredPosition.y + (sizeDeltaYItem + 30) * target;
            var result = new Vector2(_scroll.content.anchoredPosition.x, contentY);
            TweenControl.GetInstance().DelayCall(transform, 1f, () =>
            {
                _scroll.content.anchoredPosition = result;
                TweenControl.GetInstance().DelayCall(transform, 0.1f, () =>
                {
                    _scroll.content.anchoredPosition = result;
                    worldItems[target].OnButtonClick();
                    SceneAnimate.Instance.ShowTip(false);
                    callback?.Invoke();
                });
            });
        }
        else
        {
            TweenControl.GetInstance().DelayCall(transform, 1f, () =>
            {
                worldItems[0].OnButtonClick();
                SceneAnimate.Instance.ShowTip(false);
                callback?.Invoke();
            });
        }
    }
    void ScrollRectCallBack(Vector2 value)
    {
        if (value.y > 0.2f)
        {
            //for (int i = 0; i < worldItems.Count; i++)
            //{
            //    if (worldItems[i].transform.position.y < posLast.position.y)
            //    {
            //        worldItems[i].gameObject.SetActive(false);
            //    }
            //}
            verticalLayoutGroup.enabled = false;
            CheckShowItem();
        }
    }
    public void CheckShowItem()
    {
        contentSize.enabled = false;
        foreach (var item in worldItems)
        {
            var posItem = _camera.ScreenToWorldPoint(item.transform.position);
            if (posItem.y > _posEnd.y && posItem.y < _posStart.y)
                item.gameObject.SetActive(true);
            else
                item.gameObject.SetActive(false);
        }
    }

    private void ShowActiveAllWordItem()
    {
        verticalLayoutGroup.enabled = true;

        foreach (var word in worldItems)
        {
            if (!word.gameObject.activeInHierarchy)
            {
                word.gameObject.SetActive(true);
                break;
            }
        }
    }

    //private void SetlayoutItem()
    //{
    //    foreach (var item in worldItems)
    //    {
    //        item.gameObject.SetActive(true);
    //        item.gameObject.SetActive(false);
    //    }
    //    CheckShowItem();
    //}
    private void FirstCreateWord()
    {
        worldItems.Clear();
        worldItems = new List<WorldItem>();
        int tempIndex;
        for (tempIndex = 0; tempIndex < _data.words.Count; tempIndex++)
        {
            int index = tempIndex;
            var data = _data.words[tempIndex];
            int indexSub = 0;
            wordNew = tempIndex + 1;
            foreach (var sub in data.subWords)
            {
                var wordItem = Instantiate(_wordItemPfb, _root);
                wordItem.worldController = this;
                wordItem.scroll = _scroll;
                wordItem.world = index;
                wordItem.subWorld = indexSub;
                if (countItem > 9)
                    wordItem.gameObject.SetActive(false);
                worldItems.Add(wordItem);
                countItem++;
                indexSub++;
            }
        }
        var remainChapter = countChapterMax - worldItems.Count;
        if (remainChapter > 0)
        {
            for (int i = 0; i < remainChapter; i++)
            {
                var wordItem = Instantiate(_wordItemPfb, _root);
                wordItem.worldController = this;
                wordItem.itemTemp = true;
                wordItem.scroll = _scroll;
                wordItem.world = wordNew;
                wordItem.subWorld = countChapter;
                wordItem.gameObject.SetActive(false);
                worldItems.Add(wordItem);
                countChapter++;
                if (countChapter >= _data.words[0].subWords.Count - 1)
                {
                    wordNew += 1;
                    countChapter = 0;
                }
            }
        }
    }
}
