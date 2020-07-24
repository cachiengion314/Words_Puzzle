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
    [SerializeField] private GameObject _btnAdsDisable;
    //[SerializeField] private string _contentReward = "Completely watching 10 rewarded ads, you will get 2 Multiple Hints and 5 Hints";
    [SerializeField] private Text _currProgress;
    [SerializeField] private Text _startrogress;
    [SerializeField] private Text _endProgress;
    [SerializeField] private Text _timeCountdown;
    [SerializeField] private Text _notifiCheckAds;

    [SerializeField] private Slider _sliderProgress;
    [SerializeField] private int _maxProgress;
    [SerializeField] private float _valueTimeGift = 12;
    [SerializeField] private SpineControl _animChest;
    [SerializeField] private string _idleAnim = "Daily Gift";
    [SerializeField] private string _collectAnim = "Daily Gift Collect";
    [SerializeField] private string _collectLoopAnim = "Daily Collect Loop";
    [SerializeField] private Transform _posStart;
    [SerializeField] private GameObject _overlayCollect;
    [SerializeField] private TextMeshProUGUI _textHintCollect;
    [SerializeField] private TextMeshProUGUI _textMultipleHintCollect;
    [SerializeField] private TextMeshProUGUI _textSelectedHintCollect;
    [SerializeField] private TextMeshProUGUI _textNotifyCollect;
    [SerializeField] private TextMeshProUGUI _textNotifyTitle;
    [SerializeField] private GameObject _fxLightPfb;
    [SerializeField] private Transform _posChest;

    private GameObject _fxEffect;
    //private RewardVideoController _rewardedVideoControl;
    private const string PROGRESS_KEY = "PROGRESS";
    private const string TIME_REWARD_KEY = "TIME_REWARD";
    private const string NEXT_DAY_KEY = "NEXTDAY";
    private const string TIME_NEXT_DAY_KEY = "TIME_NEXTDAY";
    private int _currProgressValue;
    private double _timeValue;
    private bool _isReward;
    private DateTime nextDay;

    private List<int> listRandomMultiple = new List<int>();
    private List<int> listRandomSelected = new List<int>();
    private List<int> listRandomHint = new List<int>();

    private const string CLOSE_CHEST = "Complete watching 10 rewarded ads, you can get";
    private const string OPEN_CHEST = "Here are your rewards.";

    void Awake()
    {
        InitProgress();
        PlayerPrefs.SetInt("Max_FreeBooster_Progress", _maxProgress);
    }

    void Start()
    {
        base.Start();
        if (!AdsManager.instance.AdsIsLoaded(false))
            AdsManager.instance.LoadDataAds();
        InitListRandomMultipleHint();
        InitListRandomSelectedHint();
        InitListRandomHint();
        CheckTimeReward();
    }

    private void InitProgress()
    {
        //_rewardedVideoControl = FindObjectOfType<RewardVideoController>();
        //if (_rewardedVideoControl == null)
        //    _rewardedVideoControl = Instantiate(_rewardedVideoPfb);
        //_rewardedVideoControl.onRewardedCallback -= OnRewarded;
        //_rewardedVideoControl.onRewardedCallback += OnRewarded;

        AdsManager.instance.onAdsRewarded -= OnRewarded;
        AdsManager.instance.onAdsRewarded += OnRewarded;

        _notifiCheckAds.text = "";
        UpdateNextDay();
        _currProgressValue = CPlayerPrefs.GetInt(PROGRESS_KEY, 0);
        _sliderProgress.maxValue = _maxProgress;
        UpdateProgress();
        InitTimeCountDown();

        var hintAmount = CPlayerPrefs.GetInt("HINT_COLLECT", 0);
        var multipleHintAmount = CPlayerPrefs.GetInt("MULTIPLE_HINT_COLLECT", 0);
        var selectedHintAmount = CPlayerPrefs.GetInt("SELECTED_HINT_COLLECT", 0);
        _textHintCollect.text = hintAmount > 0 ? "X" + hintAmount : "";
        _textMultipleHintCollect.text = multipleHintAmount > 0 ? "X" + multipleHintAmount : "";
        _textSelectedHintCollect.text = selectedHintAmount > 0 ? "X" + selectedHintAmount : "";
    }

    private void InitListRandomMultipleHint()
    {
        listRandomMultiple = new List<int>();
        int num = 100;
        var rate5 = (int)(0.04f * num);
        var rate4 = (int)((0.04f + 0.06f) * num);
        var rate3 = (int)((0.04f + 0.06f + 0.1f) * num);
        var rate2 = (int)((0.04f + 0.06f + 0.1f + 0.2f) * num);
        //var rate1 = (int)(0.6f * num);
        for (int i = 0; i < num; i++)
        {
            if (i <= rate5)
                listRandomMultiple.Add(5);
            if (rate5 < i && i <= rate4)
                listRandomMultiple.Add(4);
            if (rate4 < i && i <= rate3)
                listRandomMultiple.Add(3);
            if (rate3 < i && i <= rate2)
                listRandomMultiple.Add(2);
            if (rate2 < i)
                listRandomMultiple.Add(1);
        }
    }

    private void InitListRandomSelectedHint()
    {
        listRandomSelected = new List<int>();
        int num = 100;
        var rate5 = (int)(0.08f * num);
        var rate4 = (int)((0.08f + 0.12f) * num);
        var rate3 = (int)((0.08f + 0.12f + 0.2f) * num);
        var rate2 = (int)((0.08f + 0.12f + 0.2f + 0.4f) * num);
        //var rate1 = (int)(0.6f * num);
        for (int i = 0; i < num; i++)
        {
            if (i <= rate5)
                listRandomSelected.Add(5);
            if (rate5 < i && i <= rate4)
                listRandomSelected.Add(4);
            if (rate4 < i && i <= rate3)
                listRandomSelected.Add(3);
            if (rate3 < i && i <= rate2)
                listRandomSelected.Add(2);
            if (rate2 < i)
                listRandomSelected.Add(1);
        }
    }

    private void InitListRandomHint()
    {
        listRandomHint = new List<int>();
        int num = 100;
        var rate5 = (int)(0.6f * num);
        var rate4 = (int)((0.6f + 0.2f) * num);
        var rate3 = (int)((0.6f + 0.2f + 0.15f) * num);
        var rate2 = (int)((0.6f + 0.2f + 0.15f + 0.05f) * num);
        //var rate1 = (int)(0.6f * num);
        for (int i = 0; i < num; i++)
        {
            if (i <= rate5)
                listRandomHint.Add(5);
            if (rate5 < i && i <= rate4)
                listRandomHint.Add(4);
            if (rate4 < i && i <= rate3)
                listRandomHint.Add(3);
            if (rate3 < i && i <= rate2)
                listRandomHint.Add(2);
            if (rate2 < i)
                listRandomHint.Add(1);
        }
    }

    private void InitTimeCountDown()
    {
        UpdateNextDay();
        UpdateTimeValue();
    }

    private void ShowBtnWatch(bool show)
    {
        _btnWatch.gameObject.SetActive(show);
        _collectButton.gameObject.SetActive(!show);
    }

    public void OnClickReward()
    {
        AudienceNetworkFbAd.instance.rewardIdFaceAds = ConfigController.instance.config.facebookAdsId.rewardedFreeBoosters;
        UnityAdTest.instance.myPlacementId = ConfigController.instance.config.unityAdsId.rewardedFreeBoosters;
        AdmobController.instance.videoAdsId = ConfigController.instance.config.admob.rewardedFreeBoosters;

        AdsManager.instance.ShowVideoAds();

        // AdmobController.instance.ShowRewardBasedVideo();
        Sound.instance.Play(Sound.Others.PopupOpen);
#if UNITY_EDITOR
        OnRewarded();
#endif
    }
    void OnRewarded()
    {
        _btnWatch.interactable = false;
        TweenControl.GetInstance().DelayCall(transform, 0.1f, () =>
        {
            _currProgressValue += 1;
            //Debug.Log("OnRewarded invoke: " + this.name);
            CPlayerPrefs.SetInt(PROGRESS_KEY, _currProgressValue);
            UpdateProgress();
            _btnWatch.interactable = true;
        });
    }

    public void OnClickCollect()
    {
        var hintRandomAmount = RandomSingle(listRandomHint);
        var multipleHintRandomAmount = RandomSingle(listRandomMultiple);
        var selectedHintRandomAmount = RandomSingle(listRandomSelected);
        CPlayerPrefs.SetInt("HINT_COLLECT", hintRandomAmount);
        CPlayerPrefs.SetInt("MULTIPLE_HINT_COLLECT", multipleHintRandomAmount);
        CPlayerPrefs.SetInt("SELECTED_HINT_COLLECT", selectedHintRandomAmount);
        _textHintCollect.text = "X" + /*ConfigController.Config.gameParameters.rewardHintDaily*/hintRandomAmount;
        _textMultipleHintCollect.text = "X" + /*ConfigController.Config.gameParameters.rewardMultipleHintDaily*/multipleHintRandomAmount;
        _textSelectedHintCollect.text = "X" + /*ConfigController.Config.gameParameters.rewardMultipleHintDaily*/selectedHintRandomAmount;
        Sound.instance.Play(Sound.Collects.LevelOpen);
        _collectButton.gameObject.SetActive(false);
        _currProgressValue = 0;
        CurrencyController.CreditHintFree(/*ConfigController.Config.gameParameters.rewardHintDaily*/hintRandomAmount);
        CurrencyController.CreditMultipleHintFree(/*ConfigController.Config.gameParameters.rewardMultipleHintDaily*/multipleHintRandomAmount);
        CurrencyController.CreditSelectedHintFree(/*ConfigController.Config.gameParameters.rewardMultipleHintDaily*/selectedHintRandomAmount);
        InitTimeCountDown();
        _isReward = false;
        CPlayerPrefs.SetBool(TIME_REWARD_KEY, _isReward);
        if (DateTime.Compare(DateTime.Now, nextDay) < 0)
            CPlayerPrefs.SetBool(NEXT_DAY_KEY, false);
        if (HomeController.instance != null)
            HomeController.instance.ShowIconNoti();
        _animChest.onEventAction = OpenChestEvent;
        _animChest.SetAnimation(_collectAnim, false, () =>
        {
            RestartCountdown();
        });
        CPlayerPrefs.SetInt(PROGRESS_KEY, _currProgressValue);
        UpdateProgress();
        Firebase.Analytics.FirebaseAnalytics.LogEvent(
          Firebase.Analytics.FirebaseAnalytics.EventEarnVirtualCurrency,
          new Firebase.Analytics.Parameter[] {
            new Firebase.Analytics.Parameter(
              Firebase.Analytics.FirebaseAnalytics.ParameterValue, "Hint: " + hintRandomAmount + "| MultipleHint: " + multipleHintRandomAmount + "| SelectedHint: " + selectedHintRandomAmount),
            new Firebase.Analytics.Parameter(
              Firebase.Analytics.FirebaseAnalytics.ParameterVirtualCurrencyName, "collect_freeboosters"),
          }
        );
    }

    void OpenChestEvent(Spine.Event eventData)
    {
        if (eventData.Data.Name == "ket thuc collect")
        {
            if (EffectController.instance.IsEffectOn)
            {
                _fxEffect = Instantiate(_fxLightPfb, transform);
                _fxEffect.transform.position = _posChest.position;
            }
            _textNotifyTitle.text = OPEN_CHEST;
            _textNotifyCollect.gameObject.SetActive(false);
            TweenControl.GetInstance().ScaleFromZero(_textHintCollect.gameObject, 0.3f);
            TweenControl.GetInstance().ScaleFromZero(_textMultipleHintCollect.gameObject, 0.3f);
            TweenControl.GetInstance().ScaleFromZero(_textSelectedHintCollect.gameObject, 0.3f);
            //_overlayCollect.SetActive(true);
            //TweenControl.GetInstance().DelayCall(transform, 3f, () =>
            //{
            //    _overlayCollect.SetActive(false);
            //});
        }
    }

    private int RandomSingle(List<int> listRandom)
    {
        var temp = 0;
        temp = UnityEngine.Random.Range(0, listRandom.Count);
        var numsRandom = 0;
        numsRandom = listRandom[temp];
        listRandom.RemoveAt(temp);
        return numsRandom;
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
        CheckTimeReward();
        //});
    }

    private IEnumerator ShowEffectCollect(int value)
    {
        MonoUtils.instance.ShowTotalStarCollect(value, null);
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
        var isNextDay = CPlayerPrefs.GetBool(NEXT_DAY_KEY);
        if (isNextDay)
        {
            CPlayerPrefs.SetInt(PROGRESS_KEY, 0);
            _currProgressValue = 0;
        }
        if (CPlayerPrefs.HasKey(TIME_REWARD_KEY))
        {
            _isReward = CPlayerPrefs.GetBool(TIME_REWARD_KEY, false);
            if (!_isReward && !isNextDay)
                StartCoroutine(CountDownTime());
            else
                ShowReward();
        }
        else
        {
            ShowReward();
            //_timeCountdown.text = _contentReward;
        }
        UpdateProgress();
    }

    private void ShowReward()
    {
        _animChest.SetAnimation(_idleAnim, true);
        _timeValue = 0;
        if (_currProgressValue < _maxProgress)
        {
            ShowBtnWatch(true);
            if (AdsManager.instance != null)
            {
                if (!AdsManager.instance.AdsIsLoaded(false, _notifiCheckAds))
                    //_btnAdsDisable.SetActive(true);
                    _btnWatch.gameObject.SetActive(false);
                else
                    //_btnAdsDisable.SetActive(false);
                    _btnWatch.gameObject.SetActive(true);
            }
        }
        else
            ShowBtnWatch(false);
        _timeCountdown.transform.localScale = Vector3.zero;
    }

    private IEnumerator CountDownTime()
    {
        _animChest.SetAnimation(_collectLoopAnim, true);
        _btnWatch.gameObject.SetActive(false);
        while (!_isReward)
        {
            _textNotifyTitle.text = OPEN_CHEST;
            _textHintCollect.transform.localScale = Vector3.one;
            _textMultipleHintCollect.transform.localScale = Vector3.one;
            _textSelectedHintCollect.transform.localScale = Vector3.one;
            _textNotifyCollect.gameObject.SetActive(false);
            _timeCountdown.transform.localScale = Vector3.one;
            TimeSpan timeSpan = TimeSpan.FromSeconds(_timeValue);
            _timeCountdown.text = timeSpan.ToString();
            if (_timeCountdown.text == "00:00:00" || _timeCountdown.text == "")
            {
                _textNotifyTitle.text = CLOSE_CHEST;
                _textHintCollect.transform.localScale = Vector3.zero;
                _textMultipleHintCollect.transform.localScale = Vector3.zero;
                _textSelectedHintCollect.transform.localScale = Vector3.zero;
                _textNotifyCollect.gameObject.SetActive(true);
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

    void UpdateNextDay()
    {
        var isRefresh = CPlayerPrefs.GetBool(NEXT_DAY_KEY, false);
        var timeRefresh = DateTime.Today + TimeSpan.FromSeconds(_valueTimeGift * 3600);
        if (CPlayerPrefs.HasKey(NEXT_DAY_KEY))
        {
            var time = CPlayerPrefs.GetLong(TIME_NEXT_DAY_KEY);
            nextDay = DateTime.FromBinary(time);
            Debug.Log("NextDay: " + nextDay);
        }
        else
        {
            nextDay = DateTime.FromBinary(timeRefresh.Ticks);
            CPlayerPrefs.SetLong(TIME_NEXT_DAY_KEY, timeRefresh.Ticks);
            Debug.Log("NextDay New: " + nextDay);
        }
        if (DateTime.Compare(DateTime.Now, nextDay) > 0 && !isRefresh)
        {
            CPlayerPrefs.SetBool(NEXT_DAY_KEY, true);
            nextDay = DateTime.Now.Date.AddDays(1) + TimeSpan.FromSeconds(_valueTimeGift * 3600);
            Debug.Log("NextDay New Refresh: " + nextDay);
            CPlayerPrefs.SetLong(TIME_NEXT_DAY_KEY, nextDay.Ticks);
        }
    }

    private void UpdateTimeValue()
    {
        var timeNow = DateTime.Now;
        var result = nextDay - timeNow;
        _timeValue = (int)result.TotalSeconds; // _timeValue = (int)(_sumTime - timeNow);
        if (_timeValue <= 0)
        {
            _timeValue = 0;
            UpdateNextDay();
        }
    }

    private void OnDestroy()
    {
        if (_fxEffect != null)
            Destroy(_fxEffect);

        //_rewardedVideoControl.onRewardedCallback -= OnRewarded;
        AdsManager.instance.onAdsRewarded -= OnRewarded;
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
            //_rewardedVideoControl.gameObject.SetActive(true);
        }
    }
    //==
}
