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

    private Vector2 posTarget;
    public int target;

    protected override void Awake()
    {
        base.Awake();
        CreateWord();
    }

    protected override void Start()
    {
        base.Start();
        //CUtils.ShowBannerAd();

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float screenAspect = screenWidth * 1.0f / screenHeight;
        //if (screenAspect < (9f / 16f)) title.SetActive(false);

        //UpdateUI();
        //snapScroll.InitPoints(_root.childCount);
        target = Prefs.unlockedSubWorld + Prefs.unlockedWorld * 5;
        SetPosScroll();
    }
    private void Update()
    {
        //UpdateUI();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetPosScroll();
        }
    }

    //private void UpdateUI()
    //{
    //    scrollContent.sizeDelta = new Vector2(scrollContent.sizeDelta.x, mainUI.rect.width * scrollContent.childCount);
    //    contentLayoutGroup.spacing = mainUI.rect.width;
    //}

    private void SetPosScroll()
    {
        if (target > 0)
        {
            TweenControl.GetInstance().DelayCall(transform, 1f, () =>
            {
                var spacing = 30;
                //var distance = mainUI.transform.localPosition - worldItems[target].transform.position;
                var sizeDeltaYItem = (worldItems[target].transform as RectTransform).sizeDelta.y;
                //var contentY = (distance.y - sizeDeltaYItem) / 2 - sizeDeltaYItem / 2 + spacing;
                //snapScroll.SetPage(target);
                var contentY = mainUI.anchoredPosition.y + sizeDeltaYItem * target;
                scrollContent.anchoredPosition = new Vector3(scrollContent.anchoredPosition.x, contentY, 0);
                worldItems[target].OnButtonClick();
            });
        }
        else
        {
            TweenControl.GetInstance().DelayCall(transform, 1f, () =>
            {
                worldItems[0].OnButtonClick();
            });
        }
    }

    private void CreateWord()
    {
        worldItems = new List<WorldItem>();
        for (int i = 0; i < _data.words.Count; i++)
        {
            int index = i;
            var data = _data.words[i];
            int indexSub = 0;
            foreach (var sub in data.subWords)
            {
                var wordItem = Instantiate(_wordItemPfb, _root);
                wordItem.worldController = this;
                wordItem.scroll = _scroll;
                wordItem.world = index;
                wordItem.subWorld = indexSub;
                worldItems.Add(wordItem);
                indexSub++;
            }
        }
    }
}
