using GoogleMobileAds.Api;
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
    public RectTransform animbutton;
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
            var line = WordRegion.instance.Lines.Single(li => li.cells.Contains(Cell));
            line.SetDataLetter(line.answers[UnityEngine.Random.Range(0, line.answers.Count)]);
            Cell.ShowHint();
            line.CheckLineDone();
            WordRegion.instance.SaveLevelProgress();
            WordRegion.instance.CheckGameComplete();
        });
        //_rewardController.gameObject.SetActive(true);
        CPlayerPrefs.SetBool(WordRegion.instance.keyLevel + "ADS_HINT_FREE", true);
    }
}
