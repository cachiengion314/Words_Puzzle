using GoogleMobileAds.Api;
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

    void Start()
    {
        _boardClaim.transform.localScale = Vector3.zero;
        CheckShowAgain();
    }

    private void OnDisable()
    {
        if (MainController.instance != null)
            MainController.instance.rewardVideoController.onRewardedCallback -= OnCompleteVideo;
    }

    private void CheckShowAgain()
    {
        _showAgain.isOn = CPlayerPrefs.GetBool("DONT_SHOW", false);
    }

    public void OnShowAdsVideo()
    {
        if (MainController.instance.rewardVideoController != null)
            Destroy(MainController.instance.rewardVideoController.gameObject);
        MainController.instance.rewardVideoController = Instantiate(_rewardVideoPfb);
        MainController.instance.rewardVideoController.onRewardedCallback += OnCompleteVideo;
        if (_showAgain.isOn)
        {
            OnWatchClick();
        }
        else
        {
            Sound.instance.PlayButton();
            TweenControl.GetInstance().ScaleFromZero(_boardFreeWatch, 0.3f);
        }
    }

    private void OnCompleteVideo()
    {
        Destroy(MainController.instance.rewardVideoController.gameObject);
        _boardClaim.Setup(_amountStars, () =>
        {

        });
    }

    public void OnWatchClick()
    {
        if (_boardFreeWatch.transform.localScale == Vector3.one)
            TweenControl.GetInstance().ScaleFromOne(_boardFreeWatch, 0.3f);
        TweenControl.GetInstance().DelayCall(transform, 0.1f, () =>
        {
            AdmobController.instance.ShowRewardBasedVideo();
            Sound.instance.PlayButton();
        });
    }

    public void DontShowAgain()
    {
        CPlayerPrefs.SetBool("DONT_SHOW", _showAgain.isOn);
    }

    public void OnClose(GameObject obj)
    {
        Sound.instance.PlayButton();
        TweenControl.GetInstance().ScaleFromOne(obj, 0.3f, () =>
        {

        });
    }
}

