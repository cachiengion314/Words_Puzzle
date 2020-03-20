using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class WinDialog : Dialog {
    private int numLevels;
    private bool isLastLevel;
    private int subWorld, level;

    [SerializeField]
    private GameObject RewardButton;
    [SerializeField]
    private GameObject GroupButton;
    [SerializeField]
    private Transform StarsGrid;
    [SerializeField]
    private GameObject StartGroup;
    [SerializeField]
    private GameObject EggBig;

    [SerializeField]
    private Image FadeImage;
    [SerializeField]
    private GameObject EggLevelClear;
    [SerializeField]
    private GameObject TitleLevelClear;
    [SerializeField]
    private GameObject EggChapterClear;

    [SerializeField]
    private TextMeshProUGUI txtReward;
    [SerializeField]
    private TextMeshProUGUI txtRewardByAds;

    protected override void Start()
    {
        base.Start();
        ShowStars();
        CheckUnlock();
    }

    private void ShowStars()
    {
        numLevels = Superpow.Utils.GetNumLevels(GameState.currentWorld, GameState.currentSubWorld);
        subWorld = GameState.currentSubWorld;
        level = GameState.currentLevel;

        isLastLevel = Prefs.IsLastLevel();

        if (isLastLevel)
        {
            for (int i = 0; i < numLevels; i++)
            {
                var starGroup = GameObject.Instantiate(StartGroup, StarsGrid);
                starGroup.SetActive(true);
                if (i <= level)
                {
                    var startOn = starGroup.transform.GetChild(0);
                    startOn.gameObject.SetActive(true);
                    if (i == level)
                    {
                        startOn.DOScale(0f, 0.8f).From().SetDelay(0.2f).SetEase(Ease.OutElastic);
                        StartCoroutine(IEShowButtonLevelClear());
                    }
                }
            }
        }
        else
        {
            EggBig.SetActive(false);
            RewardButton.SetActive(false);
            GroupButton.SetActive(true);
        }
    }

    private void CheckUnlock()
    {
        GameState.currentLevel = (level + 1) % numLevels;
        if (level == numLevels - 1)
        {
            Prefs.countChapter += 1;
            Prefs.countChapterDaily += 1;
            GameState.currentSubWorld = (subWorld + 1) % Const.NUM_SUBWORLD;
            if (subWorld == Const.NUM_SUBWORLD - 1)
            {
                GameState.currentWorld++;
            }
        }

        if (isLastLevel)
        {
            Prefs.unlockedWorld = GameState.currentWorld;
            Prefs.unlockedSubWorld = GameState.currentSubWorld;
            Prefs.unlockedLevel = GameState.currentLevel;
        }
    }

    private IEnumerator IEShowButtonLevelClear()
    {
        GameState.currentLevel = (level + 1) % numLevels;
        if (level == numLevels - 1)
        {
            txtRewardByAds.text = "X" + Const.REWARD_ADS_CHAPTER_CLEAR;
            txtReward.text = "X" + Const.REWARD_CHAPTER_CLEAR + "";

            yield return new WaitForSeconds(1f);
            FadeImage.gameObject.SetActive(true);
            var tweener = FadeImage.DOFade(1f, 1f);
            tweener.onComplete += () =>
            {
                StartCoroutine(IEShowEggOpen());
            };
        }
        else
        {
            txtRewardByAds.text = "X" + Const.REWARD_ADS_LEVEL_CLEAR + "";
            txtReward.text = "X" + Const.REWARD_CHAPTER_CLEAR + "";

            yield return new WaitForSeconds(1f);
            GroupButton.SetActive(true);
        };
    }

    private IEnumerator IEShowEggOpen() {
        CurrencyController.CreditBalance(Const.REWARD_CHAPTER_CLEAR);

        TitleLevelClear.SetActive(false);
        EggLevelClear.SetActive(false);
        EggChapterClear.SetActive(true);

        var tweener = FadeImage.DOFade(0f, 1f);
        tweener.onComplete += () =>
        {
            FadeImage.gameObject.SetActive(false);
        };

        yield return new WaitForSeconds(1.5f);
        GroupButton.SetActive(true);
    }

    public void NextClick()
    {
        Close();
        Sound.instance.PlayButton();
        Prefs.countLevel += 1; 
        Prefs.countLevelDaily += 1;
        CUtils.LoadScene(/*level == numLevels - 1 ? 1 :*/ 3, true);
        FacebookController.instance.user.levelProgress = new string[] { "0" };
        FacebookController.instance.SaveDataGame();
    }

    
    public void RewardClick()
    {
        Sound.instance.PlayButton();
        if (level == numLevels - 1)
        {
            CurrencyController.CreditBalance(Const.REWARD_ADS_CHAPTER_CLEAR);
        }
        else
        {
            CurrencyController.CreditBalance(Const.REWARD_ADS_LEVEL_CLEAR);
        }
        RewardButton.SetActive(false);

        CUtils.ShowInterstitialAd();
    }

    public void LeaderboardClick()
    {
        Sound.instance.PlayButton();
    }
}
