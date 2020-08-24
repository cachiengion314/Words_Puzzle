using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using EnhancedUI.EnhancedScroller;

public class WorldItem : EnhancedScrollerCellView
{
    [HideInInspector] public bool itemTemp;
    public GameObject rootBg;
    public MaskableGraphic itemNumber, itemNumberBack;
    public Image play, star, bg;
    public Button button;
    public Sprite spriteBgLock;
    public Sprite spriteBgUnlock;
    public Sprite playIng;
    public Sprite playUnactive;
    public Sprite playClear;
    public Text itemName, processText, subWorldName;
    public LevelItem levelItemPrefab;
    public RectTransform levelGrid;
    public Color colorTextLock;
    public Color colorTextUnLock;
    public int world, subWorld, totalSubword;
    int unlockedWorld, unlockedSubWorld, unlockedLevel;

    public LayoutElement layoutElement;

    public VerticalLayoutGroup levelGridVerticalLayout;
    public VerticalLayoutGroup thisVerticalLayout;

    [HideInInspector] public WorldController worldController;

    private float _sizeItemOpen;

    private void Start()
    {
        layoutElement = gameObject.GetComponent<LayoutElement>();
        _sizeItemOpen = bg.rectTransform.sizeDelta.y + levelGrid.sizeDelta.y - (levelGridVerticalLayout.spacing - thisVerticalLayout.spacing);
    }

    public void Setup()
    {
        button.interactable = true;
        itemName.text = "CHAPTER " + (/*transform.GetSiblingIndex()*/subWorld + world * totalSubword + 1);

        //world = transform.parent.parent.GetSiblingIndex();
        //subWorld = transform.GetSiblingIndex();
        int numLevels = 0;
        unlockedWorld = Prefs.unlockedWorld;
        unlockedSubWorld = Prefs.unlockedSubWorld;
        unlockedLevel = Prefs.unlockedLevel;

        if (world > unlockedWorld || (world == unlockedWorld && subWorld > unlockedSubWorld))
        {
            button.interactable = false;
            SetStateWord(playUnactive, spriteBgLock, colorTextLock, "0" + "/" + numLevels);
            itemNumberBack.gameObject.SetActive(false);
            //star.gameObject.SetActive(false);
            levelGrid.gameObject.SetActive(false);
        }
        else if (world == unlockedWorld && subWorld == unlockedSubWorld)
        {
            SetStateWord(playIng, spriteBgUnlock, colorTextUnLock, unlockedLevel + "/" + numLevels);
            //star.gameObject.SetActive(true);
            levelGrid.gameObject.SetActive(false);
            //levelGrid.gameObject.SetActive(true);
            //scroll.DOVerticalNormalizedPos(1f - ((float)transform.GetSiblingIndex() / (float)transform.parent.childCount), 0f);
        }
        else
        {
            SetStateWord(playClear, spriteBgUnlock, colorTextUnLock, "Clear");
            //star.gameObject.SetActive(true);
            levelGrid.gameObject.SetActive(false);
        }

        //button.onClick.AddListener(OnButtonClick);
    }

    private void SetStateWord(Sprite spritePlay, Sprite spriteBG, Color color, string processContent = "")
    {
        play.sprite = spritePlay;
        bg.sprite = spriteBG;
        bg.SetNativeSize();
        play.SetNativeSize();

        processText.text = processContent;
        subWorldName.color = color;
    }

    private void SetColorAlpha(MaskableGraphic graphic, float alpha)
    {
        graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);
    }

    private void CloseAllChapter()
    {
        foreach (var word in worldController.worldItems)
        {
            if (word != this && word.levelGrid.gameObject.activeInHierarchy)
            {
                word.OnButtonClick();
            }
        }
    }

    public void OnButtonClick()
    {
        worldController.verticalLayoutGroup.enabled = true;
        if (!itemTemp)
        {
            var numLevels = Superpow.Utils.GetNumLevels(0, 0);
            var numLevelReality = Superpow.Utils.GetNumLevels(world, subWorld);
            if (levelGrid.childCount <= 0)
            {
                //Load level
                for (int i = 0; i < numLevelReality; i++)
                {
                    LevelItem levelButton = Instantiate(levelItemPrefab);
                    levelButton.numlevels = numLevels;
                    levelButton.world = world;
                    levelButton.subWorld = subWorld;
                    levelButton.level = i;
                    levelButton.transform.SetParent(levelGrid);
                    levelButton.transform.localScale = Vector3.one;
                    levelButton.transform.SetLocalZ(0);
                }
            }
        }
        //worldController.scrollContent.GetComponent<VerticalLayoutGroup>().enabled = true;
        //worldController.scrollContent.GetComponent<ContentSizeFitter>().enabled = true;
        if (world > unlockedWorld || (world == unlockedWorld && subWorld > unlockedSubWorld))
        {

        }
        else
        {
            CloseAllChapter();
            GameState.currentSubWorldName = subWorldName.text;

            levelGrid.gameObject.SetActive(!levelGrid.gameObject.activeSelf);

            if (levelGrid.gameObject.activeSelf)
            {
                layoutElement.minHeight = _sizeItemOpen;
                //if (scroll.verticalNormalizedPosition <= 0.05f) scroll.DOVerticalNormalizedPos(0f, 0.1f);
            }
            else
                layoutElement.minHeight = bg.rectTransform.sizeDelta.y;
            Sound.instance.Play(Sound.Others.PopupOpen);
        }
    }
}
