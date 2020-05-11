using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenWordDictionaryDialog : Dialog
{
    [SerializeField] private Button _btnOk;
    [SerializeField] private Button _btnLater;
    [SerializeField] private Button _btnCancel;
    [SerializeField] private RewardVideoController _rewardVideoPfb;
    [SerializeField] private TextMeshProUGUI _textTitle;
    private RewardVideoController _rewardControl;

    private const string CONTENT_DEFAULT = "You need to watch a rewarded ad to see the list of words founded.";
    private const string CONTENT_NO_INTERNET = "You need internet connection to see the list of words founded.";
    private const string CONTENT_ADS_LOADED_FAILD = "This feature can not be used right now. Please try again later!";

    private void OnEnable()
    {
        _rewardControl = FindObjectOfType<RewardVideoController>();
        if (_rewardControl == null)
            _rewardControl = Instantiate(_rewardVideoPfb);
        _rewardControl.onRewardedCallback -= OnCompleteVideo;
        _rewardControl.onUpdateBtnAdsCallback += CheckBtnShowUpdate;
        _textTitle.text = CONTENT_DEFAULT;
        ShowBtnLater(false);
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
        AdmobController.instance.ShowRewardBasedVideo(() =>
        {
            ShowBtnLater(true);
        });
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
                    _textTitle.text = CONTENT_NO_INTERNET;
                    ShowBtnLater(true);
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
            DictionaryDialog.instance.currListWord.OnCompleteReward();
            Close();
        });
    }

    private void ShowBtnLater(bool show)
    {
        _btnLater.gameObject.SetActive(show);
        _btnOk.gameObject.SetActive(!show);
        _btnCancel.gameObject.SetActive(!show);
    }

}
