using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExtraWordDialog : Dialog
{
    public Transform claimTr;
    public ExtraProgress extraProgress;
    public GameObject claimButton;
    public GameObject rewardButton;
    public TextMeshProUGUI progressText;
    //public TextMeshProUGUI wordText;
    //public TextMeshProUGUI claimQuantityText;
    public CanvasGroup panelNewLevel;
    public CanvasGroup panelOldLevel;
    [SerializeField] private TextMeshProUGUI _bonusWordPfb;
    [SerializeField] private TextMeshProUGUI _textCollectEnough;
    [SerializeField] private Transform _contentScroll;
    [Space]
    [SerializeField] private int _reward = 40;
    [SerializeField] private int _amountWordTarget = 2;
    [Header("THEME UI CHANGE")]
    [SerializeField] private Image _btnHtpl;
    [SerializeField] private Image _board;
    [SerializeField] private Image _boardTitle;
    [SerializeField] private Image _iconStar;
    [SerializeField] private Image _bgProgress;
    [SerializeField] private Image _progressMask;
    [SerializeField] private Image _progressBar;
    [SerializeField] private Image _circleProgress;
    [SerializeField] private Image _btnCollect;
    [SerializeField] private Image _btnReward;
    [SerializeField] private Image _iconCandyStar;
    [SerializeField] private Image _iconAds;
    [SerializeField] private TextMeshProUGUI _txtBoardTitle;
    [SerializeField] private TextMeshProUGUI _txtTotalProgress;
    [SerializeField] private TextMeshProUGUI _txtBtnCollect;
    [SerializeField] private TextMeshProUGUI _txtBtnReward;
    //[SerializeField] private SpineControl _animBtnAdsReward;

    private int claimQuantity;

    protected override void Start()
    {
        base.Start();
        CheckTheme();
        if (MainController.instance != null)
            MainController.instance.canvasCollect.gameObject.SetActive(true);

        extraProgress.target = Prefs.extraTarget;
        extraProgress.current = Prefs.extraProgress;
        claimQuantity = (int)extraProgress.target / _amountWordTarget * _amountWordTarget;

        UpdateUI();
        ShowPanelCurrLevel();
    }

    private void OnEnable()
    {
        AdsManager.instance.onAdsRewarded -= OnCompleteVideo;
        AdsManager.instance.onAdsRewarded += OnCompleteVideo;
    }

    private void OnDisable()
    {
        AdsManager.instance.onAdsRewarded -= OnCompleteVideo;
    }

    private void OnDestroy()
    {
        AdsManager.instance.onAdsRewarded -= OnCompleteVideo;
    }

    private void CheckTheme()
    {
        if (MainController.instance != null)
        {
            var currTheme = ThemesControl.instance.CurrTheme;
            _btnHtpl.sprite = currTheme.uiData.bonusBoxData.btnHtpl;
            _board.sprite = currTheme.uiData.bonusBoxData.board;
            _boardTitle.sprite = currTheme.uiData.bonusBoxData.boardTitle;
            _iconStar.sprite = currTheme.uiData.bonusBoxData.iconStar;
            _bgProgress.sprite = currTheme.uiData.bonusBoxData.bgProgress;
            _progressMask.sprite = currTheme.uiData.bonusBoxData.progressBar;
            _progressBar.sprite = currTheme.uiData.bonusBoxData.progressBar;
            _circleProgress.sprite = currTheme.uiData.bonusBoxData.circleProgress;
            _btnCollect.sprite = currTheme.uiData.bonusBoxData.btnCollect;
            _btnReward.sprite = currTheme.uiData.bonusBoxData.btnReward;
            _iconCandyStar.sprite = currTheme.uiData.bonusBoxData.iconCandyStar;
            _iconAds.sprite = currTheme.uiData.bonusBoxData.iconAds;

            _btnHtpl.SetNativeSize();
            //_board.SetNativeSize();
            //_boardTitle.SetNativeSize();
            _iconStar.SetNativeSize();
            //_bgProgress.SetNativeSize();
            //_progressMask.SetNativeSize();
            //_progressBar.SetNativeSize();
            _circleProgress.SetNativeSize();
            //_btnCollect.SetNativeSize();
            //_btnReward.SetNativeSize();
            _iconCandyStar.SetNativeSize();
            _iconAds.SetNativeSize();

            _txtBoardTitle.color = currTheme.uiData.bonusBoxData.colorTextBoardTitle;
            _txtBtnCollect.color = currTheme.uiData.bonusBoxData.colorTextBtn;
            _txtBtnReward.color = currTheme.uiData.bonusBoxData.colorTextBtnReward;
            _txtTotalProgress.color = _textCollectEnough.color = _bonusWordPfb.color = currTheme.fontData.colorContentDialog;

            if (currTheme.uiData.bonusBoxData.alignCenterIcon)
            {
                _iconCandyStar.transform.localPosition = new Vector3(_iconCandyStar.transform.localPosition.x, 0);
                _iconAds.transform.localPosition = new Vector3(_iconAds.transform.localPosition.x, 0);
                _txtBtnReward.transform.localPosition = new Vector3(_txtBtnReward.transform.localPosition.x, 0);
            }
            //_animBtnAdsReward.thisSkeletonControl.initialSkinName = currTheme.animData.skinAnim;
            //_animBtnAdsReward.SetSkin(currTheme.animData.skinAnim);
        }
    }

    void OnCompleteVideo()
    {
        TweenControl.GetInstance().DelayCall(transform, 0.1f,()=> {
            gameObject.GetComponent<GraphicRaycaster>().enabled = false;
            TweenControl.GetInstance().DelayCall(transform, 0.5f, () =>
            {
                StartCoroutine(ShowEffectCollect(_reward, rewardButton.transform));
                Collect();
            });
            Firebase.Analytics.FirebaseAnalytics.LogEvent(
              Firebase.Analytics.FirebaseAnalytics.EventEarnVirtualCurrency,
              new Firebase.Analytics.Parameter[] {
            new Firebase.Analytics.Parameter(
              Firebase.Analytics.FirebaseAnalytics.ParameterValue, _reward),
            new Firebase.Analytics.Parameter(
              Firebase.Analytics.FirebaseAnalytics.ParameterVirtualCurrencyName, "bonus_box"),
              }
            );
        });
    }

    private IEnumerator ShowEffectCollect(int value, Transform posStart)
    {
        MonoUtils.instance.ShowTotalStarCollect(value, null);
        var result = value / 5;
        for (int i = 0; i < value; i++)
        {
            if (i < 5)
            {
                MonoUtils.instance.ShowEffect(result, null, null, posStart);
            }
            yield return new WaitForSeconds(0.06f);
            if (i == 5)
                gameObject.GetComponent<GraphicRaycaster>().enabled = true;
        }
    }

    public void OnClickShowVideoAds()
    {
    
        AdsManager.instance.ShowVideoAds();

        Sound.instance.Play(Sound.Others.PopupOpen);
    }

    private void ShowPanelCurrLevel()
    {
        if (Prefs.IsSaveLevelProgress())
        {
            panelNewLevel.alpha = 1;
            panelOldLevel.alpha = 0;
            panelNewLevel.blocksRaycasts = true;
            panelOldLevel.blocksRaycasts = false;
        }
        else
        {
            panelNewLevel.alpha = 0;
            panelOldLevel.alpha = 1;
            panelOldLevel.blocksRaycasts = true;
            panelNewLevel.blocksRaycasts = false;
        }
    }

    public void OnClickHTPL(int selectID)
    {
        DialogController.instance.ShowDialog(DialogType.HowtoPlay, DialogShow.STACK_DONT_HIDEN);
        Sound.instance.Play(Sound.Others.PopupOpen);
        HowToPlayDialog.instance.ShowMeanWordByID(selectID);
    }

    public void Claim()
    {
        extraProgress.current = Mathf.Abs((int)extraProgress.current - (int)extraProgress.target);
        Prefs.extraProgress = (int)extraProgress.current;
        UpdateUI();

        StartCoroutine(ShowEffectCollect(claimQuantity, claimButton.transform));
        ExtraWord.instance.OnClaimed();

        if (Prefs.extraTarget == _amountWordTarget && Prefs.totalExtraAdded > _amountWordTarget)
        {
            Prefs.extraTarget = _amountWordTarget;
            extraProgress.target = _amountWordTarget;
            claimQuantity = (int)extraProgress.target / _amountWordTarget * _amountWordTarget;
            UpdateUI();
        }
        Firebase.Analytics.FirebaseAnalytics.LogEvent(
          Firebase.Analytics.FirebaseAnalytics.EventEarnVirtualCurrency,
          new Firebase.Analytics.Parameter[] {
            new Firebase.Analytics.Parameter(
              Firebase.Analytics.FirebaseAnalytics.ParameterValue, claimQuantity),
            new Firebase.Analytics.Parameter(
              Firebase.Analytics.FirebaseAnalytics.ParameterVirtualCurrencyName, "collect_bonus_box"),
          }
        );
    }

    private void Collect()
    {
        extraProgress.current = Mathf.Abs((int)extraProgress.current - (int)extraProgress.target);
        Prefs.extraProgress = (int)extraProgress.current;
        UpdateUI();

        ExtraWord.instance.OnClaimed();

        if (Prefs.extraTarget == _amountWordTarget && Prefs.totalExtraAdded > _amountWordTarget)
        {
            Prefs.extraTarget = _amountWordTarget;
            extraProgress.target = _amountWordTarget;
            claimQuantity = (int)extraProgress.target / _amountWordTarget * _amountWordTarget;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        //claimQuantityText.text = claimQuantity.ToString();
        _textCollectEnough.gameObject.SetActive(extraProgress.current < extraProgress.target);
        claimButton.SetActive(extraProgress.current >= extraProgress.target);
        rewardButton.SetActive(extraProgress.current >= extraProgress.target);
        progressText.text = extraProgress.current + "/" + extraProgress.target;
        //wordText.text = "";
        ClearContentScroll();
        ShowBonusWord();
    }

    private void ShowBonusWord()
    {
        foreach (var word in ExtraWord.instance.extraWords)
        {
            //wordText.text += "  " + word.ToUpper();
            var wordItem = Instantiate(_bonusWordPfb, _contentScroll);
            var buttonWord = wordItem.GetComponent<Button>();
            wordItem.text = word.ToUpper();
            buttonWord.onClick.RemoveAllListeners();
            buttonWord.onClick.AddListener(() => OnClickBonusWord(wordItem.text));
        }
    }

    private void ClearContentScroll()
    {
        for (int i = 0; i < _contentScroll.childCount; i++)
        {
            var child = _contentScroll.GetChild(i).gameObject;
            Destroy(child);
        }
    }

    void OnClickBonusWord(string word)
    {
        DialogController.instance.ShowDialog(DialogType.MeanInGameDialog, DialogShow.STACK_DONT_HIDEN);
        Sound.instance.Play(Sound.Others.PopupOpen);
        DictionaryInGameDialog.instance.GetIndexByWord(word);
    }

    public override void Close()
    {
        base.Close();
        ExtraWord.instance.OnClaimed();
        MainController.instance.beeController.OnBeeButtonClick();
    }
}
