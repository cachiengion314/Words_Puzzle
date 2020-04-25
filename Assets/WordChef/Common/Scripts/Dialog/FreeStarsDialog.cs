using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreeStarsDialog : Dialog
{
    [SerializeField] private Button _btnWatch;
    [SerializeField] private RewardVideoController _rewardVideoPfb;
    private RewardVideoController _rewardControl;

    private void OnEnable()
    {
        _rewardControl = FindObjectOfType<RewardVideoController>();
        if (_rewardControl == null)
            _rewardControl = Instantiate(_rewardVideoPfb);
        _rewardControl.onRewardedCallback -= OnCompleteVideo;
        _rewardControl.onUpdateBtnAdsCallback += CheckBtnShowUpdate;
    }

    private void CheckBtnShowUpdate(bool IsAvailableToShow)
    {
        //_btnWatch.gameObject.SetActive(IsAvailableToShow);
    }

    private void OnDestroy()
    {
        if (_rewardControl != null)
        {
            _rewardControl.onRewardedCallback -= OnCompleteVideo;
            _rewardControl.onUpdateBtnAdsCallback -= CheckBtnShowUpdate;
        }
    }

    public void OnClickOpen()
    {
        _rewardControl.onRewardedCallback += OnCompleteVideo;
        AdmobController.instance.ShowRewardBasedVideo();
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
        _rewardControl.onRewardedCallback -= OnCompleteVideo;
        _rewardControl.onUpdateBtnAdsCallback -= CheckBtnShowUpdate;
        TweenControl.GetInstance().DelayCall(transform, 0.1f, () =>
        {
            Sound.instance.Play(Sound.Others.PopupOpen);
            DialogController.instance.ShowDialog(DialogType.RewardedVideo, DialogShow.REPLACE_CURRENT);
        });
    }

}
