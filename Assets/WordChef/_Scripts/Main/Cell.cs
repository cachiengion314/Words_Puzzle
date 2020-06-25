using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Cell : MonoBehaviour
{
    public TextMeshProUGUI letterText;
    public Text letterTextNor;
    public Image bg;
    public Image imageCell;
    public GameObject ImgPedestal;
    public Image iconCoin;
    public Image imgHiden;
    public Button btnCellTarget;
    public GameObject fxExplode;
    public GameObject Mask;
    public string letter;
    public bool isShown;
    public bool isBee;
    public bool isAds;
    public bool showAnsScale;
    [Space]
    public Sprite _spriteLetter;
    public Sprite _spriteLetterDone;

    private Vector3 originLetterScale;

    public void Animate()
    {
        SetBgLetter(_spriteLetter);
        Mask.SetActive(true);
        bg.transform.SetParent(Mask.transform);
        ImgPedestal.SetActive(true);
        CollectStar();
        Vector3 beginPosition = TextPreview.instance.transform.position;
        originLetterScale = letterText.transform.localScale;
        Vector3 middlePoint = CUtils.GetMiddlePoint(beginPosition, transform.position, -0.3f);
        Vector3[] waypoint = { beginPosition, middlePoint, transform.position };

        ShowText();
        //letterText.transform.localScale = Vector3.one * 1.1f;
        if (!showAnsScale)
            bg.transform.localPosition = new Vector3(0, -55, 0);
        else
            letterText.transform.localScale = Vector3.zero;
        var canvasGroup = bg.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.5f;
        //letterText.transform.localPosition = new Vector3(letterText.transform.localPosition.x,
        //    letterText.transform.localPosition.y + (transform as RectTransform).sizeDelta.y / 2);
        //letterText.transform.localScale = TextPreview.instance.textGrid.transform.localScale;
        //letterText.transform.SetParent(MonoUtils.instance.textFlyTransform);
        //iTween.MoveTo(letterText.gameObject, iTween.Hash("path", waypoint, "time", 0.2f, "oncomplete", "OnMoveToComplete", "oncompletetarget", gameObject));
        //iTween.ScaleTo(letterText.gameObject, iTween.Hash("scale", originLetterScale, "time", 0.2f));
        TweenControl.GetInstance().FadeAnfa(canvasGroup, 1, 0.05f, () =>
        {
            if (!showAnsScale)
                TweenControl.GetInstance().MoveRect(bg.transform as RectTransform, new Vector3(0, -69f, 0), 0.1f, OnMoveToComplete);
            else
                TweenControl.GetInstance().ScaleFromZero(letterText.gameObject, 0.2f, OnMoveToComplete);
        });
        //TweenControl.GetInstance().Scale(letterText.gameObject,Vector3.one,0.3f);
    }

    private void CollectStar()
    {
        if (iconCoin.transform.localScale == Vector3.one)
        {
            if (MainController.instance != null)
            {
                MainController.instance.canvasFx.gameObject.SetActive(true);
                MainController.instance.canvasCollect.gameObject.SetActive(true);
            }
            if (WordRegion.instance.CurLevel >= 5 && !CPlayerPrefs.HasKey("SHOW_TUT_CELL_STAR"))
                CPlayerPrefs.SetBool("SHOW_TUT_CELL_STAR", true);
            //var canvas = iconCoin.gameObject.AddComponent<Canvas>();
            //canvas.overrideSorting = true;
            //canvas.sortingLayerName = "UI2";
            //TweenControl.GetInstance().MoveRectY(iconCoin.transform as RectTransform, bg.rectTransform.sizeDelta.y, 0.5f, () =>
            //{
            //    iconCoin.transform.SetParent(MonoUtils.instance.rootDefault);
            //    TweenControl.GetInstance().JumpRect(iconCoin.transform as RectTransform, MonoUtils.instance.posDefault.localPosition, -400f, 1, 0.5f, false, () =>
            //    {
            //        CurrencyController.CreditBalance(ConfigController.instance.config.gameParameters.rewardedBeeAmount);
            //        iconCoin.transform.localScale = Vector3.zero;
            //        //Destroy(iconCoin.GetComponent<Canvas>());
            //        MonoUtils.instance.ShowTotalStarCollect(WordRegion.instance.numStarCollect, MonoUtils.instance.textCollectDefault, 0.3f);
            //    });
            //}, EaseType.OutBack);
            //Sound.instance.Play(Sound.Collects.CoinCollect);
            MonoUtils.instance.ShowEffect(ConfigController.instance.config.gameParameters.rewardedBeeAmount, null, null, transform, WordRegion.instance.starCollectPfb, -450);
            MonoUtils.instance.ShowTotalStarCollect(WordRegion.instance.numStarCollect, MonoUtils.instance.textCollectDefault, 0.3f);
        }
    }

    private void SetBgLetter(Sprite sprite)
    {
        bg.sprite = sprite;
        //bg.SetNativeSize();
    }

    private void OnMoveToComplete()
    {
        ImgPedestal.SetActive(false);
        //letterText.transform.SetParent(transform);
        //iTween.ScaleTo(letterText.gameObject, iTween.Hash("scale", originLetterScale * 1.3f, "time", 0.15f, "oncomplete", "OnScaleUpComplete", "oncompletetarget", gameObject));
        SetBgLetter(_spriteLetterDone);
        bg.transform.SetParent(transform);
        bg.transform.localPosition = Vector3.zero;
        fxExplode.gameObject.SetActive(true);
        Mask.SetActive(false);
        TweenControl.GetInstance().DelayCall(transform, 0.15f, OnScaleUpComplete);
    }

    private void OnScaleUpComplete()
    {
        iTween.ScaleTo(letterText.gameObject, iTween.Hash("scale", originLetterScale, "time", 0.15f));
        //fxExplode.gameObject.SetActive(false);
        if (ThemesControl.instance.CurrTheme.fontData.fontScale)
            CalculateTextRaitoScale(letterTextNor.rectTransform);
        StartCoroutine(TurnOffContentsSizeFilter());
    }

    private IEnumerator TurnOffContentsSizeFilter()
    {
        yield return new WaitForEndOfFrame();
        letterTextNor.GetComponent<ContentSizeFitter>().enabled = false;
    }

    public void ShowHint()
    {
        if (isAds && WordRegion.instance.BtnADS != null)
        {
            WordRegion.instance.BtnADS.gameObject.SetActive(false);
            CPlayerPrefs.SetBool(WordRegion.instance.keyLevel + "ADS_HINT_FREE", true);
            isAds = false;
            CPlayerPrefs.SetBool(gameObject.name + "_ADS", isAds);
        }
        isShown = true;
        originLetterScale = letterText.transform.localScale;
        CollectStar();
        ShowText();
        bg.color = new Color(1, 1, 1, 0.5f);
        imgHiden.gameObject.SetActive(false);
        //TweenControl.GetInstance().KillTweener(iconCoin.transform);
        iconCoin.transform.localScale = Vector3.zero;
        OnMoveToComplete();
    }

    public void ShowTextBee()
    {
        if (isAds && WordRegion.instance.BtnADS != null)
        {
            WordRegion.instance.BtnADS.gameObject.SetActive(false);
            CPlayerPrefs.SetBool(WordRegion.instance.keyLevel + "ADS_HINT_FREE", true);
            isAds = false;
            CPlayerPrefs.SetBool(gameObject.name + "_ADS", isAds);
        }
        isShown = true;
        isBee = true;
        CPlayerPrefs.SetBool(gameObject.name, isBee);
        originLetterScale = letterText.transform.localScale;
        ShowText();
        bg.color = new Color(1, 1, 1, 0.5f);
        TweenControl.GetInstance().KillTweener(iconCoin.transform);
        iconCoin.transform.localScale = Vector3.zero;
        OnMoveToComplete();
    }

    public void ShowText()
    {
        letterText.transform.localPosition = Vector3.zero;
        letterText.text = letter;
        if (letterTextNor != null)
        {
            letterTextNor.text = letter;
            if (ThemesControl.instance.CurrTheme.fontData.fontScale)
                CalculateTextRaitoScale(letterTextNor.rectTransform);
            StartCoroutine(TurnOffContentsSizeFilter());
        }
        bg.color = new Color(1, 1, 1, 1);
        bg.gameObject.SetActive(true);
    }

    public void CalculateTextRaitoScale(RectTransform rectObj)
    {
        var widthBg = bg.sprite.rect.width;
        var heightBg = bg.sprite.rect.height;
        var valueX = bg.rectTransform.rect.width;
        var valueY = bg.rectTransform.rect.height;

        var resultX = valueX / widthBg;
        var resultY = valueY / heightBg;
        var result = resultX < resultY ? resultX : resultY;

        var ratioScale = new Vector3(result, result, result);
        rectObj.localScale = ratioScale;
    }
}
