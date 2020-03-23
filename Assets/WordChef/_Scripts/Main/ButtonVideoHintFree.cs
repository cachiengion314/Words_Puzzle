using GoogleMobileAds.Api;
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
    //[SerializeField] private RewardController _rewardController;

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

    private void OnEnable()
    {
        if (MainController.instance.rewardVideoController != null)
            Destroy(MainController.instance.rewardVideoController.gameObject);
        MainController.instance.rewardVideoController = Instantiate(_rewardVideoPfb);
        MainController.instance.rewardVideoController.onRewardedCallback += OnCompleteVideo;
    }

    private void OnDisable()
    {
        if (MainController.instance != null)
            MainController.instance.rewardVideoController.onRewardedCallback -= OnCompleteVideo;
    }

    public void OnClickOpen()
    {
        AdmobController.instance.ShowRewardBasedVideo();
        Sound.instance.PlayButton();
    }

    private void OnCompleteVideo()
    {
        gameObject.SetActive(false);
        _cell.ShowHint();
        Destroy(MainController.instance.rewardVideoController.gameObject);
        //_rewardController.gameObject.SetActive(true);
    }
}
