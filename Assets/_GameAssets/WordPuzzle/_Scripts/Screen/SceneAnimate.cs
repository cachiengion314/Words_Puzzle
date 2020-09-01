using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Superpow;
using Spine.Unity;

public class SceneAnimate : MonoBehaviour
{
    public static SceneAnimate Instance { get; private set; }

    public GameObject donotDestroyOnLoad;
    public AnimEvent animEvent;
    [Space]
    [SerializeField] private string _closeScene;
    public Animator animatorScene;
    public GameObject _loadingScreen;
    //public Slider _progressLoading;
    public TextMeshProUGUI _textProgress;
    [Space]
    public Button _btnPlay;
    public GameObject _maskShadow;
    public SpineControl _spineAnimEgg;
    public SpineControl _spineAnimShadow;
    public string showAnim = "animation";
    public string loopAnim = "Loop";
    public string showShadow = "Shadow";
    public string showShadowLoop = "Shadow Loop";
    public string showgiado = "animation2";
    public string idleEgg = "Không Anim";
    public string idleEggShadow = "Không Anim Shadow";
    [Space]
    [SerializeField] private Image _overlayPauseGame;
    [Space]
    [SerializeField] private SafeAreaPanel _safeArea;
    [SerializeField] private Image _bgFirstLoading;
    [SerializeField] private Image _bgLoading;
    [SerializeField] private SkeletonGraphic _animTip;
    [SerializeField] private Text _textTip;
    [SerializeField] private TextMeshProUGUI _textProgressTip;
    [SerializeField] private Color _colorNor;
    [SerializeField] private List<TipData> _tipDatas;
    [Space]
    public Image overlay;
    public Image imageItem;
    public TextMeshProUGUI textItem;
    [HideInInspector] public ItemType itemType;
    [HideInInspector] public int itemValue;
    [Header("UI TEST")]
    public bool isShowTest;
    [SerializeField] private Dropdown _levels;
    [SerializeField] private GameObject _reporter;
    [SerializeField] private GameObject _btnTest;
    [SerializeField] private GameObject _btnUnlockAllFlag;

    private List<LevelData> _levelDatas;
    private const int PLAY = 0;
    private const int FACEBOOK = 1;
    private bool isShowBtnTest;
    private float currProgress = 0;
    private float maxProgress = 100;

    public string CloseSceneName
    {
        get
        {
            return _closeScene;
        }
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (!CPlayerPrefs.HasKey("INSTALLED"))
        {
            Caching.ClearCache();
            CPlayerPrefs.DeleteAll();
            CPlayerPrefs.Save();
        }
        CPlayerPrefs.useRijndael(CommonConst.ENCRYPTION_PREFS);
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        //animatorScene.gameObject.SetActive(false);
        _loadingScreen.gameObject.SetActive(false);
        _overlayPauseGame.gameObject.SetActive(false);
        _textProgress.text = "";
    }

    void Start()
    {
        if (DonotDestroyOnLoad.instance == null && donotDestroyOnLoad != null)
            Instantiate(donotDestroyOnLoad);
        _safeArea.CheckSafeArea();
        LoadScenHomeWithProgress();

        _btnTest.gameObject.SetActive(isShowTest);
        _reporter.gameObject.SetActive(isShowTest);
        _btnUnlockAllFlag.gameObject.SetActive(isShowTest);
        if (isShowTest)
        {
            LoadOptionData();
            _levels.onValueChanged.RemoveAllListeners();
            _levels.onValueChanged.AddListener(OnUnlockLevel);
        }
    }

    public void OnClick(int index)
    {
        _btnPlay.interactable = false;
        switch (index)
        {
            case PLAY:
                GameState.currentWorld = Prefs.unlockedWorld;
                GameState.currentSubWorld = Prefs.unlockedSubWorld;
                GameState.currentLevel = Prefs.unlockedLevel;
                //GameState.countSpell = Prefs.countSpell;
                //Debug.Log(Prefs.unlockedWorld.ToString() + Prefs.unlockedWorld.ToString() + Prefs.unlockedLevel.ToString());

                CUtils.LoadScene(Const.SCENE_MAIN, false);
                break;
            case FACEBOOK:
                CUtils.LikeFacebookPage(ConfigController.Config.facebookPageID);
                break;
        }
        Sound.instance.Play(Sound.Others.PopupOpen);
    }

