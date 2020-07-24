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
    [SerializeField] private GameObject _panelWatch;
    //private RewardVideoController _rewardControl;

    private const string CONTENT_DEFAULT = "You need to watch a rewarded ad to see the list of words founded.";
    private const string CONTENT_NO_INTERNET = "You need internet connection to see the list of words founded.";
    private const string CONTENT_ADS_LOADED_FAILD = "This feature can not be used right now. Please try again later!";

    private void OnEnable()
    {
        //_rewardControl = FindObjectOfType<RewardVideoController>();
        //if (_rewardControl == null)
        //    _rewardControl = Instantiate(_rewardVideoPfb);
        //_rewardControl.onRewardedCallback -= OnCompleteVideo;
        //_rewardControl.onUpdateBtnAdsCallback += CheckBtnShowUpdate;
        AdsManager.instance.onAdsRewarded -= OnCompleteVideo;

        CheckShowTextTitle();
        ShowBtnLater(false);
    }

    private void CheckShowTextTitle()
    {
        CUtils.CheckConnection(this, (result) =>
        {
            if (result == 0)
            {
                _textTitle.text = CONTENT_DEFAULT;
            }
            else
            {
                _textTitle.text = CONTENT_NO_INTERNET;
                ShowBtnLater(true);
            }
        });
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

    public void OnClickOpen()
    {
        //_rewardControl.onRewardedCallback += OnCompleteVideo;
        AdsManager.instance.onAdsRewarded += OnCompleteVideo;

        AdsManager.instance.ShowVideoAds(false,LoadAdsFailed, NoInterNet);

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
            });
        });
    }

    void LoadAdsFailed()
    {
        _textTitle.text = CONTENT_ADS_LOADED_FAILD;
        ShowBtnLater(true);
    }

    void NoInterNet()
    {
        _textTitle.text = CONTENT_NO_INTERNET;
        ShowBtnLater(true);
    }

    private void OnCompleteVideo()
    {
        //_rewardControl.onRewardedCallback -= OnCompleteVideo;
        AdsManager.instance.onAdsRewarded -= OnCompleteVideo;

        //_rewardControl.onUpdateBtnAdsCallback -= CheckBtnShowUpdate;
        _panelWatch.transform.localScale = Vector3.zero;
        GetComponent<Image>().enabled = false;
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
