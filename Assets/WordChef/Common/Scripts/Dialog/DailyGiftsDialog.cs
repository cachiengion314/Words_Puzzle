using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyGiftsDialog : Dialog
{
    [SerializeField] private RewardVideoController _rewardedVideoPfb;
    [SerializeField] private Button _btnWatch;
    [SerializeField] private Button _collectButton;
    //[SerializeField] private string _contentReward = "Completely watching 10 rewarded ads, you will get 2 Multiple Hints and 5 Hints";
    [SerializeField] private Text _currProgress;
    [SerializeField] private Text _startrogress;
    [SerializeField] private Text _endProgress;
    [SerializeField] private Text _timeCountdown;

    [SerializeField] private Slider _sliderProgress;
    [SerializeField] private int _maxProgress;
    [SerializeField] private float _valueTimeGift = 12;
    [SerializeField] private SpineControl _animChest;
    [SerializeField] private string _idleAnim = "Daily Gift";
    [SerializeField] private string _collectAnim = "Daily Gift Collect";
    [SerializeField] private string _collectLoopAnim = "Daily Collect Loop";
    [SerializeField] private Transform _posStart;

    private RewardVideoController _rewardedVideoControl;
    private const string PROGRESS_KEY = "PROGRESS";
    private const string TIME_REWARD_KEY = "TIME_REWARD";
    private const string DAY_KEY = "DAY";
    private int _currProgressValue;
    private double _timeValue;
    private bool _isReward;
    private double _sumTime;
    private double _timeTarget;

    void Awake()
    {
        InitProgress();
    }

    void Start()
    {
        CheckTimeReward();
    }

    private void InitProgress()
    {
        _timeTarget = _valueTimeGift * 3600;
        _rewardedVideoControl = FindObjectOfType<RewardVideoController>();
        if (_rewardedVideoControl == null)
            _rewardedVideoControl = Instantiate(_rewardedVideoPfb);
        _rewardedVideoControl.onRewardedCallback -= OnRewarded;
        _rewardedVideoControl.onUpdateBtnAdsCallback += CheckBtnShowUpdate;
        _currProgressValue = CPlayerPrefs.GetInt(PROGRESS_KEY, 0);
        _sliderProgress.maxValue = _maxProgress;
        UpdateProgress();
        InitTimeCountDown();
    }

    private void InitTimeCountDown()
    {
        if (!CPlayerPrefs.HasKey(DAY_KEY))
        {
            _sumTime = _timeTarget;
            CPlayerPrefs.SetDouble(DAY_KEY, _timeTarget);
        }
        else
            _sumTime = CPlayerPrefs.GetDouble(DAY_KEY);
        UpdateTimeValue();
    }

    private void ShowBtnWatch(bool show)
    {
        _btnWatch.gameObject.SetActive(show);
        _collectButton.gameObject.SetActive(!show);
    }

    public void OnClickReward()
    {
        _rewardedVideoControl.onRewardedCallback += OnRewarded;
        AdmobController.instance.ShowRewardBasedVideo();
        Sound.instance.Play(Sound.Others.PopupOpen);
#if UNITY_EDITOR
        OnRewarded();
#endif
    }

    private void CheckBtnShowUpdate(bool IsAvailableToShow)
    {
        //_rewardedButton.gameObject.SetActive(IsAvailableToShow);
    }

    void OnRewarded()
    {
        _rewardedVideoControl.onRewardedCallback -= OnRewarded;
        _rewardedVideoControl.onUpdateBtnAdsCallback -= CheckBtnShowUpdate;
        _btnWatch.interactable = false;
        TweenControl.GetInstance().DelayCall(transform, 0.1f, () =>
        {
            _currProgressValue += 1;
            if (_currProgressValue >= _maxProgress)
            {
                ShowBtnWatch(false);
            }
            CPlayerPrefs.SetInt(PROGRESS_KEY, _currProgressValue);
            UpdateProgress();
            _btnWatch.interactable = true;
        });
    }

    public void OnClickCollect()
    {
        Sound.instance.Play(Sound.Collects.LevelOpen);
        _collectButton.gameObject.SetActive(false);
        _currProgressValue = 0;
        CPlayerPrefs.SetBool(TIME_REWARD_KEY, false);
        CurrencyController.CreditHintFree(ConfigController.Config.gameParameters.rewardHintDaily);
        CurrencyController.CreditMultipleHintFree(ConfigController.Config.gameParameters.rewardMultipleHintDaily);
        _animChest.SetAnimation(_collectAnim, false, () =>
        {
            RestartCountdown();
        });
        CPlayerPrefs.SetInt(PROGRESS_KEY, _currProgressValue);
        UpdateProgress();
    }

    private void UpdateProgress()
    {
        _startrogress.text = _sliderProgress.value.ToString();
        _endProgress.text = _sliderProgress.maxValue.ToString();
        _currProgress.text = _currProgressValue.ToString();
        _sliderProgress.value = _currProgressValue;
        if (_currProgressValue >= _maxProgress)
        {
            //_animChest.SetAnimation(_collectLoopAnim, true);
            ShowBtnWatch(false);
        }
    }

    private void RestartCountdown()
    {
        //StartCoroutine(ShowEffectCollect(ConfigController.Config.rewardedVideoAmount * 10));
        //TweenControl.GetInstance().DelayCall(transform, 0.7f,()=> {
        _animChest.SetAnimation(_idleAnim, true);
        var valueTarget = (_timeTarget == _valueTimeGift * 3600) ? (_valueTimeGift * 2) * 3600 : _valueTimeGift * 3600;
        _timeTarget = valueTarget;
        CPlayerPrefs.SetDouble(DAY_KEY, _timeTarget);
        InitTimeCountDown();
        _isReward = false;
        CPlayerPrefs.SetBool(TIME_REWARD_KEY, _isReward);
        CheckTimeReward();
        //});
    }

    private IEnumerator ShowEffectCollect(int value)
    {
        for (int i = 0; i < value; i++)
        {
            if (i < 5)
            {
                MonoUtils.instance.ShowEffect(value / 5, null, null, _posStart);
            }
            yield return new WaitForSeconds(0.02f);
        }

    }

    private void CheckTimeReward()
    {
        if (CPlayerPrefs.HasKey(TIME_REWARD_KEY))
        {
            _isReward = CPlayerPrefs.GetBool(TIME_REWARD_KEY, false);
            if (!_isReward)
                StartCoroutine(CountDownTime());
            else
                ShowReward();
        }
        else
        {
            ShowReward();
            //_timeCountdown.text = _contentReward;
        }
    }

    private void ShowReward()
    {
        _timeValue = 0;
        ShowBtnWatch(true);
        _timeCountdown.transform.localScale = Vector3.zero;
    }

    private IEnumerator CountDownTime()
    {
        while (!_isReward)
        {
            _timeCountdown.transform.localScale = Vector3.one;
            TimeSpan timeSpan = TimeSpan.FromSeconds(_timeValue);
            _timeCountdown.text = timeSpan.ToString();
            if (_timeCountdown.text == "00:00:00" || _timeCountdown.text == "")
            {
                _timeCountdown.transform.localScale = Vector3.zero;
                _isReward = true;
                CPlayerPrefs.SetBool(TIME_REWARD_KEY, _isReward);
                _btnWatch.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(1);
            if (_timeValue > 0)
                UpdateTimeValue();
        }
    }

    private void UpdateTimeValue()
    {
        var timeNow = DateTime.Now.TimeOfDay.TotalSeconds;
        _timeValue = (int)(_sumTime - timeNow);
        if (_timeValue <= 0)
            _timeValue = 0;
    }

    private void OnDestroy()
    {
        _rewardedVideoControl.onUpdateBtnAdsCallback -= CheckBtnShowUpdate;
        _rewardedVideoControl.onRewardedCallback -= OnRewarded;
    }

    //TEST
    public void OnTestReward()
    {
        _timeValue = 0;
        TimeSpan timeSpan = TimeSpan.FromSeconds(_timeValue);
        _timeCountdown.text = timeSpan.ToString();
        if (_timeCountdown.text == "00:00:00")
        {
            _timeCountdown.transform.localScale = Vector3.zero;
            _isReward = true;
            CPlayerPrefs.SetBool(TIME_REWARD_KEY, _isReward);
            _rewardedVideoControl.gameObject.SetActive(true);
        }
    }
    //==
}
