using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Superpow;

public class SceneAnimate : MonoBehaviour
{
    public static SceneAnimate Instance { get; private set; }

    public GameObject donotDestroyOnLoad;
    public AnimEvent animEvent;
    [Space]
    [SerializeField] private string _closeScene;
    public Animator animatorScene;
    public GameObject _loadingScreen;
    public Slider _progressLoading;
    public GameObject btnTest;
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
    [SerializeField] private Image _bgLoading;
    [SerializeField] private Image _imgTip;
    [SerializeField] private Text _textTip;
    [SerializeField] private Color _colorNor;
    [SerializeField] private List<TipData> _tipDatas;
    [Header("UI TEST")]
    [SerializeField] private Dropdown _levels;

    private List<LevelData> _levelDatas;
    private const int PLAY = 0;
    private const int FACEBOOK = 1;
    private bool isShowBtnTest;

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
        LoadScenHomeWithProgress();
        LoadOptionData();
        _levels.onValueChanged.AddListener(OnUnlockLevel);
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
        _progressLoading.value = 0;
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
        while (_progressLoading.value < _progressLoading.maxValue)
        {
            _progressLoading.value = asyncOp.progress * 100;
            _textProgress.text = "Loading " + (int)_progressLoading.value + "%";
            if (asyncOp.progress >= 0.9f)
            {
                _progressLoading.value = _progressLoading.maxValue;
                _textProgress.text = "Loading 100%";
                if (GameState.currentLevel == 0 && GameState.currentSubWorld == 0 && GameState.currentWorld == 0 && !isTut && !isFirstGame)
                {
                    ShowTitleHome(false);
                    _loadingScreen.gameObject.SetActive(false);
                }
                asyncOp.allowSceneActivation = true;
                //_loadingScreen.gameObject.SetActive(false);
            }
            yield return null;
        }
    }

    public void ShowTip(bool show, Action callback = null)
    {
        var tweenControl = TweenControl.GetInstance();
        if (show)
        {
            var randomTemp = CheckTip();
            var tipRandom = _tipDatas[UnityEngine.Random.Range(0, randomTemp.Count)];
            _textTip.text = tipRandom.contentTip;
            _imgTip.sprite = tipRandom.iconTip;
            _imgTip.SetNativeSize();
            _textTip.color = _colorNor;
            _imgTip.color = new Color(1, 1, 1, 1);
            _bgLoading.color = new Color(1, 1, 1, 1);
            _bgLoading.gameObject.SetActive(true);
        }
        else
        {
            tweenControl.FadeAnfaText(_textTip, 0, 0.5f);
            tweenControl.FadeAnfa(_imgTip, 0, 0.5f);
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
    public void OnAddStarAndBeeTest(int numBee)
    {
        CurrencyController.CreditBalance(10000);
        CurrencyController.CreditHintFree(3);
        CPlayerPrefs.SetBool("HINT_TUTORIAL", true);
        CPlayerPrefs.SetBool("SELECTED_HINT_TUTORIAL", true);
        CPlayerPrefs.SetBool("MULTIPLE_HINT_TUTORIAL", true);
        CPlayerPrefs.SetBool("BEE_TUTORIAL", true);
        BeeManager.instance.CreaditAmountBee(numBee);
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
        var data = _levelDatas[value];
        Prefs.unlockedLevel = GameState.unlockedLevel = data.level;
        Prefs.unlockedSubWorld = GameState.unlockedSubWord = data.chapter;
        Prefs.unlockedWorld = GameState.unlockedWorld = data.word;

        FacebookController.instance.user.unlockedLevel = Prefs.unlockedLevel.ToString();
        FacebookController.instance.user.unlockedWorld = Prefs.unlockedWorld.ToString();
        FacebookController.instance.user.unlockedSubWorld = Prefs.unlockedSubWorld.ToString();
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
    public Sprite iconTip;
    public string contentTip;
}

public enum TipType
{
    NORMAL,
    SELECTED_HINT,
    MULTIPLE_HINT,
    BEEHIVE
}