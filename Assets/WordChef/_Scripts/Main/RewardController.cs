﻿using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardController : MonoBehaviour
{
    private bool _dontShowAgain;
    //[SerializeField] private RewardedButton _rewardedButton;
    [SerializeField] private FreeStarsDialogConfirm _boardClaim;
    [SerializeField] private GameObject _boardFreeWatch;
    [SerializeField] private Toggle _showAgain;
    [SerializeField] private int _amountStars;
    [SerializeField] private RewardVideoController _rewardVideoPfb;
    private RewardVideoController _rewardVideoControl;

    public GameObject overLay;

    void Start()
    {
        overLay.SetActive(false);
        _boardClaim.transform.localScale = Vector3.zero;
        CheckShowAgain();
    }

    private void OnDestroy()
    {
        if (_rewardVideoControl != null)
        {
            _rewardVideoControl.onRewardedCallback -= OnCompleteVideo;
        }
    }

    private void CheckShowAgain()
    {
        _showAgain.isOn = CPlayerPrefs.GetBool("DONT_SHOW", false);
    }

    public void OnShowAdsVideo()
    {
        if (ExtraWord.instance != null)
            ExtraWord.instance.effectLightLoop.gameObject.SetActive(false);
        _rewardVideoControl = FindObjectOfType<RewardVideoController>();
        if (_rewardVideoControl == null)
            _rewardVideoControl = Instantiate(_rewardVideoPfb);
        _rewardVideoControl.onRewardedCallback += OnCompleteVideo;
        _rewardVideoControl.onUpdateBtnAdsCallback += UpdateBtnAds;
        if (_showAgain.isOn)
        {
            OnWatchClick();
        }
        else
        {
            overLay.SetActive(true);
            Sound.instance.Play(Sound.Others.PopupOpen);
            TweenControl.GetInstance().ScaleFromZero(_boardFreeWatch, 0.3f);
        }
    }

    private void UpdateBtnAds(bool IsAvailable)
    {
        if(!IsAvailable)
        {
            if (ExtraWord.instance != null && _boardFreeWatch.transform.localScale == Vector3.zero)
                ExtraWord.instance.OnClaimed();
        }
    }

    private void OnCompleteVideo()
    {
        _rewardVideoControl.onRewardedCallback -= OnCompleteVideo;
        overLay.SetActive(true);
        _boardClaim.Setup(_amountStars, () =>
        {

        });
    }

    public void OnWatchClick()
    {
        overLay.SetActive(false);
        if (_boardFreeWatch.transform.localScale == Vector3.one)
            TweenControl.GetInstance().ScaleFromOne(_boardFreeWatch, 0.3f);
        TweenControl.GetInstance().DelayCall(transform, 0.1f, () =>
        {
            AdmobController.instance.ShowRewardBasedVideo();
            Sound.instance.Play(Sound.Others.PopupOpen);
        });
    }

    public void DontShowAgain()
    {
        CPlayerPrefs.SetBool("DONT_SHOW", _showAgain.isOn);
    }

    public void OnClose(GameObject obj)
    {
        if (ExtraWord.instance != null)
            ExtraWord.instance.OnClaimed();
        overLay.SetActive(false);
        Sound.instance.Play(Sound.Others.PopupClose);
        TweenControl.GetInstance().ScaleFromOne(obj, 0.3f, () =>
        {

        });
    }
}

