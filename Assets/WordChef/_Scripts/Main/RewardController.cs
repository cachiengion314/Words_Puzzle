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
        CheckShowAgain();
        _rewardVideoControl = FindObjectOfType<RewardVideoController>();
        if (_rewardVideoControl == null)
            _rewardVideoControl = Instantiate(_rewardVideoPfb);
        if (_showAgain.isOn)
        {
            OnWatchClick();
        }
        else
        {
            //overLay.SetActive(true);
            //Sound.instance.Play(Sound.Others.PopupOpen);
            //TweenControl.GetInstance().ScaleFromZero(_boardFreeWatch, 0.3f);
            Sound.instance.Play(Sound.Others.PopupOpen);
            DialogController.instance.ShowDialog(DialogType.FreeStarsPlay, DialogShow.REPLACE_CURRENT);
        }
    }


    private void OnCompleteVideo()
    {
        _rewardVideoControl.onRewardedCallback -= OnCompleteVideo;
        //overLay.SetActive(true);
        if (_boardFreeWatch.transform.localScale == Vector3.one)
            _boardFreeWatch.transform.localScale = Vector3.zero;
        //_boardClaim.Setup(_amountStars, () =>
        //{

        //});
        TweenControl.GetInstance().DelayCall(transform, 0.5f, () =>
        {
            Sound.instance.Play(Sound.Others.PopupOpen);
            DialogController.instance.ShowDialog(DialogType.RewardedVideo, DialogShow.REPLACE_CURRENT);
        });
    }

    public void OnWatchClick()
    {
        _rewardVideoControl.onRewardedCallback += OnCompleteVideo;
        //overLay.SetActive(false);
        TweenControl.GetInstance().DelayCall(transform, 0.1f, () =>
        {
            AdsManager.instance.ShowVideoAds();
            Sound.instance.Play(Sound.Others.PopupOpen);
#if UNITY_EDITOR
            OnCompleteVideo();
#endif
        });
    }

    public void DontShowAgain()
    {
        CPlayerPrefs.SetBool("DONT_SHOW", _showAgain.isOn);
    }

    public void OnClose(GameObject obj)
    {
        _rewardVideoControl.onRewardedCallback -= OnCompleteVideo;
        if (ExtraWord.instance != null)
            ExtraWord.instance.OnClaimed();
        
        Sound.instance.Play(Sound.Others.PopupClose);
        TweenControl.GetInstance().ScaleFromOne(obj, 0.3f, () =>
        {
            overLay.SetActive(false);
        });
    }
}