    public void ShowTitleHome(bool show)
    {
        if (!show)
        {
            _spineAnimEgg.SetAnimation(idleEgg, false);
            _spineAnimShadow.SetAnimation(idleEggShadow, false);
        }
        _spineAnimEgg.gameObject.SetActive(show);
        _spineAnimShadow.gameObject.SetActive(show);
        _btnPlay.gameObject.SetActive(show);
        _maskShadow.gameObject.SetActive(show);
    }

    public void SceneClose(Action callback)
    {
        //Sound.instance.Play(Sound.Scenes.CurtainClose);
        //animatorScene.gameObject.SetActive(true);
        //animatorScene.SetBool(_closeScene, true);
        ShowTip(true);
        ScreenFader.instance.DelayCall(1.8f, () =>
        {
            ShowTitleHome(false);
            callback?.Invoke();
        });
    }

    public void SceneOpen(Action callback = null)
    {
        //Sound.instance.Play(Sound.Scenes.CurtainOpen);
        animatorScene.gameObject.SetActive(true);
        animatorScene.SetBool(_closeScene, false);
        ScreenFader.instance.DelayCall(0.4f, () =>
        {
            callback?.Invoke();
        });
    }

    public void LoadScenHomeWithProgress()
    {
        _textProgress.text = "";
        StartCoroutine(RunProgressLoading());
    }

    public IEnumerator RunProgressLoading()
    {
        //_progressLoading.value = 0;
        currProgress = 0;
        _loadingScreen.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        var isTut = CPlayerPrefs.GetBool("TUTORIAL", false);
        var isFirstGame = CPlayerPrefs.HasKey("INSTALLED");
        if (!isTut && !isFirstGame)
        {
            GameState.currentWorld = Prefs.unlockedWorld;
            GameState.currentSubWorld = Prefs.unlockedSubWorld;
            GameState.currentLevel = Prefs.unlockedLevel;
        }
        var asyncOp = SceneManager.LoadSceneAsync((GameState.currentLevel == 0 && GameState.currentSubWorld == 0 && GameState.currentWorld == 0 && !isTut && !isFirstGame) ? Const.SCENE_MAIN : Const.SCENE_HOME);
        asyncOp.allowSceneActivation = false;
        while (currProgress < maxProgress)
        {
            currProgress = asyncOp.progress * 100;
            _textProgress.text = "Loading " + (int)currProgress + "%";
            if (asyncOp.progress >= 0.9f)
            {
                currProgress = maxProgress;
                _textProgress.text = "Loading 100%";
                if (GameState.currentLevel == 0 && GameState.currentSubWorld == 0 && GameState.currentWorld == 0 && !isTut && !isFirstGame)
                {
                    ShowTitleHome(false);
                    _loadingScreen.gameObject.SetActive(false);
                    _bgFirstLoading.gameObject.SetActive(false);
                }
                asyncOp.allowSceneActivation = true;
                //_loadingScreen.gameObject.SetActive(false);
            }
            yield return null;
        }
    }

