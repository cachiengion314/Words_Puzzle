﻿using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonVideoHintFree : MonoBehaviour
{
    public Button _btnAds;
    private Cell _cell;
    [SerializeField] private RewardVideoController _rewardVideoPfb;
    private RewardVideoController _rewardController;

    public Cell Cell
    {
        get
        {
            return _cell;
        }
        set
        {
            _cell = value;
        }
    }

    private void Start()
    {
        _rewardController = FindObjectOfType<RewardVideoController>();
        if (_rewardController == null)
            _rewardController = Instantiate(_rewardVideoPfb);
        _rewardController.onRewardedCallback -= OnCompleteVideo;
    }

    private void OnDisable()
    {
        if (_rewardController != null)
            _rewardController.onRewardedCallback -= OnCompleteVideo;
    }

    public void OnClickOpen()
    {
        _rewardController.onRewardedCallback += OnCompleteVideo;
        AdmobController.instance.ShowRewardBasedVideo();
        Sound.instance.Play(Sound.Others.PopupOpen);
#if UNITY_EDITOR
        OnCompleteVideo();
#endif
    }

    private void OnCompleteVideo()
    {
        _rewardController.onRewardedCallback -= OnCompleteVideo;
        gameObject.SetActive(false);
        TweenControl.GetInstance().DelayCall(transform, 0.1f, () =>
        {
            Cell.ShowHint();
        });
        //_rewardController.gameObject.SetActive(true);
        CPlayerPrefs.SetBool(WordRegion.instance.keyLevel + "ADS_HINT_FREE", true);
        WordRegion.instance.SaveLevelProgress();
    }
}
