using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardController : MonoBehaviour
{
    private bool _dontShowAgain;
    [SerializeField] private RewardedButton _rewardedButton;
    [SerializeField] private FreeStarsDialogConfirm _boardClaim;
    [SerializeField] private GameObject _boardFreeWatch;
    [SerializeField] private Toggle _showAgain;
    [SerializeField] private int _amountStars;
    void Start()
    {
        _boardClaim.transform.localScale = Vector3.zero;
        _rewardedButton.onRewarded += OnCompleteRewardVideo;
        CheckShowAgain();
    }

    private void CheckShowAgain()
    {
        _showAgain.isOn = CPlayerPrefs.GetBool("DONT_SHOW", false);
    }

    private void OnCompleteRewardVideo()
    {
        _rewardedButton.gameObject.SetActive(false);
        _boardClaim.Setup(_amountStars,()=> {

        });
    }

    public void OnShowAdsVideo()
    {
        if (_showAgain.isOn)
        {
            OnWatchClick();
        }
        else
        {
            Sound.instance.PlayButton();
            TweenControl.GetInstance().ScaleFromZero(_boardFreeWatch,0.3f);
        }
    }

    public void OnWatchClick()
    {
        TweenControl.GetInstance().ScaleFromOne(_boardFreeWatch, 0.3f);
        _rewardedButton.OnClick();
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

