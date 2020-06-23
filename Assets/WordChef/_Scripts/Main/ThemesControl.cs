using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemesControl : MonoBehaviour
{
    public static ThemesControl instance;
    [SerializeField] private ThemesData[] _themesDatas;
    [SerializeField] private Cell cellPfb;
    [SerializeField] private Text letterTextPfb;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void LoadThemeData(int indexTheme)
    {
        CPlayerPrefs.SetInt("CURR_THEMES", indexTheme);
        var currTheme = _themesDatas[indexTheme];
        cellPfb.bg.sprite = currTheme.uiData.bgCellDone;
        cellPfb.iconCoin.sprite = currTheme.uiData.iconCoinCell;
        cellPfb.showAnsScale = currTheme.showAnsScale;
        cellPfb._spriteLetter = currTheme.uiData.bgCell;
        cellPfb._spriteLetterDone = currTheme.uiData.bgCellDone;
        letterTextPfb.font = currTheme.fontData.fontNormal;
        var bgLetter = letterTextPfb.GetComponentInChildren<Image>();
        bgLetter.sprite = currTheme.uiData.bgLetter;
        bgLetter.SetNativeSize();

        if (WordRegion.instance != null)
        {
            var wordRegion = WordRegion.instance;
            wordRegion.btnDictionary.image.sprite = currTheme.uiData.btnDictionary;
            wordRegion.btnSetting.image.sprite = currTheme.uiData.btnSetting;
            wordRegion.btnDictionary.image.SetNativeSize();
            wordRegion.btnSetting.image.SetNativeSize();
            var iconDictionary = wordRegion.btnDictionary.GetComponentInChildren<Image>();
            iconDictionary.sprite = currTheme.uiData.btnDictionary;
            iconDictionary.SetNativeSize();
            var iconSetting = wordRegion.btnSetting.GetComponentInChildren<Image>();
            iconSetting.sprite = currTheme.uiData.btnSetting;
            iconSetting.SetNativeSize();

            wordRegion.background.sprite = currTheme.uiData.background;
            wordRegion.header.sprite = currTheme.uiData.header;
            wordRegion.iconStar.sprite = currTheme.uiData.iconStar;
            wordRegion.iconAdd.sprite = currTheme.uiData.iconAdd;
            wordRegion.bgCurrency.sprite = currTheme.uiData.bgCurrency;
            wordRegion.bgLevelTitle.sprite = currTheme.uiData.bgLevelTitle;

            wordRegion.iconStar.SetNativeSize();
            wordRegion.iconAdd.SetNativeSize();
            wordRegion.bgCurrency.SetNativeSize();
            wordRegion.bgLevelTitle.SetNativeSize();

            wordRegion.shadowBonuxbox.SetActive(currTheme.uiData.showShadow);
            wordRegion.shadowHelp.SetActive(currTheme.uiData.showShadow);

            wordRegion.animBtnBonusBox.initialSkinName = currTheme.animData.skinAnim;
            wordRegion.animBtnHint.initialSkinName = currTheme.animData.skinAnim;
            wordRegion.animBtnHintTarget.initialSkinName = currTheme.animData.skinAnim;
            wordRegion.animBtnMultipleHint.initialSkinName = currTheme.animData.skinAnim;
            wordRegion.animBtnShuffle.initialSkinName = currTheme.animData.skinAnim;
            wordRegion.animBtnRewardAds.initialSkinName = currTheme.animData.skinAnim;
            wordRegion.animBtnHelp.initialSkinName = currTheme.animData.skinAnim;
            wordRegion.animBtnHelpShadow.initialSkinName = currTheme.animData.skinAnim;
            wordRegion.animBtnBonusBoxShadow.initialSkinName = currTheme.animData.skinAnim;
        }
    }
}
