﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Cell : MonoBehaviour
{
    public TextMeshProUGUI letterText;
    public Image bg;
    public GameObject giftAds;
    public Image iconCoin;
    public GameObject fxExplode;
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
        iconCoin.transform.localScale = Vector3.zero;
        Vector3 beginPosition = TextPreview.instance.transform.position;
        originLetterScale = letterText.transform.localScale;
        Vector3 middlePoint = CUtils.GetMiddlePoint(beginPosition, transform.position, -0.3f);
        Vector3[] waypoint = { beginPosition, middlePoint, transform.position };

        ShowText();
        letterText.transform.localPosition = new Vector3(letterText.transform.localPosition.x,
            letterText.transform.localPosition.y + (transform as RectTransform).sizeDelta.y / 2);
        //letterText.transform.localScale = TextPreview.instance.textGrid.transform.localScale;
        //letterText.transform.SetParent(MonoUtils.instance.textFlyTransform);
        //iTween.MoveTo(letterText.gameObject, iTween.Hash("path", waypoint, "time", 0.2f, "oncomplete", "OnMoveToComplete", "oncompletetarget", gameObject));
        //iTween.ScaleTo(letterText.gameObject, iTween.Hash("scale", originLetterScale, "time", 0.2f));
        TweenControl.GetInstance().MoveRect(letterText.transform as RectTransform, transform.position, 0.2f, OnMoveToComplete);
    }

    private void SetBgLetter(Sprite sprite)
    {
        bg.sprite = sprite;
        bg.SetNativeSize();
    }

    private void OnMoveToComplete()
    {
        letterText.transform.SetParent(transform);
        //iTween.ScaleTo(letterText.gameObject, iTween.Hash("scale", originLetterScale * 1.3f, "time", 0.15f, "oncomplete", "OnScaleUpComplete", "oncompletetarget", gameObject));
        SetBgLetter(_spriteLetterDone);
        fxExplode.gameObject.SetActive(true);
        TweenControl.GetInstance().DelayCall(transform, 0.15f, OnScaleUpComplete);
    }

    private void OnScaleUpComplete()
    {
        iTween.ScaleTo(letterText.gameObject, iTween.Hash("scale", originLetterScale, "time", 0.15f));
        fxExplode.gameObject.SetActive(false);
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
        if (isAds)
        {
            if (WordRegion.instance.BtnADS != null)
                Destroy(WordRegion.instance.BtnADS.gameObject);
        }
        letterText.text = letter;
        bg.color = new Color(1, 1, 1, 1);
        bg.gameObject.SetActive(true);
    }

}
