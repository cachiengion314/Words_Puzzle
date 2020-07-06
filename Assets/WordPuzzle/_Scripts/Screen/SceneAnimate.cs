using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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
    [SerializeField] private Image _bgLoading;
    [SerializeField] private Image _imgTip;
    [SerializeField] private Text _textTip;
    [SerializeField] private Color _colorNor;
    [SerializeField] private List<TipData> _tipDatas;

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
        _textProgress.text = "";
    }

    void Start()
    {
        if (DonotDestroyOnLoad.instance == null && donotDestroyOnLoad != null)
            Instantiate(donotDestroyOnLoad);
        LoadScenHomeWithProgress();
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
        Sound.instance.Play(Sound.Scenes.CurtainClose);
        //animatorScene.gameObject.SetActive(true);
        //animatorScene.SetBool(_closeScene, true);
        ShowTip(true);
        ScreenFader.instance.DelayCall(1.8f, () =>
        {
            ShowTitleHome(false);
            animEvent.EventAnimCallback();
            callback?.Invoke();
        });
    }

    public void SceneOpen(Action callback = null)
    {
        Sound.instance.Play(Sound.Scenes.CurtainOpen);
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
            var tipRandom = _tipDatas[UnityEngine.Random.Range(0, _tipDatas.Count)];
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
    //===
}

[Serializable]
public class TipData
{
    public Sprite iconTip;
    public string contentTip;
}