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
    public TextMeshProUGUI claimQuantityText;
    public CanvasGroup panelNewLevel;
    public CanvasGroup panelOldLevel;
    [SerializeField] private TextMeshProUGUI _bonusWordPfb;
    [SerializeField] private Transform _contentScroll;
    [Space]
    [SerializeField] private RewardVideoController _rewardVideoPfb;
    [SerializeField] private int reward = 40;
    [SerializeField] private Transform _currBanlancePos;

    private RewardVideoController _rewardController;
    private int numWords, claimQuantity;

    protected override void Start()
    {
        base.Start();

        _rewardController = FindObjectOfType<RewardVideoController>();
        if (_rewardController == null)
            _rewardController = Instantiate(_rewardVideoPfb);
        _rewardController.onRewardedCallback -= OnCompleteVideo;

        extraProgress.target = Prefs.extraTarget;
        extraProgress.current = Prefs.extraProgress;
        claimQuantity = (int)extraProgress.target / 2 * 20;

        UpdateUI();
        ShowPanelCurrLevel();
    }

    void OnCompleteVideo()
    {
        _rewardController.onRewardedCallback -= OnCompleteVideo;
        StartCoroutine(ShowEffectCollect(reward / 5));
        gameObject.GetComponent<GraphicRaycaster>().enabled = false;
        Claim();
        TweenControl.GetInstance().DelayCall(transform, 2.5f, () => {
            gameObject.GetComponent<GraphicRaycaster>().enabled = true;
        });
    }

    private IEnumerator ShowEffectCollect(int value)
    {
        for (int i = 0; i < value; i++)
        {
            if (i < 5)
            {
                MonoUtils.instance.ShowEffect(value, _currBanlancePos);
            }
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void OnClickShowVideoAds()
    {
        _rewardController.onRewardedCallback += OnCompleteVideo;
        AdmobController.instance.ShowRewardBasedVideo();
        Sound.instance.Play(Sound.Others.PopupOpen);
#if UNITY_EDITOR
        OnCompleteVideo();
#endif
    }

    private void ShowPanelCurrLevel()
    {
        if (Prefs.IsSaveLevelProgress())
        {
            panelNewLevel.alpha = 1;
            panelOldLevel.alpha = 0;
        }
        else
        {
            panelNewLevel.alpha = 0;
            panelOldLevel.alpha = 1;
        }
    }

    public void OnClickHTPL(int selectID)
    {
        DialogController.instance.ShowDialog(DialogType.HowtoPlay, DialogShow.STACK);
        Sound.instance.Play(Sound.Others.PopupOpen);
        HowToPlayDialog.instance.ShowMeanWordByID(selectID);
    }

    public void Claim()
    {
        CPlayerPrefs.SetBool("COLLECTED", true);
        extraProgress.current -= (int)extraProgress.target;
        Prefs.extraProgress = (int)extraProgress.current;
        UpdateUI();

        StartCoroutine(ShowEffectCollect(claimQuantity / 5));
        ExtraWord.instance.OnClaimed();

        if (Prefs.extraTarget == 2 && Prefs.totalExtraAdded > 2)
        {
            Prefs.extraTarget = 4;
            extraProgress.target = 4;
            claimQuantity = (int)extraProgress.target / 2 * 20;
            UpdateUI();
        }
    }

    //private IEnumerator ClaimEffect()
    //{
    //    Transform rubyBalance = GameObject.FindWithTag("RubyBalance").transform;
    //    //var middlePoint = CUtils.GetMiddlePoint(claimTr.position, rubyBalance.position, -0.4f);
    //    //Vector3[] waypoints = { claimTr.position, middlePoint, rubyBalance.position };
    //    for (int i = 0; i < claimQuantity; i++)
    //    {
    //        if (i < 5)
    //            MonoUtils.instance.ShowEffect(claimQuantity / 5, rubyBalance);
    //        yield return new WaitForSeconds(0.02f);
    //    }
    //}

    private void UpdateUI()
    {
        claimQuantityText.text = claimQuantity.ToString();
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
    }
}
