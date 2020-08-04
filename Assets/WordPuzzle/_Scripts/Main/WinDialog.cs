using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;
using Superpow;

public class WinDialog : Dialog
{
    private int numLevels;
    private bool isLastLevel;
    private int subWorld, level;
    private bool _isWatchAds;

    public int honeyPoints;
    public TextMeshProUGUI honeyPointsTxt;

    [SerializeField]
    private GameObject explosiveFxPref;
    [SerializeField]
    private GameObject _panelDialog;
    [SerializeField]
    private GameObject RewardButton;
    [SerializeField]
    private Button _nextButton;
    [SerializeField]
    private Button _homeButton;
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
    private Text txtReward;
    [SerializeField]
    private TextMeshProUGUI txtRewardByAds;
    [SerializeField]
    private TextMeshProUGUI _txtCollectChickenBank;
    [SerializeField]
    private GameObject _btnBee;
    public GameObject _chickenBank;
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
    [SerializeField] private SpineControl _animCandyChapterClear;
    [SerializeField] private string levelClearIdleAnim = "idle";
    [SerializeField] private string showLevelClearAnim = "animation";
    [SerializeField] private string eggLevelAnim = "Level Clear";
    [SerializeField] private string eggChapterAnim = "Chapter Clear";
    [SerializeField] private string eggLevelIdleAnim = "idle Level Clear";
    [SerializeField] private string eggChapterIdleAnim = "idle Chapter Clear";
    [SerializeField] private string candyChapterAnim = "Keo truoc";
    [SerializeField] private string candyChapterIdleAnim = "Keo truoc loop";
    [Space]
    [SerializeField] private GameObject _btnAdsDisable;
    [SerializeField] private Color _colorDisable;
    [SerializeField] private Color _colorNormal;
    [Header("UI THEME")]
    [SerializeField] private Button _btnDictionary;
    [SerializeField] private Image _iconStar;
    [SerializeField] private Image _iconAdd;
    [SerializeField] private Image _bgCurrency;
    [SerializeField] private Image _iconDictionary;
    [SerializeField] private Image _iconHoney;
    [SerializeField] private Image _bgHoney;
    [SerializeField] private Image _imgNumBee;
    [SerializeField] private TextMeshProUGUI _textNumberStar;
    [SerializeField] private TextMeshProUGUI _textHoney;
    [SerializeField] private TextMeshProUGUI _textNumBee;
    [SerializeField] private SpineControl _animIconBee;

    private bool isAdsLoaed;
    private GameObject _fxEffect;
    private List<GameObject> _stars;
    //private RewardVideoController _rewardControl;

