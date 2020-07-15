using Superpow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldController : BaseController
{
    public RectTransform mainUI, scrollContent;
    //public VerticalLayoutGroup contentLayoutGroup;
    //public SnapScrollRect snapScroll;

    public GameObject title;
    [HideInInspector] public List<WorldItem> worldItems;
    [SerializeField] private GameData _data;
    [SerializeField] private WorldItem _wordItemPfb;
    [SerializeField] private Transform _root;
    [SerializeField] private ScrollRect _scroll;

    private bool _isCheckItem;
    private float _heightItem;
    private float _heightRoot;
    private Vector2 posTarget;
    public int target;
    [SerializeField] private RectTransform posFirst;
    [SerializeField] private RectTransform posLast;

    private int countItem = 0;
    private int countChapter;
    private int wordNew;

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

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float screenAspect = screenWidth * 1.0f / screenHeight;

        //var numlevels = Utils.GetNumLevels(Prefs.unlockedSubWorld, Prefs.unlockedWorld);
        if (Prefs.unlockedWorld >= _data.words.Count)
            Prefs.unlockedWorld = _data.words.Count - 1;
        target = Prefs.unlockedSubWorld + Prefs.unlockedWorld * _data.words[0].subWords.Count;
        _heightItem = (_wordItemPfb.transform as RectTransform).rect.height;
        _heightRoot = _heightItem * worldItems.Count;
        mainUI.anchoredPosition = scrollContent.anchoredPosition;
        SetPosScroll();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SetPosScroll();
    }

    private void SetPosScroll()
    {
        if (target > 0)
        {
            var sizeDeltaYItem = (worldItems[target].transform as RectTransform).sizeDelta.y;
            var contentY = mainUI.anchoredPosition.y + (sizeDeltaYItem +30) * target;
            var result = new Vector2(_scroll.content.anchoredPosition.x, contentY);
            TweenControl.GetInstance().DelayCall(transform, 1f, () =>
            {
                _scroll.content.anchoredPosition = result;
                TweenControl.GetInstance().DelayCall(transform, 0.1f, () =>
                {
                    _scroll.content.anchoredPosition = result;
                    worldItems[target].OnButtonClick();
                    SceneAnimate.Instance.ShowTip(false);
                });
            });
        }
        else
        {
            TweenControl.GetInstance().DelayCall(transform, 1f, () =>
            {
                worldItems[0].OnButtonClick();
                SceneAnimate.Instance.ShowTip(false);
            });
        }
    }

    void ScrollRectCallBack(Vector2 value)
    {
        if (value.y <= 0.1f)
        {
            //var wordItem = Instantiate(_wordItemPfb, _root);
            //wordItem.worldController = this;
            //wordItem.itemTemp = true;
            //wordItem.scroll = _scroll;
            //wordItem.world = wordNew;
            //wordItem.subWorld = countChapter;
            //worldItems.Add(wordItem);
            //countChapter++;
            //if (countChapter >= _data.words[0].subWords.Count - 1)
            //{
            //    wordNew += 1;
            //    countChapter = 0;
            //}
            foreach (var item in worldItems)
            {
                if (!item.gameObject.activeInHierarchy)
                {
                    item.gameObject.SetActive(true);
                    break;
                }
            }
        }
        CheckShowItem();
    }

    private void CheckShowItem()
    {
        foreach (var item in worldItems)
        {
            if (item.transform.position.y < posLast.position.y)
                item.gameObject.SetActive(false);
            else
                item.gameObject.SetActive(true);
        }
    }

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
    }
}
