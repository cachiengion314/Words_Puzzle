using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldController : BaseController {
    //public RectTransform mainUI, scrollContent;
    //public HorizontalLayoutGroup contentLayoutGroup;
    //public SnapScrollRect snapScroll;

    public GameObject title;


    protected override void Start()
    {
        base.Start();
        //CUtils.ShowBannerAd();

        //UpdateUI();
        //snapScroll.SetPage(Prefs.unlockedWorld);

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float screenAspect = screenWidth * 1.0f / screenHeight;
        if (screenAspect < (9f / 16f)) title.SetActive(false);
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
}
