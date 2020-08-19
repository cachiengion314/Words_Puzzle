using GoogleMobileAds.Api;
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

    [SerializeField] private SpineControl _animAds;

    private LineWord _lineTarget;

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
        CheckTheme();
        _lineTarget = null;
    }

    public void SetLineFreeletter()
    {
        _lineTarget = WordRegion.instance.Lines.Single(li => li.cells.Contains(Cell));
        if (_lineTarget != null)
        {
            var tempAnswers = _lineTarget.answers;
            for (int i = 0; i < WordRegion.instance.Lines.Count; i++)
            {
                var l = WordRegion.instance.Lines[i];
                if (l != _lineTarget && !l.isShown && l.answer != "")
                    tempAnswers.Remove(l.answer);
            }
            _lineTarget.SetDataLetter(tempAnswers[UnityEngine.Random.Range(0, tempAnswers.Count)]);
        }
    }

    private void CheckTheme()
    {
        if (MainController.instance != null)
        {
            var currTheme = ThemesControl.instance.CurrTheme;
            _animAds.thisSkeletonControl.initialSkinName = currTheme.animData.skinAnim;
            _animAds.SetSkin(currTheme.animData.skinAnim);
        }
    }

    private void OnEnable()
    {
        AdsManager.instance.onAdsRewarded -= OnCompleteVideo;
        AdsManager.instance.onAdsClose -= OnAdsClosed;
        AdsManager.instance.onAdsRewarded += OnCompleteVideo;
        AdsManager.instance.onAdsClose += OnAdsClosed;
    }

    private void OnDisable()
    {
        if (AdsManager.instance != null)
        {
            AdsManager.instance.onAdsClose -= OnAdsClosed;
            AdsManager.instance.onAdsRewarded -= OnCompleteVideo;
        }
    }

    private void OnDestroy()
    {
        if (AdsManager.instance != null)
        {
            AdsManager.instance.onAdsClose -= OnAdsClosed;
            AdsManager.instance.onAdsRewarded -= OnCompleteVideo;
        }
    }

    public void OnClickOpen()
    {
        TutorialController.instance.HidenPopTut();
        _btnAds.interactable = false;
        Sound.instance.Play(Sound.Others.PopupOpen);
        AdsManager.instance.ShowVideoAds(true, OnAdsClosed);
    }

    private void OnCompleteVideo()
    {
        TweenControl.GetInstance().DelayCall(transform, 0.1f, () =>
        {
            if (_lineTarget != null)
            {
                Debug.Log("Cell: " + Cell.gameObject.name);
                _btnAds.interactable = true;

                gameObject.SetActive(false);

                Cell.ShowHint();
                _lineTarget.CheckSetDataAnswer(_lineTarget.answer);
                _lineTarget.CheckLineDone();
                WordRegion.instance.SaveLevelProgress();
                WordRegion.instance.CheckGameComplete();

                Firebase.Analytics.FirebaseAnalytics.LogEvent(
                  Firebase.Analytics.FirebaseAnalytics.EventEarnVirtualCurrency,
                  new Firebase.Analytics.Parameter[] {
            new Firebase.Analytics.Parameter(
              Firebase.Analytics.FirebaseAnalytics.ParameterValue, 0),
            new Firebase.Analytics.Parameter(
              Firebase.Analytics.FirebaseAnalytics.ParameterVirtualCurrencyName, "free_letter"),
                  }
                );
            }
        });
        CPlayerPrefs.SetBool(WordRegion.instance.keyLevel + "ADS_HINT_FREE", true);
    }

    void OnAdsClosed()
    {
        _btnAds.interactable = true;
    }
}
