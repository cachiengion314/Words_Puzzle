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
        var numlevels = Utils.GetNumLevels(Prefs.unlockedSubWorld, Prefs.unlockedWorld);
        target = Prefs.unlockedSubWorld + Prefs.unlockedWorld * numlevels;
        _heightItem = (_wordItemPfb.transform as RectTransform).rect.height;
        _heightRoot = _heightItem * worldItems.Count;
        SetPosScroll();
    }
    private void Update()
    {
        //UpdateUI();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //SetPosScroll();
            //DetecthShowChapter();
        }

        if (_isCheckItem)
            DetecthShowChapter();
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

        StartCoroutine(DelaySetPosRemainItem());
    }

    private IEnumerator DelaySetPosRemainItem()
    {
        for (int i = 11; i < worldItems.Count; i++)
        {
            var item = worldItems[i];
            item.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void CreateWord()
    {
        worldItems = new List<WorldItem>();
        var countItem = 0;
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
                if (countItem > 9)
                    wordItem.gameObject.SetActive(false);
                worldItems.Add(wordItem);
                countItem++;
                indexSub++;
            }
        }
    }

    public void DetecthShowChapter()
    {
        //_root.GetComponent<ContentSizeFitter>().enabled = false;
        //_root.GetComponent<VerticalLayoutGroup>().enabled = false;
        //_scroll.content.sizeDelta = new Vector2(_scroll.content.rect.width, _heightRoot);
        foreach (var item in worldItems)
        {
            var posItem = item.transform.position - _scroll.transform.position;
            var distance = Vector3.Distance(_scroll.transform.position, posItem);

            if (distance < 100)
                item.gameObject.SetActive(true);
            else
                item.gameObject.SetActive(false);
        }
    }
}
