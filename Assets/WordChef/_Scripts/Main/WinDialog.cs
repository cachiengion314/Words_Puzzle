using System.Collections;
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
    private bool _isWatchAds;

    [SerializeField]
    private GameObject _panelDialog;
    [SerializeField]
    private GameObject RewardButton;
    [SerializeField]
    private Button _nextButton;
    [SerializeField]
    private GameObject GroupButton;
    [SerializeField]
    private Transform StarsGrid;
    [SerializeField]
    private Transform[] _starsGrid;
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
    [SerializeField]
    private TextMeshProUGUI _txtCollectChickenBank;
    [SerializeField]
    private GameObject _btnBee;
    [SerializeField]
    private GameObject _chickenBank;
    [SerializeField]
    private SpineControl _chickenBankAnim;
    [SerializeField] private string _collectAnim = "icon chicken bank PLAY";
    [SerializeField] private string _loopAnim = "icon chicken bank PLAY LOOP";
    [SerializeField]
    private GameObject _starReward;
    [SerializeField]
    private RewardVideoController _rewardVideoPfb;
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

    private GameObject _fxEffect;
    private List<GameObject> _stars;
    private RewardVideoController _rewardControl;

    private void Awake()
    {
        _animEggChapterClear.onEventAction = ShowStarsEffect;
        _animEggLevelClear.onEventAction = ShowStarsEffect;
    }

    protected override void Start()
    {
        base.Start();
        ShowStars();
        CheckUnlock();
        SaveProgressComplete();
        _rewardControl = GameObject.FindObjectOfType<RewardVideoController>();
        if (_rewardControl == null)
            _rewardControl = Instantiate(_rewardVideoPfb, transform);
        _rewardControl.onRewardedCallback -= OnCompleteReward;
        isSound = false;
    }

    private void ShowStars()
    {
        numLevels = Superpow.Utils.GetNumLevels(GameState.currentWorld, GameState.currentSubWorld);
        subWorld = GameState.currentSubWorld;
        level = GameState.currentLevel;

        isLastLevel = Prefs.IsSaveLevelProgress();
        light.SetActive(false);
        GroupButton.SetActive(false);
        SetupStars();

        ShowTitleAnim();
        if (isLastLevel)
        {
            ShowEggAnim();
        }
        else
        {
            EggBig.SetActive(false);
            RewardButton.SetActive(false);
            ShowPanelButton(true);
        }
    }

    private void ShowTitleAnim()
    {
        if (level == numLevels - 1)
        {
            TweenControl.GetInstance().DelayCall(transform, 0.1f, () =>
            {
                ShowTitleChapterClear(true);
                Sound.instance.Play(Sound.Scenes.ChapterClear);
                ShowEffectTitle(1.4f);
                _animChapterClear.SetAnimation(showLevelClearAnim, false, () =>
                {
                    _animChapterClear.SetAnimation(levelClearIdleAnim, true);
                });
            });

        }
        else
        {
            CPlayerPrefs.SetBool("Received", false);

            //TweenControl.GetInstance().MoveRectY(TitleLevelClear.transform as RectTransform, -151f, 2f);
            TweenControl.GetInstance().DelayCall(transform, 0.1f, () =>
            {
                ShowTitleChapterClear(false);
                Sound.instance.Play(Sound.Scenes.LevelClear);
                ShowEffectTitle(0.5f);
                _animLevelClear.SetAnimation(showLevelClearAnim, false, () =>
            {
                _animLevelClear.SetAnimation(levelClearIdleAnim, true);
            });
            });
        }
    }

    private void ShowEggAnim()
    {
        if (level == numLevels - 1)
        {
            ShowEggChapterClear(true);
            TweenControl.GetInstance().DelayCall(transform, 1.2f, () =>
            {
                _animEggChapterClear.gameObject.SetActive(true);
                _animEggChapterClear.SetAnimation(eggChapterAnim, false, () =>
                {
                    _animEggChapterClear.SetAnimation(eggChapterIdleAnim, true);

                });
            });
        }
        else
        {
            ShowEggChapterClear(false);
            TweenControl.GetInstance().DelayCall(transform, 1.2f, () =>
            {
                _animEggLevelClear.gameObject.SetActive(true);
                _animEggLevelClear.SetAnimation(eggLevelAnim, false, () =>
                {
                    _animEggLevelClear.SetAnimation(eggLevelIdleAnim, true);

                });
            });
        }
    }

    private IEnumerator PlaySoundCollect()
    {
        for (int i = 0; i < 5; i++)
        {
            if (i < 5)
            {
                Sound.instance.Play(Sound.Collects.CoinCollect);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void ShowPanelButton(bool show)
    {
        //WordRegion.instance.listWordCorrect = new List<string>();
        //foreach (var line in WordRegion.instance.Lines)
        //{
        //    WordRegion.instance.listWordCorrect.Add(line.answer);
        //}
        var tweenControl = TweenControl.GetInstance();
        GroupButton.SetActive(show);

        var valueShow = (ConfigController.instance.config.gameParameters.minBank * 10 / 100) + ConfigController.instance.config.gameParameters.minBank;
        var currStarBank = ChickenBankController.instance.CurrStarChicken;
        if (currStarBank < valueShow && !CPlayerPrefs.HasKey("OPEN_CHICKEN"))
        {
            _chickenBank.SetActive(false);
        }
        else
        {
            _chickenBankAnim.onEventAction = ShowTextCollect;
            var posTargetChicken = _chickenBank.transform.localPosition.x / 2;
            tweenControl.MoveRectX(_chickenBank.transform as RectTransform, posTargetChicken - 50, 0.5f, () =>
            {
                tweenControl.MoveRectX(_chickenBank.transform as RectTransform, posTargetChicken, 0.3f, () =>
                {
                    StartCoroutine(PlaySoundCollect());

                    _chickenBankAnim.SetAnimation(_collectAnim, false, () =>
                    {
                        _chickenBankAnim.SetAnimation(_loopAnim, true);
                    });
                });
            });
        }

        for (int i = 0; i < GroupButton.transform.childCount; i++)
        {
            var button = GroupButton.transform.GetChild(i).gameObject;
            if (button != _btnBee && button != _chickenBank)
            {
                button.transform.localScale = Vector3.zero;
                tweenControl.Scale(button, Vector3.one * 1.2f, 0.3f, () =>
                {
                    tweenControl.Scale(button, Vector3.one, 0.3f, null, EaseType.InQuad);
                });
            }
            else
            {
                var posTarget = _btnBee.transform.localPosition.x / 2;
                tweenControl.MoveRectX(_btnBee.transform as RectTransform, posTarget + 50, 0.5f, () =>
                {
                    tweenControl.MoveRectX(_btnBee.transform as RectTransform, posTarget, 0.3f, () =>
                    {
                        _starReward.SetActive(true);
                        _starReward.transform.localScale = Vector3.zero;
                        var cvGR = _starReward.GetComponent<CanvasGroup>();
                        tweenControl.FadeAnfa(cvGR, 1, 1.2f);
                        tweenControl.ScaleFromZero(_starReward.gameObject, 1.5f);
                        tweenControl.MoveRectY(_starReward.transform as RectTransform, -20, 0.6f, () =>
                        {
                            tweenControl.MoveRectY(_starReward.transform as RectTransform, -95, 0.4f);
                        });
                    });
                });
            }
        }
    }

    private void ShowTextCollect(Spine.Event eventData)
    {
        var tweenControl = TweenControl.GetInstance();
        if (eventData.Data.Name == "x25")
        {
            var condition = GetChickenbankNonReward();
            if (condition > FacebookController.instance.user.maxbank)
                return;
            StartCoroutine(BankNumberUp());
            //tweenControl.FadeAnfaText(_txtCollectChickenBank, 1, 0.5f);
            //tweenControl.DelayCall(_txtCollectChickenBank.transform, 1f, () =>
            //{
            //    tweenControl.FadeAnfaText(_txtCollectChickenBank, 0, 0.5f, () => {
            //        _txtCollectChickenBank.text = ChickenBankController.instance.CurrStarChicken.ToString();
            //        tweenControl.FadeAnfaText(_txtCollectChickenBank, 1, 0.5f);
            //    });
            //});
        }
    }


    private IEnumerator BankNumberUp()
    {
        var tempValue = GetChickenbankNonReward();
        var result = tempValue;
        for (int i = 0; i < ChickenBankController.instance.Amount; i++)
        {
            _txtCollectChickenBank.text = "X" + result;
            if (i < 5 && result < FacebookController.instance.user.maxbank)
            {
                result += ChickenBankController.instance.Amount / 5;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void SetupStars()
    {
        _stars = new List<GameObject>();
        for (int i = 0; i < numLevels; i++)
        {
            var star = GameObject.Instantiate(StartGroup, _starsGrid[i]);
            star.transform.localPosition = Vector3.zero;
            star.SetActive(true);
            if (i < level)
                star.transform.GetChild(0).gameObject.SetActive(true);
            _stars.Add(star);
        }
        //StarsGrid.gameObject.SetActive(false);
    }

    private void ShowEffectTitle(float timeDelayShow)
    {
        TweenControl.GetInstance().DelayCall(transform, timeDelayShow, () =>
        {
            ChickenBankController.instance.AddtoBank();
            var result = GetChickenbankNonReward();
            _txtCollectChickenBank.text = "X" + (result > FacebookController.instance.user.maxbank ? FacebookController.instance.user.maxbank : result);
            _fxEffect = Instantiate(WordRegion.instance.compliment.fxLevelClear.gameObject, transform);
        });
    }

    private double GetChickenbankNonReward()
    {
        return (ChickenBankController.instance.CurrStarChicken + FacebookController.instance.user.remainBank) - ChickenBankController.instance.Amount;
    }

    private void ShowEggChapterClear(bool show)
    {
        EggLevelClear.gameObject.SetActive(!show);
        EggChapterClear.gameObject.SetActive(show);
    }

    private void ShowTitleChapterClear(bool show)
    {
        _animLevelClear.gameObject.SetActive(!show);
        _animChapterClear.gameObject.SetActive(show);
    }

    private void ShowStarsEffect(Spine.Event eventData)
    {
        if (eventData.Data.Name == "EGG_CHAP" || eventData.Data.Name == "EGG_LEVEL")
        {
            StarsGrid.gameObject.SetActive(true);
            for (int i = 0; i < numLevels; i++)
            {
                var currStar = _stars[i];
                if (i <= level)
                {
                    var startOn = currStar.transform.GetChild(0);
                    startOn.gameObject.SetActive(true);
                    if (i == level)
                    {
                        startOn.DOScale(0f, 0.8f).From().SetDelay(0.2f).SetEase(Ease.OutElastic);
                        StartCoroutine(IEShowButtonLevelClear());
                    }
                    else
                    {
                        StartCoroutine(IEShowButtonLevelClear());
                    }
                }
            }
        }
        else if (eventData.Data.Name == "LIGHT_OPEN")
        {
            light.SetActive(true);
        }
    }



    private void CheckUnlock()
    {
        GameState.currentLevel = (level + 1) % numLevels;
        if (level == numLevels - 1)
        {
            Prefs.countChapter += 1;
            Prefs.countChapterDaily += 1;
            GameState.currentSubWorld = (subWorld + 1) % MainController.instance.gameData.words.Count;
            if (subWorld == MainController.instance.gameData.words.Count - 1)
            {
                GameState.currentWorld++;
            }
        }

        if (isLastLevel)
        {
            Prefs.unlockedWorld = GameState.currentWorld;
            Prefs.unlockedSubWorld = GameState.currentSubWorld;
            Prefs.unlockedLevel = GameState.currentLevel;

            FacebookController.instance.user.levelProgress = new string[] { "0" };
            FacebookController.instance.user.answerProgress = new string[] { "0" };
        }
    }

    private IEnumerator IEShowButtonLevelClear()
    {
        GameState.currentLevel = (level + 1) % numLevels;
        if (level == numLevels - 1)
        {
            txtRewardByAds.text = "x" + Const.REWARD_ADS_CHAPTER_CLEAR;
            txtReward.text = "x" + Const.REWARD_CHAPTER_CLEAR + "";

            yield return new WaitForSeconds(0);
            //FadeImage.gameObject.SetActive(true);
            //var tweener = FadeImage.DOFade(1f, 1f);
            //tweener.onComplete += () =>
            //{
            StartCoroutine(IEShowEggOpen());
            //};
        }
        else
        {
            txtRewardByAds.text = "x" + Const.REWARD_ADS_LEVEL_CLEAR + "";
            txtReward.text = "x" + Const.REWARD_CHAPTER_CLEAR + "";
            //_starReward.SetActive(true);
            yield return new WaitForSeconds(0);
            ShowPanelButton(true);
        };
    }

    private IEnumerator IEShowEggOpen()
    {
        //TitleLevelClear.SetActive(false);
        EggLevelClear.SetActive(false);
        EggChapterClear.SetActive(true);
        //var tweener = FadeImage.DOFade(0f, 1f);
        //tweener.onComplete += () =>
        //{

        FadeImage.gameObject.SetActive(false);
        //};
        yield return new WaitForSeconds(0);
        ShowPanelButton(true);
    }

    public void NextClick()
    {
        if (level == numLevels - 1)
        {
            var creditBalance = CPlayerPrefs.GetBool("Received", false);
            if (!creditBalance)
            {
                StartCoroutine(ShowEffectCollect(Const.REWARD_CHAPTER_CLEAR, _nextButton.transform));
            }
        }

        if (_fxEffect != null)
            Destroy(_fxEffect);
        gameObject.GetComponent<GraphicRaycaster>().enabled = false;

        if (Prefs.IsLastLevel())
        {
            FacebookController.instance.newLevel = true;
            CPlayerPrefs.SetBool("LevelMisspelling", false);
        }
        if (Prefs.IsSaveLevelProgress())
        {
            FacebookController.instance.user.unlockedLevel = Prefs.unlockedLevel.ToString();
            FacebookController.instance.user.unlockedWorld = Prefs.unlockedWorld.ToString();
            FacebookController.instance.user.unlockedSubWorld = Prefs.unlockedSubWorld.ToString();
            FacebookController.instance.SaveDataGame();
        }
        //Close();
        _panelDialog.SetActive(false);
        DialogOverlay.instance.Overlay.enabled = false;
        Sound.instance.Play(Sound.Collects.LevelClose);
        TweenControl.GetInstance().DelayCall(transform, (level == numLevels - 1) ? 2.5f : 0.75f, () =>
         {
             if (level == numLevels - 1)
                 CPlayerPrefs.SetBool("Received", true);
             Close();
             CUtils.LoadScene(/*level == numLevels - 1 ? 1 :*/ 3, true);
         });
    }

    public void NextClickReward()
    {
        if (_fxEffect != null)
            Destroy(_fxEffect);
        gameObject.GetComponent<GraphicRaycaster>().enabled = false;

        if (Prefs.IsLastLevel())
        {
            FacebookController.instance.newLevel = true;
            CPlayerPrefs.SetBool("LevelMisspelling", false);
        }
        if (Prefs.IsSaveLevelProgress())
        {
            FacebookController.instance.user.unlockedLevel = Prefs.unlockedLevel.ToString();
            FacebookController.instance.user.unlockedWorld = Prefs.unlockedWorld.ToString();
            FacebookController.instance.user.unlockedSubWorld = Prefs.unlockedSubWorld.ToString();
            FacebookController.instance.SaveDataGame();
        }
        //Close();
        _panelDialog.SetActive(false);
        DialogOverlay.instance.Overlay.enabled = false;
        Sound.instance.Play(Sound.Collects.LevelClose);
        TweenControl.GetInstance().DelayCall(transform, 0.75f, () =>
        {
            if (level == numLevels - 1)
                CPlayerPrefs.SetBool("Received", true);
             Close();
            CUtils.LoadScene(/*level == numLevels - 1 ? 1 :*/ 3, true);
        });
    }


    public void RewardClick()
    {
        _isWatchAds = true;
        _nextButton.interactable = false;
        _rewardControl.onRewardedCallback += OnCompleteReward;
        TweenControl.GetInstance().DelayCall(transform, 0.1f, () =>
        {
            Sound.instance.Play(Sound.Others.PopupOpen);
            AdmobController.instance.ShowRewardBasedVideo();
#if UNITY_EDITOR
            OnCompleteReward();
#endif
        });
        //CUtils.ShowInterstitialAd();
    }

    private IEnumerator ShowEffectCollect(int value, Transform posCollect = null)
    {
        MonoUtils.instance.ShowTotalStarCollect(value, null);
        var result = value / 5;
        for (int i = 0; i < value; i++)
        {
            if (i < 5)
            {
                MonoUtils.instance.ShowEffect(result, null, null, posCollect == null ? RewardButton.transform : posCollect);
            }
            yield return new WaitForSeconds(0.06f);
        }

    }

    void OnCompleteReward()
    {
        _rewardControl.onRewardedCallback -= OnCompleteReward;
        //RewardButton.GetComponent<Button>().interactable = false;
        gameObject.GetComponent<GraphicRaycaster>().enabled = false;
        txtReward.text = "X" + Const.REWARD_ADS_CHAPTER_CLEAR;
        if (level == numLevels - 1)
        {
            //var value = Const.REWARD_ADS_CHAPTER_CLEAR;
            //CurrencyController.CreditBalance(value);
            TweenControl.GetInstance().DelayCall(transform, 0.5f, () =>
            {
                StartCoroutine(ShowEffectCollect(Const.REWARD_ADS_CHAPTER_CLEAR));
            });
            CPlayerPrefs.SetBool("Received", true);
        }
        else
        {
            TweenControl.GetInstance().DelayCall(transform, 0.5f, () =>
            {
                //CurrencyController.CreditBalance(Const.REWARD_ADS_LEVEL_CLEAR);
                StartCoroutine(ShowEffectCollect(Const.REWARD_ADS_LEVEL_CLEAR));
                Debug.Log("reward Level: " + Const.REWARD_ADS_LEVEL_CLEAR);
            });
        }
        TweenControl.GetInstance().DelayCall(transform, 2.5f, () =>
        {
            NextClickReward();
        });
    }

    public void LeaderboardClick()
    {
        Sound.instance.Play(Sound.Others.PopupOpen);
    }

    private void SaveProgressComplete()
    {
        if (Prefs.IsLastLevel())
        {
            FacebookController.instance.newLevel = true;
            CPlayerPrefs.SetBool("LevelMisspelling", true);
        }
        if (Prefs.IsSaveLevelProgress())
        {
            Prefs.countLevel += 1;
            Prefs.countLevelDaily += 1;

            FacebookController.instance.user.unlockedLevel = Prefs.unlockedLevel.ToString();
            FacebookController.instance.user.unlockedWorld = Prefs.unlockedWorld.ToString();
            FacebookController.instance.user.unlockedSubWorld = Prefs.unlockedSubWorld.ToString();
            FacebookController.instance.SaveDataGame();
        }
    }

    private void RewardChapter()
    {
        if (level == numLevels - 1)
        {
            var creditBalance = CPlayerPrefs.GetBool("Received", false);
            if (!creditBalance)
            {
                if (!_isWatchAds)
                    CurrencyController.CreditBalance(Const.REWARD_CHAPTER_CLEAR);
                CPlayerPrefs.SetBool("Received", true);
            }
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            RewardChapter();
    }

}
