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
    public GameObject ImgPedestal;
    public Image iconCoin;
    public GameObject fxExplode;
    public GameObject Mask;
    public string letter;
    public bool isShown;
    public bool isBee;
    public bool isAds;
    [Space]
    public Sprite _spriteLetter;
    public Sprite _spriteLetterDone;

    private Vector3 originLetterScale;

    public void Animate()
    {
        SetBgLetter(_spriteLetter);
        bg.transform.SetParent(Mask.transform);
        ImgPedestal.SetActive(true);
        iconCoin.transform.localScale = Vector3.zero;
        Vector3 beginPosition = TextPreview.instance.transform.position;
        originLetterScale = letterText.transform.localScale;
        Vector3 middlePoint = CUtils.GetMiddlePoint(beginPosition, transform.position, -0.3f);
        Vector3[] waypoint = { beginPosition, middlePoint, transform.position };

        ShowText();
        //letterText.transform.localScale = Vector3.one * 1.1f;
        bg.transform.localPosition = new Vector3(0, -55, 0);
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
            TweenControl.GetInstance().MoveRect(bg.transform as RectTransform, new Vector3(0, -69f, 0), 0.1f, OnMoveToComplete);
        });
        //TweenControl.GetInstance().Scale(letterText.gameObject,Vector3.one,0.3f);
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
        TweenControl.GetInstance().DelayCall(transform, 0.15f, OnScaleUpComplete);
    }

    private void OnScaleUpComplete()
    {
        iTween.ScaleTo(letterText.gameObject, iTween.Hash("scale", originLetterScale, "time", 0.15f));
        //fxExplode.gameObject.SetActive(false);
        CalculateTextRaitoScale();
    }

    public void ShowHint()
    {
        isShown = true;
        originLetterScale = letterText.transform.localScale;
        ShowText();
        bg.color = new Color(1, 1, 1, 0.5f);
        iconCoin.transform.localScale = Vector3.zero;
        OnMoveToComplete();
    }

    public void ShowTextBee()
    {
        isShown = true;
        isBee = true;
        CPlayerPrefs.SetBool(gameObject.name, isBee);
        originLetterScale = letterText.transform.localScale;
        ShowText();
        bg.color = new Color(1, 1, 1, 0.5f);
        OnMoveToComplete();
    }

    public void ShowText()
    {
        letterText.transform.localPosition = Vector3.zero;
        if (isAds)
        {
            if (WordRegion.instance.BtnADS != null)
                Destroy(WordRegion.instance.BtnADS.gameObject);
        }
        letterText.text = letter;
        if (letterTextNor != null)
        {
            letterTextNor.text = letter;
            CalculateTextRaitoScale();
        }
        bg.color = new Color(1, 1, 1, 1);
        bg.gameObject.SetActive(true);
    }

    private void CalculateTextRaitoScale()
    {
        var widthBg = bg.sprite.rect.width;
        var heightBg = bg.sprite.rect.height;
        var valueX = letterTextNor.rectTransform.rect.width;
        var valueY = letterTextNor.rectTransform.rect.height;
        var ratioScale = new Vector3(valueX / widthBg, valueY / heightBg, 0);
        letterTextNor.rectTransform.localScale = ratioScale;
    }
}