    public static WinDialog instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        _animEggChapterClear.onEventAction = ShowStarsEffect;
        _animEggLevelClear.onEventAction = ShowStarsEffect;
        CheckTheme();
    }

    protected override void Start()
    {
        base.Start();
        ShowStars();
        CheckUnlock();
        SaveProgressComplete();
        //_rewardControl = GameObject.FindObjectOfType<RewardVideoController>();
        //if (_rewardControl == null)
        //    _rewardControl = Instantiate(_rewardVideoPfb, transform);

        //_rewardControl.onRewardedCallback -= OnCompleteReward;
        AdsManager.instance.onAdsRewarded -= OnCompleteReward;
        isAdsLoaed = AdsManager.instance.AdsIsLoaded();
        if (!isAdsLoaed)
            AdsManager.instance.LoadDataAds();
        CheckShowAdsButton();
        isSound = false;
        _btnBee.transform.position = new Vector3(_btnBee.transform.position.x, WordRegion.instance.animBtnHintTarget.transform.position.y);
        _chickenBank.transform.position = new Vector3(_chickenBank.transform.position.x, _btnBee.transform.position.y);
    }

    private void CheckTheme()
    {
        var currTheme = ThemesControl.instance.CurrTheme;

        _btnDictionary.image.sprite = currTheme.uiData.btnDictionary;
        _btnDictionary.image.SetNativeSize();

        _iconStar.sprite = currTheme.uiData.iconStar;
        _iconAdd.sprite = currTheme.uiData.iconAdd;
        _bgCurrency.sprite = currTheme.uiData.bgCurrency;
        _iconDictionary.sprite = currTheme.uiData.iconDictionary;
        _bgHoney.sprite = currTheme.uiData.bgHoney;
        _iconHoney.sprite = currTheme.uiData.iconHoney;
        _imgNumBee.sprite = currTheme.uiData.numBooster;

        _iconStar.SetNativeSize();
        _iconAdd.SetNativeSize();
        _bgCurrency.SetNativeSize();
        _iconDictionary.SetNativeSize();
        _bgHoney.SetNativeSize();
        _iconHoney.SetNativeSize();
        _imgNumBee.SetNativeSize();

        _textNumberStar.font = currTheme.fontData.fontAsset;
        _textNumberStar.fontSizeMax = currTheme.fontData.fontSizeMaxNumStar;
        _textNumberStar.color = currTheme.fontData.colorTextNumStar;

        _textHoney.font = currTheme.fontData.fontAsset;
        _textHoney.color = currTheme.fontData.colorTextNumStar;
        _textHoney.fontSizeMax = currTheme.fontData.fontSizeMaxNumStar;

        _textNumBee.font = currTheme.fontData.fontAsset;
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

        _panelDialog.SetActive(false);
        DialogOverlay.instance.Overlay.enabled = false;
    }

    public void ShowLevelChapterClear()
    {
        _panelDialog.SetActive(true);
        DialogOverlay.instance.Overlay.enabled = true;
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
                //Sound.instance.Play(Sound.Scenes.ChapterClear);
                Sound.instance.Play(Sound.Scenes.LevelClear);
                ShowEffectTitle(0.84f);
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
    string explosiveEffect = "ExplosiveEffect";
    private void ExplosiveEffect()
    {
        GameObject explosive = Instantiate(explosiveFxPref, _panelDialog.transform);
        explosive.transform.localPosition = new Vector3(0, 110, 0);
    }
    private void ShowEggAnim()
    {
        if (level == numLevels - 1)
        {
            ShowEggChapterClear(true);
            TweenControl.GetInstance().DelayCall(transform, 1.2f, () =>
            {
                if (EffectController.instance.IsEffectOn)
                {
                    Invoke(explosiveEffect, 1.7f);
                }
                _animEggChapterClear.gameObject.SetActive(true);
                _animCandyChapterClear.SetAnimation(candyChapterAnim, false, () =>
                {
                    _animCandyChapterClear.SetAnimation(candyChapterIdleAnim, true);
                });
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

    private void ShowPanelButton(bool show)
    {
        //WordRegion.instance.listWordCorrect = new List<string>();
        //foreach (var line in WordRegion.instance.Lines)
        //{
        //    WordRegion.instance.listWordCorrect.Add(line.answer);
        //}
        //if (AdsManager.instance != null)
        //{
        //    if (!)
        //    {
        //        RewardButton.SetActive(false);
        //        //txtRewardByAds.color = _colorDisable;
        //    }
        //    else
        //    {
        //        RewardButton.SetActive(true);
        txtRewardByAds.color = _colorNormal;
        //    }
        //}

        var tweenControl = TweenControl.GetInstance();
        var numlevels = Utils.GetNumLevels(Prefs.unlockedWorld, Prefs.unlockedSubWorld);
        var currlevel = Int32.Parse(MainController.instance.levelNameText.text.Replace("LEVEL ", ""));
        var valueShow = (ConfigController.instance.config.gameParameters.minBank * 10 / 100) + ConfigController.instance.config.gameParameters.minBank;
        var currStarBank = ChickenBankController.instance.CurrStarChicken;
        var lastWord = MainController.instance.gameData.words[MainController.instance.gameData.words.Count - 1];
        var lastLevel = lastWord.subWords[lastWord.subWords.Count - 1].gameLevels[lastWord.subWords[lastWord.subWords.Count - 1].gameLevels.Count - 1];

        GroupButton.SetActive(show);
        _starReward.SetActive(true);
        _starReward.transform.localScale = Vector3.zero;

        if (currStarBank < valueShow && !CPlayerPrefs.HasKey("OPEN_CHICKEN"))
        {
            _chickenBank.SetActive(false);
        }
        else
        {
            if (currStarBank >= valueShow && !CPlayerPrefs.HasKey("CHICKEN_TUTORIAL"))
            {
                _chickenBank.SetActive(false);
                gameObject.GetComponent<GraphicRaycaster>().enabled = false;
            }
            _chickenBankAnim.onEventAction = ShowTextCollect;
            var posTargetChicken = _chickenBank.transform.localPosition.x / 2;
            tweenControl.MoveRectX(_chickenBank.transform as RectTransform, posTargetChicken - 50, 0.5f, () =>
            {
                tweenControl.MoveRectX(_chickenBank.transform as RectTransform, posTargetChicken, 0.3f, () =>
                {
                    if (currStarBank >= valueShow && !CPlayerPrefs.HasKey("CHICKEN_TUTORIAL"))
                    {
                        gameObject.GetComponent<GraphicRaycaster>().enabled = true;
                        CPlayerPrefs.SetBool("CHICKEN_TUTORIAL", true);
                        TutorialController.instance.ShowPopChickenBankTut();
                    }
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
                if (currlevel >= lastLevel.level)
                {
                    _homeButton.gameObject.SetActive(true);
                    _nextButton.gameObject.SetActive(false);
                }
                button.transform.localScale = Vector3.zero;
                tweenControl.Scale(button, Vector3.one * 1.3f, 0.3f, () =>
                {
                    tweenControl.Scale(button, Vector3.one, 0.3f,
                        () =>
                        {
                            //TweenControl.GetInstance().FadeAnfaText(honeyPointsTxt, 1, .5f, () => { TweenControl.GetInstance().FadeAnfaText(honeyPointsTxt, 0, .5f); });
                            if (!isColorFade) this.StartCoroutine(ColorFade());
                        }
                        , EaseType.InQuad);
                });
            }
            else
            {

                if (currlevel > 40 || CPlayerPrefs.HasKey("BEE_TUTORIAL") || BeeManager.instance.CurrBee > 0)
                {
                    _btnBee.gameObject.SetActive(true);
                    _animIconBee.thisSkeletonControl.initialSkinName = ThemesControl.instance.CurrTheme.animData.skinAnim;
                    _animIconBee.SetSkin(ThemesControl.instance.CurrTheme.animData.skinAnim);
                }
                else
                    _btnBee.gameObject.SetActive(false);
                var posTarget = _btnBee.transform.localPosition.x / 2;
                tweenControl.MoveRectX(_btnBee.transform as RectTransform, posTarget + 50, 0.5f, () =>
                {
                    tweenControl.MoveRectX(_btnBee.transform as RectTransform, posTarget, 0.3f, () =>
                    {
                        tweenControl.Scale(_starReward, Vector3.one * 1.35f, 0.3f, () =>
                        {
                            tweenControl.Scale(_starReward, Vector3.one, 0.8f);
                        }, EaseType.Linear);
                        //var cvGR = _starReward.GetComponent<CanvasGroup>();
                        //tweenControl.FadeAnfa(cvGR, 1, 1.2f);
                        //tweenControl.ScaleFromZero(_starReward.gameObject, 1.5f);
                        //tweenControl.MoveRectY(_starReward.transform as RectTransform, -20, 0.6f, () =>
                        //{
                        //    tweenControl.MoveRectY(_starReward.transform as RectTransform, -95, 0.4f);
                        //});
                    });
                });
            }
        }
        if (currlevel > AdsManager.instance.MinLevelToLoadRewardVideo && isAdsLoaed)
            RewardButton.SetActive(true);
        else
            RewardButton.SetActive(false);
    }
    bool isColorFade;
    public IEnumerator ColorFade()
    {
        isColorFade = true;

        honeyPointsTxt.font = ThemesControl.instance.CurrTheme.fontData.fontAsset;
        honeyPointsTxt.color = ThemesControl.instance.CurrTheme.fontData.colorWin;
        honeyPointsTxt.text = "X" + honeyPoints.ToString();

        float alphaValue = 0;
        Color currentColor = new Color(honeyPointsTxt.color.r, honeyPointsTxt.color.g, honeyPointsTxt.color.b, 0);
        honeyPointsTxt.color = currentColor;
        yield return new WaitForSeconds(.2f);
        while (alphaValue <= 1)
        {
            alphaValue += 5 * Time.deltaTime;
            honeyPointsTxt.color = new Color(currentColor.r, currentColor.g, currentColor.b, alphaValue);
            yield return null;
        }
        yield return null;

        while (alphaValue >= 0)
        {
            alphaValue -= 5 * Time.deltaTime;
            honeyPointsTxt.color = new Color(currentColor.r, currentColor.g, currentColor.b, alphaValue);
            yield return null;
        }
        isColorFade = false;
    }
    public void HidenTut()
    {
        TutorialController.instance.HidenPopTut();
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

    public void UpdateChickenBankAmount()
    {
        _txtCollectChickenBank.text = "X" + ChickenBankController.instance.CurrStarChicken;
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
                Sound.instance.Play(Sound.Collects.CoinCollect);
            }
            yield return new WaitForSeconds(0.06f);
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
            if (WordRegion.instance != null && WordRegion.instance.CurLevel >= 48)
                ChickenBankController.instance.AddtoBank();
            var result = GetChickenbankNonReward();
            _txtCollectChickenBank.text = "X" + (result > FacebookController.instance.user.maxbank ? FacebookController.instance.user.maxbank : result);

            if (EffectController.instance.IsEffectOn)
            {
                _fxEffect = Instantiate(WordRegion.instance.compliment.fxLevelClear.gameObject, transform);
            }
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
            if (eventData.Data.Name == "EGG_CHAP")
                Sound.instance.Play(Sound.Scenes.ChapterClear);
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

    private void LevelUpCallEventFirebase()
    {
        var unlockWord = Prefs.unlockedWorld;
        var unlockedSubWorld = Prefs.unlockedSubWorld;
        var unlockedLevel = Prefs.unlockedLevel;
        var numLevelInChap = Utils.GetNumLevels(unlockWord, unlockedSubWorld);
        var currlevel = (unlockedLevel + numLevelInChap * unlockedSubWorld + unlockWord * MainController.instance.gameData.words[0].subWords.Count * numLevelInChap);

        Firebase.Analytics.FirebaseAnalytics.LogEvent(
          Firebase.Analytics.FirebaseAnalytics.EventLevelUp,
          new Firebase.Analytics.Parameter[] {
            new Firebase.Analytics.Parameter(
              Firebase.Analytics.FirebaseAnalytics.ParameterCharacter, SystemInfo.deviceName + " (Level Up)"),
            new Firebase.Analytics.Parameter(
              Firebase.Analytics.FirebaseAnalytics.ParameterLevel, currlevel),
          }
        );
    }

    private void CheckUnlock()
    {
        var numlevels = Utils.GetNumLevels(Prefs.unlockedWorld, Prefs.unlockedSubWorld);
        var currlevel = (GameState.currentLevel + numlevels * GameState.currentSubWorld + MainController.instance.gameData.words[0].subWords.Count * numlevels * GameState.currentWorld) + 1;
        var lastWord = MainController.instance.gameData.words[MainController.instance.gameData.words.Count - 1];
        var lastLevel = lastWord.subWords[lastWord.subWords.Count - 1].gameLevels[lastWord.subWords[lastWord.subWords.Count - 1].gameLevels.Count - 1];

        if (currlevel >= lastLevel.level)
        {
            Prefs.IsLevelEnd = true;
            return;
        }
        GameState.currentLevel = (level + 1) % numLevels;
        if (level == numLevels - 1)
        {
            Prefs.countChapter += 1;
            Prefs.countChapterDaily += 1;
            GameState.currentSubWorld = (subWorld + 1) % MainController.instance.gameData.words[0].subWords.Count;
            if (subWorld == MainController.instance.gameData.words[0].subWords.Count - 1)
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
        LevelUpCallEventFirebase();
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
        TweenControl.GetInstance().KillAll();
        var numlevels = Utils.GetNumLevels(Prefs.unlockedWorld, Prefs.unlockedSubWorld);
        var currlevel = Int32.Parse(MainController.instance.levelNameText.text.Replace("LEVEL ", ""));
        var lastWord = MainController.instance.gameData.words[MainController.instance.gameData.words.Count - 1];
        var lastLevel = lastWord.subWords[lastWord.subWords.Count - 1].gameLevels[lastWord.subWords[lastWord.subWords.Count - 1].gameLevels.Count - 1];
        //CheckUnlock();
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
             if (currlevel >= lastLevel.level)
             {
                 AudienceNetworkBanner.instance.DisposeAllBannerAd();
                 Debug.Log("Banner disposed");
                 CUtils.LoadScene(Const.SCENE_HOME, true);
             }              
             else
                 CUtils.LoadScene(Const.SCENE_MAIN, true);
         });
    }

    public void NextClickReward()
    {
        var numlevels = Utils.GetNumLevels(Prefs.unlockedWorld, Prefs.unlockedSubWorld);
        var currlevel = Int32.Parse(MainController.instance.levelNameText.text.Replace("LEVEL ", ""));
        var lastWord = MainController.instance.gameData.words[MainController.instance.gameData.words.Count - 1];
        var lastLevel = lastWord.subWords[lastWord.subWords.Count - 1].gameLevels[lastWord.subWords[lastWord.subWords.Count - 1].gameLevels.Count - 1];
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
            if (currlevel >= lastLevel.level)
            {
                //AudienceNetworkBanner.instance.DisposeAllBannerAd();
                //Debug.Log("Banner disposed");
                CUtils.LoadScene(Const.SCENE_HOME, true);
            }               
            else
                CUtils.LoadScene(Const.SCENE_MAIN, true);
        });
    }


    public void RewardClick()
    {
        if (MainController.instance != null)
        {
            MainController.instance.canvasFx.gameObject.SetActive(EffectController.instance.IsEffectOn);
            MainController.instance.canvasCollect.gameObject.SetActive(true);
        }
        _isWatchAds = true;
        _nextButton.interactable = false;
        //_rewardControl.onRewardedCallback += OnCompleteReward;
        AdsManager.instance.onAdsRewarded += OnCompleteReward;

        TweenControl.GetInstance().DelayCall(transform, 0.1f, () =>
        {
            Sound.instance.Play(Sound.Others.PopupOpen);
            //AdmobController.instance.ShowRewardBasedVideo();
            //AdsManager.instance._adsController = AudienceNetworkFbAd.instance;

            if (level == numLevels - 1)
            {
                AudienceNetworkFbAd.instance.rewardIdFaceAds = ConfigController.instance.config.facebookAdsId.rewardedChapterClear;
                UnityAdTest.instance.myPlacementId = ConfigController.instance.config.unityAdsId.rewardedChapter;
                AdmobController.instance.videoAdsId = ConfigController.instance.config.admob.rewardedChapter;
            }
            else
            {
                AudienceNetworkFbAd.instance.rewardIdFaceAds = ConfigController.instance.config.facebookAdsId.rewardedLevelClear;
                UnityAdTest.instance.myPlacementId = ConfigController.instance.config.unityAdsId.rewardedLevel;
                AdmobController.instance.videoAdsId = ConfigController.instance.config.admob.rewardedLevel;
            }
            AdsManager.instance.ShowVideoAds(true, CheckShowAdsButton, CheckShowAdsButton);

//#if UNITY_EDITOR
//            OnCompleteReward();
//#endif
        });
        //CUtils.ShowInterstitialAd();
    }

    private void CheckShowAdsButton()
    {
        //if (AdsManager.instance != null)
        //{
        //    if (!AdsManager.instance.AdsIsLoaded())
        //    {
        //        //_btnAdsDisable.SetActive(true);
        RewardButton.gameObject.SetActive(false);
        //        //txtRewardByAds.color = _colorDisable;
        _nextButton.interactable = true;
        //    }
        //    else
        //    {
        //        //_btnAdsDisable.SetActive(false);
        //        txtRewardByAds.color = _colorNormal;
        //    }
        //}
        Debug.Log("Show Ads Faild");
        Debug.Log("_nextButton.interactable: " + _nextButton.interactable);
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
        //_rewardControl.onRewardedCallback -= OnCompleteReward;
        //AdsManager.instance.onAdsRewarded -= OnCompleteReward;

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
            Firebase.Analytics.FirebaseAnalytics.LogEvent(
              Firebase.Analytics.FirebaseAnalytics.EventEarnVirtualCurrency,
              new Firebase.Analytics.Parameter[] {
                new Firebase.Analytics.Parameter(
                  Firebase.Analytics.FirebaseAnalytics.ParameterValue, Const.REWARD_ADS_CHAPTER_CLEAR),
                new Firebase.Analytics.Parameter(
                  Firebase.Analytics.FirebaseAnalytics.ParameterVirtualCurrencyName, "chapter_clear"),
              }
            );
        }
        else
        {
            TweenControl.GetInstance().DelayCall(transform, 0.5f, () =>
            {
                //CurrencyController.CreditBalance(Const.REWARD_ADS_LEVEL_CLEAR);
                StartCoroutine(ShowEffectCollect(Const.REWARD_ADS_LEVEL_CLEAR));
                //Debug.Log("reward Level: " + Const.REWARD_ADS_LEVEL_CLEAR);
            });
            Firebase.Analytics.FirebaseAnalytics.LogEvent(
              Firebase.Analytics.FirebaseAnalytics.EventEarnVirtualCurrency,
              new Firebase.Analytics.Parameter[] {
                new Firebase.Analytics.Parameter(
                  Firebase.Analytics.FirebaseAnalytics.ParameterValue, Const.REWARD_ADS_LEVEL_CLEAR),
                new Firebase.Analytics.Parameter(
                  Firebase.Analytics.FirebaseAnalytics.ParameterVirtualCurrencyName, "level_clear"),
              }
            );
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
        if (level == numLevels - 1)
        {
            txtRewardByAds.text = "x" + Const.REWARD_ADS_CHAPTER_CLEAR;
            txtReward.text = "x" + Const.REWARD_CHAPTER_CLEAR + "";
        }
        else
        {
            txtRewardByAds.text = "x" + Const.REWARD_ADS_LEVEL_CLEAR + "";
            txtReward.text = "x" + Const.REWARD_CHAPTER_CLEAR + "";
        };
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

    private void OnDisable()
    {
        AdsManager.instance.onAdsRewarded -= OnCompleteReward;
    }

    private void OnDestroy()
    {
        AdsManager.instance.onAdsRewarded -= OnCompleteReward;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            RewardChapter();
    }

}
