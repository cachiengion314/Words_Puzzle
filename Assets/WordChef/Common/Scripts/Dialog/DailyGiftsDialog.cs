using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyGiftsDialog : Dialog
{
    [SerializeField] private RewardedButton _rewardedButton;
    [SerializeField] private Text _currProgress;
    [SerializeField] private Text _startrogress;
    [SerializeField] private Text _endProgress;
    [SerializeField] private Text _timeCountdown;

    [SerializeField] private Slider _sliderProgress;
    [SerializeField] private int _maxProgress;
    [SerializeField] private float _valueTimeGift = 12;

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
        _timeTarget = DateTime.Now.TimeOfDay.TotalSeconds + (_valueTimeGift * 3600);
        _rewardedButton.gameObject.SetActive(false);
        _rewardedButton.onRewarded += OnRewarded;
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
        TimeSpan timeSpan = TimeSpan.FromSeconds(_timeValue);
        _timeCountdown.text = timeSpan.ToString();
    }

    void OnRewarded()
    {
        _currProgressValue += 1;
        if (_currProgressValue >= _maxProgress)
        {
            _rewardedButton.gameObject.SetActive(false);
            _currProgressValue = 0;
            CPlayerPrefs.SetBool(TIME_REWARD_KEY, false);
            RestartCountdown();
        }
        CPlayerPrefs.SetInt(PROGRESS_KEY, _currProgressValue);
        UpdateProgress();
    }

    private void UpdateProgress()
    {
        _startrogress.text = _sliderProgress.value.ToString();
        _endProgress.text = _sliderProgress.maxValue.ToString();
        _currProgress.text = _currProgressValue.ToString();
        _sliderProgress.value = _currProgressValue;
    }

    private void RestartCountdown()
    {
        CPlayerPrefs.SetDouble(DAY_KEY, _timeTarget);
        InitTimeCountDown();
        _isReward = false;
        CPlayerPrefs.SetBool(TIME_REWARD_KEY, _isReward);
        CheckTimeReward();
    }

    private void CheckTimeReward()
    {
        _isReward = CPlayerPrefs.GetBool(TIME_REWARD_KEY, false);
        if (!_isReward)
            StartCoroutine(CountDownTime());
        else
            _rewardedButton.gameObject.SetActive(true);
    }

    private IEnumerator CountDownTime()
    {
        while (!_isReward)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(_timeValue);
            _timeCountdown.text = timeSpan.ToString();
            if (_timeCountdown.text == "00:00:00")
            {
                _isReward = true;
                CPlayerPrefs.SetBool(TIME_REWARD_KEY, _isReward);
                _rewardedButton.gameObject.SetActive(true);
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
}
