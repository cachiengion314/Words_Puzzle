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
    [SerializeField] private Image starPfb;
    [SerializeField] private List<Dialog> _dialogPfb;

    private ThemesData _currTheme;

    public ThemesData CurrTheme
    {
        get
        {
            return _currTheme;
        }
    }

    public ThemesData[] ThemesDatas
    {
        get
        {
            return _themesDatas;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void LoadThemeData(int indexTheme)
    {
        //CPlayerPrefs.SetInt("CURR_THEMES", indexTheme);
        var currTheme = _themesDatas[indexTheme];

        _currTheme = currTheme;
        cellPfb.imageCell.sprite = currTheme.uiData.imgCell;
        cellPfb.imageCell.SetNativeSize();
        cellPfb.bg.sprite = currTheme.uiData.bgCellDone;
        cellPfb.iconCoin.sprite = currTheme.uiData.iconCoinCell;
        cellPfb.showAnsScale = currTheme.showAnsScale;
        cellPfb._spriteLetter = currTheme.uiData.bgCell;
        cellPfb._spriteLetterDone = currTheme.uiData.bgCellDone;
        cellPfb.letterTextNor.font = currTheme.fontData.fontNormal;
        cellPfb.letterTextNor.GetComponent<ContentSizeFitter>().enabled = currTheme.fontData.fontScale;
        if (!currTheme.fontData.fontScale)
        {
            cellPfb.letterTextNor.resizeTextForBestFit = true;
            cellPfb.letterTextNor.resizeTextMaxSize = currTheme.fontData.fontSizeMaxCell;
        }
        else
            cellPfb.letterTextNor.resizeTextForBestFit = false;
        cellPfb.letterTextNor.color = currTheme.fontData.colorCell;
        letterTextPfb.font = currTheme.fontData.fontNormal;
        letterTextPfb.color = currTheme.fontData.colorLetter;
        var bgLetter = letterTextPfb.GetComponentInChildren<Image>();
        bgLetter.sprite = currTheme.uiData.bgLetter;
        bgLetter.SetNativeSize();
        starPfb.sprite = currTheme.uiData.iconStarFly;
        starPfb.SetNativeSize();
        if (WordRegion.instance != null)
        {
            if (LineDrawer.instance != null)
            {
                var lineDrawer = LineDrawer.instance;
                lineDrawer.LineRenderer.colorGradient = currTheme.uiData.colorLinerender;
                var particale = lineDrawer.lineParticle.GetComponent<ParticleSystem>();
                var main = particale.main;
                main.startColor = new ParticleSystem.MinMaxGradient(currTheme.uiData.colorParticle, currTheme.uiData.colorParticle2);
            }
            var wordRegion = WordRegion.instance;
            wordRegion.btnDictionary.image.sprite = currTheme.uiData.btnDictionary;
            wordRegion.btnSetting.image.sprite = currTheme.uiData.btnSetting;
            wordRegion.btnDictionary.image.SetNativeSize();
            wordRegion.btnSetting.image.SetNativeSize();
            wordRegion.board.sprite = currTheme.uiData.boardWordRegion;
            wordRegion.SpriteNormal = currTheme.uiData.boardWordRegion;

            wordRegion.background.sprite = currTheme.uiData.background;
            wordRegion.header.sprite = currTheme.uiData.header;
            wordRegion.iconStar.sprite = currTheme.uiData.iconStar;
            wordRegion.iconAdd.sprite = currTheme.uiData.iconAdd;
            wordRegion.bgCurrency.sprite = currTheme.uiData.bgCurrency;
            wordRegion.bgLevelTitle.sprite = currTheme.uiData.bgLevelTitle;
            wordRegion.iconSetting.sprite = currTheme.uiData.iconSetting;
            wordRegion.iconDictionary.sprite = currTheme.uiData.iconDictionary;
            wordRegion.imageGround.sprite = currTheme.uiData.imgGround;
            wordRegion.iconHoney.sprite = currTheme.uiData.iconHoney;
            wordRegion.bgHoney.sprite = currTheme.uiData.bgHoney;

            wordRegion.textPreview.backgroundImg.sprite = currTheme.uiData.imgBgTextPreview;
            wordRegion.textPreview.textPrefab.bgCell.sprite = currTheme.uiData.imgBgCellPreview;
            wordRegion.textPreview.textPrefab.text.font = currTheme.fontData.fontNormal;
            wordRegion.textPreview.textPrefab.text.color = currTheme.fontData.colorCell;


            wordRegion.imgNumHint.sprite = currTheme.uiData.numBooster;
            wordRegion.imgNumMultipleHint.sprite = currTheme.uiData.numBooster;
            wordRegion.imgNumSelectedHint.sprite = currTheme.uiData.numBooster;
            wordRegion.imgPriceHint.sprite = currTheme.uiData.priceBooster;
            wordRegion.imgPriceMultipleHint.sprite = currTheme.uiData.priceBooster;
            wordRegion.imgPriceSelectedHint.sprite = currTheme.uiData.priceBooster;
            var starFly = wordRegion.starCollectPfb.GetComponent<Image>();
            starFly.sprite = currTheme.uiData.iconStarFly;

            //wordRegion.board.SetNativeSize();
            wordRegion.iconStar.SetNativeSize();
            wordRegion.iconAdd.SetNativeSize();
            //wordRegion.bgCurrency.SetNativeSize();
            //wordRegion.bgLevelTitle.SetNativeSize();
            wordRegion.iconSetting.SetNativeSize();
            wordRegion.iconDictionary.SetNativeSize();
            wordRegion.iconHoney.SetNativeSize();
            //wordRegion.bgHoney.SetNativeSize();

            wordRegion.imgNumHint.SetNativeSize();
            wordRegion.imgNumMultipleHint.SetNativeSize();
            wordRegion.imgNumSelectedHint.SetNativeSize();
            wordRegion.imgPriceHint.SetNativeSize();
            wordRegion.imgPriceMultipleHint.SetNativeSize();
            wordRegion.imgPriceSelectedHint.SetNativeSize();
            starFly.SetNativeSize();

            wordRegion.shadowBonuxbox.SetActive(currTheme.uiData.showShadow);
            wordRegion.shadowHelp.SetActive(currTheme.uiData.showShadow);
            //wordRegion.imgLeafTopLeft.gameObject.SetActive(currTheme.uiData.showLeaf);
            //wordRegion.imgLeafTopRight.gameObject.SetActive(currTheme.uiData.showLeaf);
            //wordRegion.imgLeafBoardWordRegion.SetActive(currTheme.uiData.showLeaf);
            wordRegion.iconLevelTitle.SetActive(currTheme.uiData.showIconLevelTitle);
            if (currTheme.uiData.levelTitleCenter)
                wordRegion._textLevel.transform.localPosition = Vector3.zero;
            wordRegion._textLevel.font = currTheme.fontData.fontAsset;
            wordRegion._textLevel.color = currTheme.fontData.colorTextHeader;
            wordRegion._textLevel.fontSizeMax = currTheme.fontData.fontSizeMax;
            wordRegion.textNumberStar.font = currTheme.fontData.fontAsset;
            wordRegion.textNumberStar.color = currTheme.fontData.colorTextHeader;
            wordRegion.textNumberStar.fontSizeMax = currTheme.fontData.fontSizeMaxNumStar;
            wordRegion.textNumHint.font = currTheme.fontData.fontAsset;
            wordRegion.textNumMultipleHint.font = currTheme.fontData.fontAsset;
            wordRegion.textNumSelectedHint.font = currTheme.fontData.fontAsset;
            if (HoneyPointsController.instance != null)
            {
                HoneyPointsController.instance.honeyTxt.font = currTheme.fontData.fontAsset;
                HoneyPointsController.instance.honeyTxt.color = currTheme.fontData.colorTextHeader;
                HoneyPointsController.instance.honeyTxt.fontSizeMax = currTheme.fontData.fontSizeMaxNumStar;
                HoneyPointsController.instance.visualHoneyPointsTxt.font = currTheme.fontData.fontAsset;
                Color currentColor = currTheme.fontData.colorWhenPlusStar;
                HoneyPointsController.instance.visualHoneyPointsTxt.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0);
            }
            if (MonoUtils.instance != null)
            {
                MonoUtils.instance.textCollectDefault.font = currTheme.fontData.fontAsset;
                Color currentColor = currTheme.fontData.colorWhenPlusStar;
                MonoUtils.instance.textCollectDefault.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0);
            }

            wordRegion.animBtnBonusBox.thisSkeletonControl.initialSkinName = currTheme.animData.skinAnim;
            wordRegion.animBtnHint.thisSkeletonControl.initialSkinName = currTheme.animData.skinAnim;
            wordRegion.animBtnHintTarget.thisSkeletonControl.initialSkinName = currTheme.animData.skinAnim;
            wordRegion.animBtnMultipleHint.thisSkeletonControl.initialSkinName = currTheme.animData.skinAnim;
            wordRegion.animBtnShuffle.thisSkeletonControl.initialSkinName = currTheme.animData.skinAnim;
            wordRegion.animBtnRewardAds.thisSkeletonControl.initialSkinName = currTheme.animData.skinAnim;
            wordRegion.animBtnHelp.thisSkeletonControl.initialSkinName = currTheme.animData.skinAnim;
            wordRegion.animBtnHelpShadow.thisSkeletonControl.initialSkinName = currTheme.animData.skinAnim;
            wordRegion.animBtnBonusBoxShadow.thisSkeletonControl.initialSkinName = currTheme.animData.skinAnim;

            wordRegion.animBtnBonusBox.SetSkin(currTheme.animData.skinAnim);
            wordRegion.animBtnHint.SetSkin(currTheme.animData.skinAnim);
            wordRegion.animBtnHintTarget.SetSkin(currTheme.animData.skinAnim);
            wordRegion.animBtnMultipleHint.SetSkin(currTheme.animData.skinAnim);
            wordRegion.animBtnShuffle.SetSkin(currTheme.animData.skinAnim);
            wordRegion.animBtnRewardAds.SetSkin(currTheme.animData.skinAnim);
            wordRegion.animBtnHelp.SetSkin(currTheme.animData.skinAnim);
            wordRegion.animBtnHelpShadow.SetSkin(currTheme.animData.skinAnim);
            wordRegion.animBtnBonusBoxShadow.SetSkin(currTheme.animData.skinAnim);
        }
    }

    public void LoadThemeDataHome(int indexTheme)
    {
        //CPlayerPrefs.SetInt("CURR_THEMES", indexTheme);
        var currTheme = _themesDatas[indexTheme];
        _currTheme = currTheme;
        starPfb.sprite = currTheme.uiData.iconStarFly;
        starPfb.SetNativeSize();
        if (HomeController.instance != null)
        {
            var homeControl = HomeController.instance;
            homeControl.BG.sprite = currTheme.uiData.bgHome;
            homeControl.imageStar.sprite = currTheme.uiData.iconStar;
            homeControl.imageAdd.sprite = currTheme.uiData.iconAdd;
            homeControl.shadowCurrency.sprite = currTheme.uiData.shadowCurrencyHome;

            homeControl.imageStar.SetNativeSize();
            homeControl.imageAdd.SetNativeSize();
            homeControl.shadowCurrency.SetNativeSize();

            homeControl.FreeBoostersShadow.SetActive(currTheme.uiData.showShadowHome);
            homeControl.chickenBankShadow.SetActive(currTheme.uiData.showShadowHome);
            homeControl.shadowCurrency.gameObject.SetActive(currTheme.uiData.showShadowHome);

            //homeControl.FreeBoostersShadow.GetComponent<SpineControl>().thisSkeletonControl.initialSkinName = currTheme.animData.skinAnim;
            //homeControl.animChickenbank.thisSkeletonControl.initialSkinName = currTheme.animData.skinAnim;
            //homeControl.animFreebooster.thisSkeletonControl.initialSkinName = currTheme.animData.skinAnim;

            //homeControl.animChickenbank.SetSkin(currTheme.animData.skinAnim);
            //homeControl.animFreebooster.SetSkin(currTheme.animData.skinAnim);
        }
    }

    public void LoadThemeDataDialog(int indexTheme)
    {
        //PlayerPrefs.SetInt("CURR_THEMES", indexTheme);
        var currTheme = _themesDatas[indexTheme];
        _currTheme = currTheme;
        starPfb.sprite = currTheme.uiData.iconStarFly;
        starPfb.SetNativeSize();

        if (_dialogPfb.Count > 0)
        {
            foreach (var dialog in _dialogPfb)
            {
                if (!dialog.isCustomTheme)
                {
                    if (dialog.bgBoard != null)
                    {
                        dialog.bgBoard.sprite = currTheme.uiData.bgBoardDialog;
                    }
                    if (dialog.imageTitle != null)
                    {
                        dialog.imageTitle.sprite = currTheme.uiData.imageTitleDialog;
                    }
                    if (dialog.btnClose != null)
                    {
                        dialog.btnClose.sprite = currTheme.uiData.btnCloseDialog;
                        dialog.btnClose.SetNativeSize();
                    }
                }
            }
        }
    }
}
