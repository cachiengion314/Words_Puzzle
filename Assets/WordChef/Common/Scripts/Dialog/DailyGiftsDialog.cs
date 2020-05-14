﻿using System;
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
    [SerializeField] private SpineControl _animChest;
    [SerializeField] private string _idleAnim = "Daily Gift";
    [SerializeField] private string _collectAnim = "Daily Gift Collect";
    [SerializeField] private string _collectLoopAnim = "Daily Collect Loop";

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
    }

    void OnRewarded()
    {
        _currProgressValue += 1;
        if (_currProgressValue >= _maxProgress)
        {
            _rewardedButton.gameObject.SetActive(false);
            _currProgressValue = 0;
            CPlayerPrefs.SetBool(TIME_REWARD_KEY, false);
            _animChest.SetAnimation(_collectAnim, false, () => {
                RestartCountdown();
            });
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
        //if (_currProgressValue >= _maxProgress)
        //{
        //    _animChest.SetAnimation(_collectAnim, false, () => {
        //        _animChest.SetAnimation(_collectLoopAnim, true);
        //    });
        //}
    }

    private void RestartCountdown()
    {
        _animChest.SetAnimation(_idleAnim,true);
        StartCoroutine(ShowEffectCollect(ConfigController.Config.rewardedVideoAmount * 10));
        var valueTarget = (_timeTarget == _valueTimeGift * 3600) ? (_valueTimeGift * 2) * 3600 : _valueTimeGift * 3600;
        _timeTarget = valueTarget;
        CPlayerPrefs.SetDouble(DAY_KEY, _timeTarget);
        InitTimeCountDown();
        _isReward = false;
        CPlayerPrefs.SetBool(TIME_REWARD_KEY, _isReward);
        CheckTimeReward();
    }

    private IEnumerator ShowEffectCollect(int value)
    {
        var tweenControl = TweenControl.GetInstance();
        for (int i = 0; i < value; i++)
        {
            if (i < 5)
            {
                var star = Instantiate(MonoUtils.instance.rubyFly, MonoUtils.instance.textFlyTransform);
                star.transform.position = Vector3.zero;
                tweenControl.Move(star.transform, GameObject.FindGameObjectWithTag("RubyBalance").transform.position, 0.5f, () =>
                {
                    CurrencyController.CreditBalance(value / 4);
                    Sound.instance.Play(Sound.Collects.CoinCollect);
                    Destroy(star);
                }, EaseType.InBack);
            }
            yield return new WaitForSeconds(0.02f);
        }

    }

    private void CheckTimeReward()
    {
        _isReward = CPlayerPrefs.GetBool(TIME_REWARD_KEY, false);
        if (!_isReward)
            StartCoroutine(CountDownTime());
        else
        {
            _timeValue = 0;
            _rewardedButton.gameObject.SetActive(true);
            _rewardedButton.content.SetActive(true);
            _timeCountdown.transform.localScale = Vector3.zero;
        }
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
                _rewardedButton.gameObject.SetActive(true);
                _rewardedButton.content.SetActive(true);
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
            _rewardedButton.gameObject.SetActive(true);
        }
    }
    //==
}
