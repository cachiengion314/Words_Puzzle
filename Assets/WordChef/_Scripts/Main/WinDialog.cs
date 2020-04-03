﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

public class WinDialog : Dialog
{
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
    private GameObject light;

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
    [Space]
    [SerializeField] private SpineControl _animChapterClear;
    [SerializeField] private SpineControl _animLevelClear;
    [SerializeField] private SpineControl _animEggLevelClear;
    [SerializeField] private SpineControl _animEggChapterClear;
    [SerializeField] private string levelClearIdleAnim = "idle";
    [SerializeField] private string showLevelClearAnim = "animation";
    [SerializeField] private string eggLevelAnim = "Level Clear";
    [SerializeField] private string eggChapterAnim = "Chapter Clear";
    [SerializeField] private string eggLevelIdleAnim = "idle Level Clear";
    [SerializeField] private string eggChapterIdleAnim = "idle Chapter Clear";

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
        txtReward.transform.localScale = Vector3.zero;
        light.SetActive(false);

        if (isLastLevel)
        {
            if (level == numLevels - 1)
            {
                ShowChapterClear(true);
                Sound.instance.Play(Sound.Scenes.ChapterClear);
                _animChapterClear.SetAnimation(showLevelClearAnim, false, () =>
                {
                    _animChapterClear.SetAnimation(levelClearIdleAnim, true);
                });
                _animEggChapterClear.onEventAction = ShowStarsEffect;
                _animEggChapterClear.SetAnimation(eggChapterAnim, false, () =>
                {
                    _animEggChapterClear.SetAnimation(eggChapterIdleAnim, true);

                });
            }
            else
            {
                CPlayerPrefs.SetBool("Received", false);
                ShowChapterClear(false);
                Sound.instance.Play(Sound.Scenes.LevelClear);
                _animLevelClear.SetAnimation(showLevelClearAnim, false, () =>
                {
                    _animLevelClear.SetAnimation(levelClearIdleAnim, true);
                });
                _animEggLevelClear.onEventAction = ShowStarsEffect;
                _animEggLevelClear.SetAnimation(eggLevelAnim, false, () =>
                {
                    _animEggLevelClear.SetAnimation(eggLevelIdleAnim, true);

                });
            }
        }
        else
        {
            EggBig.SetActive(false);
            RewardButton.SetActive(false);
            GroupButton.SetActive(true);
        }
    }

    private void ShowChapterClear(bool show)
    {
        EggLevelClear.gameObject.SetActive(!show);
        _animLevelClear.gameObject.SetActive(!show);
        EggChapterClear.gameObject.SetActive(show);
        _animChapterClear.gameObject.SetActive(show);
    }

    private void ShowStarsEffect(Spine.Event eventData)
    {
        if (eventData.Data.Name == "EGG_CHAP" || eventData.Data.Name == "EGG_LEVEL")
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

    private IEnumerator IEShowEggOpen()
    {
        var creditBalance = CPlayerPrefs.GetBool("Received", false);
        if (!creditBalance)
        {
            CurrencyController.CreditBalance(Const.REWARD_CHAPTER_CLEAR);
            CPlayerPrefs.SetBool("Received", true);
        }
        //TitleLevelClear.SetActive(false);
        EggLevelClear.SetActive(false);
        EggChapterClear.SetActive(true);

        light.SetActive(true);
        var tweener = FadeImage.DOFade(0f, 1f);
        tweener.onComplete += () =>
        {
            txtReward.transform.localScale = Vector3.one;
            FadeImage.gameObject.SetActive(false);
        };
        yield return new WaitForSeconds(0.5f);
        GroupButton.SetActive(true);
    }

    public void NextClick()
    {
        Close();
        Sound.instance.Play(Sound.Collects.LevelClose);
        Prefs.countLevel += 1;
        Prefs.countLevelDaily += 1;
        CUtils.LoadScene(/*level == numLevels - 1 ? 1 :*/ 3, true);
        FacebookController.instance.user.levelProgress = new string[] { "0" };
        FacebookController.instance.user.answerProgress = new string[] { "0" };
        FacebookController.instance.SaveDataGame();
    }


    public void RewardClick()
    {
        Sound.instance.Play(Sound.Others.PopupOpen);
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
        Sound.instance.Play(Sound.Others.PopupOpen);
    }
}
