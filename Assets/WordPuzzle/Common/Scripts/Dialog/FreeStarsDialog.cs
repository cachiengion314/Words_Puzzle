﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FreeStarsDialog : Dialog
{
    [SerializeField] private Button _btnWatch;
    [SerializeField] private RewardVideoController _rewardVideoPfb;
    [SerializeField] private GameObject _panelWatch;
    [Header("Theme UI Change")]
    [SerializeField] private Image _iconAds;
    [SerializeField] private Image _iconStar;
    [SerializeField] private Text _txtReward;
    [SerializeField] private TextMeshProUGUI _txtMessage;
    [SerializeField] private SpineControl _animCharacter;

    //private RewardVideoController _rewardControl;

    protected override void Start()
    {
        base.Start();
        CheckTheme();
    }

    private void OnEnable()
    {
        //_rewardControl = FindObjectOfType<RewardVideoController>();
        //if (_rewardControl == null)
        //    _rewardControl = Instantiate(_rewardVideoPfb);

        //_rewardControl.onRewardedCallback -= OnCompleteVideo;
        //_rewardControl.onUpdateBtnAdsCallback += CheckBtnShowUpdate;

        AdsManager.instance.onAdsRewarded -= OnCompleteVideo;
    }

    private void CheckBtnShowUpdate(bool IsAvailableToShow)
    {
        //_btnWatch.gameObject.SetActive(IsAvailableToShow);
    }

    private void OnDestroy()
    {
        //if (_rewardControl != null)
        //{
        //    _rewardControl.onRewardedCallback -= OnCompleteVideo;
        //    _rewardControl.onUpdateBtnAdsCallback -= CheckBtnShowUpdate;
        //}
        AdsManager.instance.onAdsRewarded -= OnCompleteVideo;
    }

    private void CheckTheme()
    {
        if (MainController.instance != null)
        {
            var currTheme = ThemesControl.instance.CurrTheme;
            _btnWatch.image.sprite = currTheme.uiData.freestarData.btnWatch;
            _iconAds.sprite = currTheme.uiData.freestarData.iconAds;
            _iconStar.sprite = currTheme.uiData.freestarData.iconStar;

            _btnWatch.image.SetNativeSize();
            _iconAds.SetNativeSize();
            _iconStar.SetNativeSize();

            _txtReward.color = currTheme.uiData.freestarData.colorTextBtn;
            _txtMessage.color = currTheme.fontData.colorContentDialog;

            _animCharacter.thisSkeletonControl.initialSkinName = currTheme.animData.skinAnim;
            _animCharacter.SetSkin(currTheme.animData.skinAnim);
        }
    }

    public void OnClickOpen()
    {
        //_rewardControl.onRewardedCallback += OnCompleteVideo;
        AdsManager.instance.onAdsRewarded += OnCompleteVideo;

        AudienceNetworkFbAd.instance.rewardIdFaceAds = ConfigController.instance.config.facebookAdsId.rewardedFreeStars;
        UnityAdTest.instance.myPlacementId = ConfigController.instance.config.unityAdsId.rewardedFreeStars;
        AdmobController.instance.videoAdsId = ConfigController.instance.config.admob.rewardedFreeStars;

        AdsManager.instance.ShowVideoAds();
        //AdmobController.instance.ShowRewardBasedVideo();

        Sound.instance.audioSource.Stop();
        Sound.instance.Play(Sound.Others.PopupOpen);
        TweenControl.GetInstance().DelayCall(transform, 0.1f, () =>
        {
            CUtils.CheckConnection(this, (result) =>
            {
                if (result == 0)
                {
#if UNITY_EDITOR
                    OnCompleteVideo();
#endif
                }
                else
                {
                    Close();
                }
            });
        });
    }

    private void OnCompleteVideo()
    {
        Debug.Log("OnCompleteVideo freestar invoke");
        AdsManager.instance.onAdsRewarded -= OnCompleteVideo;
        //_rewardControl.onRewardedCallback -= OnCompleteVideo;
        //_rewardControl.onUpdateBtnAdsCallback -= CheckBtnShowUpdate;
        _panelWatch.transform.localScale = Vector3.zero;
        TweenControl.GetInstance().DelayCall(transform, 0.5f, () =>
        {
            Sound.instance.Play(Sound.Others.PopupOpen);
            DialogController.instance.ShowDialog(DialogType.RewardedVideo, DialogShow.REPLACE_CURRENT);
        });
        Firebase.Analytics.FirebaseAnalytics.LogEvent(
          Firebase.Analytics.FirebaseAnalytics.EventEarnVirtualCurrency,
          new Firebase.Analytics.Parameter[] {
            new Firebase.Analytics.Parameter(
              Firebase.Analytics.FirebaseAnalytics.ParameterValue, 20),
            new Firebase.Analytics.Parameter(
              Firebase.Analytics.FirebaseAnalytics.ParameterVirtualCurrencyName, "free_stars_shop"),
          }
        );
    }

}
