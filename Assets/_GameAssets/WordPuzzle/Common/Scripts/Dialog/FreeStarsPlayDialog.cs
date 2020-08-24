using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FreeStarsPlayDialog : Dialog
{
    [SerializeField] private Button _btnWatch;
    [SerializeField] private RewardVideoController _rewardVideoPfb;
    [SerializeField] private GameObject _panelWatch;
    //[SerializeField] private Toggle _showAgain;
    [SerializeField] private List<ItemCollectData> _itemsCollect;
    [Header("THEME UI CHANGE")]
    [SerializeField] private Image _iconGift;
    [SerializeField] private Image _iconStar;
    [SerializeField] private Image _iconHint;
    [SerializeField] private Image _iconSelectedHint;
    [SerializeField] private Image _iconMultipleHint;
    [SerializeField] private Image _iconAds;
    [SerializeField] private Image _imgBtn;
    [SerializeField] private TextMeshProUGUI _txtBtnWatch;
    [SerializeField] private TextMeshProUGUI _txtContent;
    [SerializeField] private TextMeshProUGUI _txtContent2;

    private List<int> listRandom = new List<int>();

    private void OnEnable()
    {
        AdsManager.instance.onAdsRewarded -= OnCompleteVideo;
        AdsManager.instance.onAdsRewarded += OnCompleteVideo;
    }

    protected override void Start()
    {
        base.Start();
        CheckTheme();
        InitListRandom();

    }

    private void CheckTheme()
    {
        if (MainController.instance != null)
        {
            var currTheme = ThemesControl.instance.CurrTheme;
            _iconGift.sprite = currTheme.uiData.freestarPlayData.iconGift;
            _iconStar.sprite = currTheme.uiData.freestarPlayData.iconStar;
            _iconHint.sprite = currTheme.uiData.freestarPlayData.iconHint;
            _iconSelectedHint.sprite = currTheme.uiData.freestarPlayData.iconSelectedHint;
            _iconMultipleHint.sprite = currTheme.uiData.freestarPlayData.iconMultipleHint;
            _iconAds.sprite = currTheme.uiData.freestarPlayData.iconAds;
            _imgBtn.sprite = currTheme.uiData.freestarPlayData.btnWatch;

            _iconGift.SetNativeSize();
            _iconStar.SetNativeSize();
            _iconHint.SetNativeSize();
            _iconSelectedHint.SetNativeSize();
            _iconMultipleHint.SetNativeSize();
            _imgBtn.SetNativeSize();

            _txtBtnWatch.color = currTheme.uiData.freestarPlayData.colorTextBtn;
            _txtContent.color = _txtContent2.color = currTheme.fontData.colorContentDialog;

            foreach (var item in _itemsCollect)
            {
                if (item.itemType == ItemType.CURRENCY_BALANCE)
                    item.iconItem = currTheme.uiData.freestarPlayData.iconStar;
                else if (item.itemType == ItemType.HINT)
                    item.iconItem = currTheme.uiData.freestarPlayData.iconHint;
                else if (item.itemType == ItemType.HINT_SELECT)
                    item.iconItem = currTheme.uiData.freestarPlayData.iconSelectedHint;
                else if (item.itemType == ItemType.HINT_RANDOM)
                    item.iconItem = currTheme.uiData.freestarPlayData.iconMultipleHint;
            }
        }
    }

    private void InitListRandom()
    {
        listRandom = new List<int>();
        int num = 100;
        var starRate = (int)(0.8f * num);
        var hintRate = (int)((0.8f + 0.11f) * num);
        var selectedHintRate = (int)((0.8f + 0.11f + 0.06f) * num);
        //var rate1 = (int)(0.6f * num);
        for (int i = 0; i < num; i++)
        {
            if (i <= starRate)
                listRandom.Add(0);
            else if (starRate < i && i <= hintRate)
                listRandom.Add(1);
            else if (hintRate < i && i <= selectedHintRate)
                listRandom.Add(2);
            else if (selectedHintRate < i)
                listRandom.Add(3);
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

    private void CheckBtnShowUpdate(bool IsAvailableToShow)
    {
        //_btnWatch.gameObject.SetActive(IsAvailableToShow);
    }

    private void OnDestroy()
    {
        AdsManager.instance.onAdsRewarded -= OnCompleteVideo;
    }

    private void OnDisable()
    {
        AdsManager.instance.onAdsRewarded -= OnCompleteVideo;
    }

    public void DontShowAgain()
    {
        //CPlayerPrefs.SetBool("DONT_SHOW", _showAgain.isOn);
    }

    public void OnClickOpen()
    {
        Sound.instance.audioSource.Stop();
        Sound.instance.Play(Sound.Others.PopupOpen);
        AdsManager.instance.ShowVideoAds(true, Close, Close);
    }

    private void OnCompleteVideo()
    {
        TweenControl.GetInstance().DelayCall(transform, 0.1f, () =>
        {
            var resultRandom = RandomSingle(listRandom);
            var itemTarget = _itemsCollect[resultRandom];
            SceneAnimate.Instance.itemType = itemTarget.itemType;
            SceneAnimate.Instance.itemValue = itemTarget.value;
            SceneAnimate.Instance.textItem.text = itemTarget.value.ToString();
            SceneAnimate.Instance.imageItem.sprite = itemTarget.iconItem;
            SceneAnimate.Instance.imageItem.SetNativeSize();
            _panelWatch.transform.localScale = Vector3.zero;
            HidenOverlay();
            TweenControl.GetInstance().DelayCall(transform, 0.5f, () =>
            {
                Sound.instance.Play(Sound.Others.PopupOpen);
                DialogController.instance.ShowDialog(DialogType.CollectFreestarPlay, DialogShow.REPLACE_CURRENT);
            });

            Firebase.Analytics.FirebaseAnalytics.LogEvent(
              Firebase.Analytics.FirebaseAnalytics.EventEarnVirtualCurrency,
              new Firebase.Analytics.Parameter[] {
            new Firebase.Analytics.Parameter(
              Firebase.Analytics.FirebaseAnalytics.ParameterValue, 20),
            new Firebase.Analytics.Parameter(
              Firebase.Analytics.FirebaseAnalytics.ParameterVirtualCurrencyName, "free_stars_main"),
              }
            );
        });
    }
    public override void Close()
    {
        base.Close();
    }

    [Serializable]
    public class ItemCollectData
    {
        public ItemType itemType;
        public Sprite iconItem;
        public int value;
    }
}
