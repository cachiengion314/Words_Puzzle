using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldController : BaseController {
    //public RectTransform mainUI, scrollContent;
    //public HorizontalLayoutGroup contentLayoutGroup;
    //public SnapScrollRect snapScroll;

    public GameObject title;
    [HideInInspector] public List<WorldItem> worldItems;
    [SerializeField] private GameData _data;
    [SerializeField] private WorldItem _wordItemPfb;
    [SerializeField] private Transform _root;
    [SerializeField] private ScrollRect _scroll;

    protected override void Start()
    {
        base.Start();
        //CUtils.ShowBannerAd();

        //UpdateUI();
        //snapScroll.SetPage(Prefs.unlockedWorld);

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float screenAspect = screenWidth * 1.0f / screenHeight;
        //if (screenAspect < (9f / 16f)) title.SetActive(false);
        CreateWord();
    }

    //private void Update()
    //{
    //    //UpdateUI();
    //}

    //private void UpdateUI()
    //{
    //    scrollContent.sizeDelta = new Vector2(mainUI.rect.width * scrollContent.childCount, scrollContent.sizeDelta.y);
    //    contentLayoutGroup.spacing = mainUI.rect.width;
    //}

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