    public IEnumerator ShowLoadingProgress(string nameScene)
    {
        currProgress = 0;
        yield return new WaitForSeconds(0.1f);
        var asyncOp = SceneManager.LoadSceneAsync(nameScene);
        asyncOp.allowSceneActivation = false;
        while (currProgress < maxProgress)
        {
            currProgress = asyncOp.progress * 100;
            _textProgressTip.text = "Loading " + (int)currProgress + "%";
            if (asyncOp.progress >= 0.9f)
            {
                currProgress = maxProgress;
                _textProgressTip.text = "Loading 100%";
                asyncOp.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void ShowTip(bool show, Action callback = null)
    {
        if (_bgFirstLoading.gameObject.activeInHierarchy)
            _bgFirstLoading.gameObject.SetActive(false);
        var tweenControl = TweenControl.GetInstance();
        if (show)
        {
            var randomTemp = CheckTip();
            var tipRandom = _tipDatas[UnityEngine.Random.Range(0, randomTemp.Count)];
            _textTip.text = tipRandom.contentTip;
            //_imgTip.sprite = tipRandom.iconTip;
            //_imgTip.SetNativeSize();
            _textTip.color = _colorNor;
            //_imgTip.color = new Color(1, 1, 1, 1);
            _animTip.color = new Color(1, 1, 1, 1);
            _bgLoading.color = new Color(1, 1, 1, 1);
            _bgLoading.gameObject.SetActive(true);
            _textProgressTip.text = "Loading " + 0 + "%";
        }
        else
        {
            tweenControl.FadeAnfaText(_textTip, 0, 0.5f);
            tweenControl.FadeAnfa(_animTip, 0, 0.5f);
            tweenControl.FadeAnfa(_bgLoading, 0, 0.5f, () =>
            {
                _bgLoading.gameObject.SetActive(false);
                callback?.Invoke();
            });
        }
    }

    private List<TipData> CheckTip()
    {
        var results = new List<TipData>();
        var gameData = Resources.Load<GameData>("GameData");
        var world = GameState.currentWorld;
        var subWorld = GameState.currentSubWorld;
        var level = GameState.currentLevel;
        var numlevels = Utils.GetNumLevels(world, subWorld);
        var currlevel = (level + numlevels * subWorld + world * gameData.words[0].subWords.Count * numlevels);

        foreach (var tipItem in _tipDatas)
        {
            if (tipItem.tipType == TipType.SELECTED_HINT && !CPlayerPrefs.HasKey("SELECTED_HINT_TUTORIAL") && currlevel < 23)
                results.Add(tipItem);
            else if (tipItem.tipType == TipType.SELECTED_HINT && !CPlayerPrefs.HasKey("MULTIPLE_HINT_TUTORIAL") && currlevel < 30)
                results.Add(tipItem);
            else if (tipItem.tipType == TipType.SELECTED_HINT && !CPlayerPrefs.HasKey("BEE_TUTORIAL") && currlevel < 39)
                results.Add(tipItem);
            else if (tipItem.tipType == TipType.NORMAL)
                results.Add(tipItem);
        }
        return results;
    }

    //private void OnApplicationQuit()
    //{
    //    CPlayerPrefs.SetBool("First_Load", false);
    //}

    //TEST
    public void OnUnlockAllFlag()
    {
        CPlayerPrefs.SetBool("UNLOCK_ALL_FLAG", true);
        CPlayerPrefs.SetBool("HONEY_TUTORIAL", true);
    }

    public void OnClickFullBonusBox()
    {
        Prefs.extraProgress = Prefs.totalExtraAdded = Prefs.extraTarget;
    }

    public void OnAddStarAndBeeTest(int numBee)
    {
        CurrencyController.CreditBalance(10000);
        CurrencyController.CreditHintFree(3);
        //CPlayerPrefs.SetBool("HINT_TUTORIAL", true);
        //CPlayerPrefs.SetBool("SELECTED_HINT_TUTORIAL", true);
        //CPlayerPrefs.SetBool("MULTIPLE_HINT_TUTORIAL", true);
        //CPlayerPrefs.SetBool("BEE_TUTORIAL", true);
        BeeManager.instance.CreaditAmountBee(numBee);
        FacebookController.instance.HoneyPoints += 10000;
        FacebookController.instance.onChangedHoneyPoints?.Invoke();
        FacebookController.instance.SaveDataGame();
        if (HomeController.instance != null)
            HomeController.instance.ShowChickenBank();
    }

    public void ShowHidenBtn(RectTransform rectTransform)
    {
        var tweenControl = TweenControl.GetInstance();
        isShowBtnTest = !isShowBtnTest;
        if (isShowBtnTest)
            tweenControl.MoveRectX(rectTransform, rectTransform.anchoredPosition.x - rectTransform.sizeDelta.x, 0.3f);
        else
            tweenControl.MoveRectX(rectTransform, rectTransform.anchoredPosition.x + rectTransform.sizeDelta.x, 0.3f);
    }

    public void OnUnlockLevel(int value)
    {
        var gameData = Resources.Load<GameData>("GameData");
        var numlevels = Utils.GetNumLevels(Prefs.unlockedWorld, Prefs.unlockedSubWorld);
        var currlevel = (GameState.currentLevel + numlevels * GameState.currentSubWorld + gameData.words[0].subWords.Count * numlevels * GameState.currentWorld) + 1;
        var lastWord = gameData.words[gameData.words.Count - 1];
        var lastLevel = lastWord.subWords[lastWord.subWords.Count - 1].gameLevels[lastWord.subWords[lastWord.subWords.Count - 1].gameLevels.Count - 1];

        var data = _levelDatas[value];
        for (int j = 0; j < lastLevel.level; j++)
        {
            var level = j + 1;
            for (int i = 0; i < 10; i++)
            {
                var lineindex = i;
                CPlayerPrefs.DeleteKey("LineWord(Clone)" + lineindex + "_" + level);
            }
        }
        Prefs.unlockedLevel = GameState.unlockedLevel = data.level;
        Prefs.unlockedSubWorld = GameState.unlockedSubWord = data.chapter;
        Prefs.unlockedWorld = GameState.unlockedWorld = data.word;
        Prefs.IsLevelEnd = false;

        FacebookController.instance.user.unlockedLevel = Prefs.unlockedLevel.ToString();
        FacebookController.instance.user.unlockedWorld = Prefs.unlockedWorld.ToString();
        FacebookController.instance.user.unlockedSubWorld = Prefs.unlockedSubWorld.ToString();
        FacebookController.instance.user.levelProgress = new string[] { "0" };
        FacebookController.instance.user.answerProgress = new string[] { "0" };
        FacebookController.instance.SaveDataGame();
    }

    private void LoadOptionData()
    {
        _levelDatas = new List<LevelData>();
        _levels.ClearOptions();
        var gameData = Resources.Load<GameData>("GameData");
        var numlevels = Utils.GetNumLevels(Prefs.unlockedWorld, Prefs.unlockedSubWorld);

        var optData = new List<Dropdown.OptionData>();
        var indexWord = 0;
        foreach (var word in gameData.words)
        {
            var indexChapter = 0;
            foreach (var chapter in word.subWords)
            {
                var indexLevel = 0;
                foreach (var level in chapter.gameLevels)
                {
                    var currlevel = (indexLevel + numlevels * indexChapter + word.subWords.Count * numlevels * indexWord) + 1;
                    var levelData = new LevelData();
                    levelData.level = indexLevel;
                    levelData.chapter = indexChapter;
                    levelData.word = indexWord;
                    var optionData = new Dropdown.OptionData("Level " + currlevel);
                    optData.Add(optionData);
                    _levelDatas.Add(levelData);
                    indexLevel++;
                }
                indexChapter++;
            }
            indexWord++;
        }
        _levels.AddOptions(optData);
    }
    //===

    private void OnApplicationPause(bool pause)
    {
        _overlayPauseGame.gameObject.SetActive(pause);
    }

    public void ShowOverLayPauseGame(bool show)
    {
        _overlayPauseGame.gameObject.SetActive(show);
    }

    public IEnumerator ShowEffectCollect(int value, Transform btnTarget = null)
    {
        if (MainController.instance != null)
            MainController.instance.canvasCollect.gameObject.SetActive(true);
        var result = value / 5;
        for (int i = 0; i < value; i++)
        {
            if (i < 5)
            {
                MonoUtils.instance.ShowEffect(result, null, null, btnTarget);
            }
            yield return new WaitForSeconds(0.06f);
        }
    }

    public void ShowItemCollect()
    {
        overlay.gameObject.SetActive(true);
        TweenControl.GetInstance().MoveRectY(imageItem.transform as RectTransform, 100f, 1);
        TweenControl.GetInstance().FadeAnfa(imageItem, 1, 0.3f, () =>
        {
            TweenControl.GetInstance().FadeAnfa(imageItem, 0, 0.7f, () =>
            {
                overlay.gameObject.SetActive(false);
            });
        });
        TweenControl.GetInstance().FadeAnfa(textItem, 1, 0.3f, () =>
        {
            TweenControl.GetInstance().FadeAnfa(textItem, 0, 0.7f, () =>
            {
                //overlay.gameObject.SetActive(false);
            });
        });
    }

    [Serializable]
    public class LevelData
    {
        public int level;
        public int chapter;
        public int word;
    }
}

[Serializable]
public class TipData
{
    public TipType tipType;
    public string contentTip;
}

public enum TipType
{
    NORMAL,
    SELECTED_HINT,
    MULTIPLE_HINT,
    BEEHIVE
}