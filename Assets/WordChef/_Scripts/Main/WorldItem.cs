using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WorldItem : MonoBehaviour {
    public MaskableGraphic itemNumber, itemNumberBack;
    public Image play, star;
    public Button button;
    public Sprite playIng;
    public Sprite playUnactive;
    public Text itemName, processText, subWorldName;
    public LevelItem levelItemPrefab;
    public RectTransform levelGrid;

    public int world, subWorld;
    int unlockedWorld, unlockedSubWorld, unlockedLevel;

    public ScrollRect scroll;

    private void Start()
    {
        itemName.text = "CHAP " + (transform.GetSiblingIndex() + 1);

        //world = transform.parent.parent.GetSiblingIndex();
        //subWorld = transform.GetSiblingIndex();
        int numLevels = Superpow.Utils.GetNumLevels(world, subWorld);

         unlockedWorld = Prefs.unlockedWorld;
         unlockedSubWorld = Prefs.unlockedSubWorld;
         unlockedLevel = Prefs.unlockedLevel;

        //Load level
        for (int i = 0; i < numLevels; i++)
        {
            LevelItem levelButton = Instantiate(levelItemPrefab);
            levelButton.world = world;
            levelButton.subWorld = subWorld;
            levelButton.level = i;
            levelButton.transform.SetParent(levelGrid);
            levelButton.transform.localScale = Vector3.one;
            levelButton.transform.SetLocalZ(0);
        }

        if (world > unlockedWorld || (world == unlockedWorld && subWorld > unlockedSubWorld))
        {
           // button.interactable = false;
            play.sprite = playUnactive;

            processText.text = "0" + "/" + numLevels;
            star.gameObject.SetActive(false);

            levelGrid.gameObject.SetActive(false);
        }
        else if (world == unlockedWorld && subWorld == unlockedSubWorld)
        {
            play.sprite = playIng;
            processText.text = unlockedLevel + "/" + numLevels;
            star.gameObject.SetActive(true);

            levelGrid.gameObject.SetActive(false);
            //levelGrid.gameObject.SetActive(true);
            scroll.DOVerticalNormalizedPos(1f - ((float)transform.GetSiblingIndex() / (float)transform.parent.childCount), 0f);
        }
        else
        {
            processText.text = numLevels + "/" + numLevels;
            star.gameObject.SetActive(true);

            levelGrid.gameObject.SetActive(false);
        }

        button.onClick.AddListener(OnButtonClick);
    }

    private void SetColorAlpha(MaskableGraphic graphic, float alpha)
    {
        graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);
    }

    private void OnButtonClick()
    {

        if (world > unlockedWorld || (world == unlockedWorld && subWorld > unlockedSubWorld))
        {
            
        }
        else {
            GameState.currentSubWorldName = subWorldName.text;

            levelGrid.gameObject.SetActive(!levelGrid.gameObject.activeSelf);

            if (levelGrid.gameObject.activeSelf)
            {
                if (scroll.verticalNormalizedPosition <= 0.05f) scroll.DOVerticalNormalizedPos(0f, 0.5f);
            }
            Sound.instance.PlayButton();
        }
    }
}
