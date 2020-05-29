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
    public string _showAnim = "animation";
    public string _loopAnim = "Loop";
    public string _showShadow = "Shadow";
    public string _showShadowLoop = "Shadow Loop";
    public string _showgiado = "animation2";

    private const int PLAY = 0;
    private const int FACEBOOK = 1;
    private bool isShowBtnTest;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        animatorScene.gameObject.SetActive(false);
        _loadingScreen.gameObject.SetActive(false);
        _textProgress.text = "";
    }

    public void OnClick(int index)
    {
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

    private void Update()
    {
        if(gameObject.GetComponent<Canvas>().worldCamera == null)
            gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    public void ShowTitleHome(bool show)
    {
        _spineAnimEgg.gameObject.SetActive(show);
        _spineAnimShadow.gameObject.SetActive(show);
        _btnPlay.gameObject.SetActive(show);
        _maskShadow.gameObject.SetActive(show);
    }

    public void SceneClose(Action callback)
    {
        Sound.instance.Play(Sound.Scenes.CurtainClose);
        animatorScene.gameObject.SetActive(true);
        animatorScene.SetBool(_closeScene, true);
        ScreenFader.instance.DelayCall(1.8f, () =>
        {
            ShowTitleHome(false);
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
        var asyncOp = SceneManager.LoadSceneAsync(Const.SCENE_HOME);
        asyncOp.allowSceneActivation = false;
        while (_progressLoading.value < _progressLoading.maxValue)
        {
            _progressLoading.value = asyncOp.progress * 100;
            _textProgress.text = "Loading " + (int)_progressLoading.value + "%";
            if (asyncOp.progress >= 0.9f)
            {
                _progressLoading.value = _progressLoading.maxValue;
                _textProgress.text = "Loading 100%";
                asyncOp.allowSceneActivation = true;
                //_loadingScreen.gameObject.SetActive(false);
            }
            yield return null;
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
