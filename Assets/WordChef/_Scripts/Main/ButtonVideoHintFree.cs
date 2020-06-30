﻿using GoogleMobileAds.Api;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Components;

public class ButtonVideoHintFree : MonoBehaviour
{
    public SimpleTMPButton _btnAds;
    public SkeletonGraphic animbutton;
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
        AdsManager.instance.onAdsRewarded -= OnCompleteVideo;
    }

    private void OnDisable()
    {
        if (_rewardController != null)
            _rewardController.onRewardedCallback -= OnCompleteVideo;
        if (AdsManager.instance != null)
            AdsManager.instance.onAdsClose -= OnAdsClosed;
    }

    public void OnClickOpen()
    {
        TutorialController.instance.HidenPopTut();
        _btnAds.interactable = false;
        _rewardController.onRewardedCallback += OnCompleteVideo;
        AdsManager.instance.onAdsRewarded += OnCompleteVideo;
        AdsManager.instance.onAdsClose += OnAdsClosed;
        //AdmobController.instance.ShowRewardBasedVideo();

        AudienceNetworkFbAd.instance.rewardIdFaceAds = ConfigController.instance.config.facebookAdsId.rewardedFreeLetter;
        Debug.Log("Change id succsesss");
        AdsManager.instance.ShowVideoAds();

        Sound.instance.Play(Sound.Others.PopupOpen);
#if UNITY_EDITOR
        OnCompleteVideo();
#endif
    }

    private void OnCompleteVideo()
    {
        _btnAds.interactable = true;
        _rewardController.onRewardedCallback -= OnCompleteVideo;
        AdsManager.instance.onAdsRewarded -= OnCompleteVideo;

        gameObject.SetActive(false);
        TweenControl.GetInstance().DelayCall(transform, 0.1f, () =>
        {
            var line = WordRegion.instance.Lines.Single(li => li.cells.Contains(Cell));
            var tempAnswers = line.answers;
            for (int i = 0; i < WordRegion.instance.Lines.Count; i++)
            {
                var l = WordRegion.instance.Lines[i];
                if (l != line && !l.isShown && l.answer != "")
                    tempAnswers.Remove(l.answer);
            }
            line.SetDataLetter(tempAnswers[UnityEngine.Random.Range(0, tempAnswers.Count)]);
            //line.SetDataLetter(line.answers[UnityEngine.Random.Range(0, line.answers.Count)]);
            Cell.ShowHint();
            line.CheckSetDataAnswer(line.answer);
            line.CheckLineDone();
            WordRegion.instance.SaveLevelProgress();
            WordRegion.instance.CheckGameComplete();
        });
        //_rewardController.gameObject.SetActive(true);
        CPlayerPrefs.SetBool(WordRegion.instance.keyLevel + "ADS_HINT_FREE", true);
    }

    void OnAdsClosed()
    {
        _btnAds.interactable = true;
        AdsManager.instance.onAdsClose -= OnAdsClosed;
    }
}
