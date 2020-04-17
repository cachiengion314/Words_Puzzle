using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneAnimate : MonoBehaviour
{
    public static SceneAnimate Instance { get; private set; }

    [SerializeField] private string _closeScene;
    public Animator animatorScene;
    public GameObject _loadingScreen;
    public Slider _progressLoading;
    public GameObject btnTest;

    private bool isShowBtnTest;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        animatorScene.gameObject.SetActive(false);
        _loadingScreen.gameObject.SetActive(false);
    }

    public void SceneClose(Action callback)
    {
        Sound.instance.Play(Sound.Scenes.CurtainClose);
        animatorScene.gameObject.SetActive(true);
        animatorScene.SetBool(_closeScene, true);
        ScreenFader.instance.DelayCall(1.8f, () =>
        {
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
            if (asyncOp.progress >= 0.9f)
            {
                _progressLoading.value = _progressLoading.maxValue;
                asyncOp.allowSceneActivation = true;
                _loadingScreen.gameObject.SetActive(false);
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
        CurrencyController.CreditBalance(9999999);
        CurrencyController.CreditHintFree(3);
        BeeManager.instance.SetAmountBee(numBee);
    }

    public void ShowHidenBtn(RectTransform rectTransform)
    {
        var tweenControl = TweenControl.GetInstance();
        isShowBtnTest = !isShowBtnTest;
        if (isShowBtnTest)
            tweenControl.MoveRectX(rectTransform, 0, 0.3f);
        else
            tweenControl.MoveRectX(rectTransform, -rectTransform.sizeDelta.x, 0.3f);
    }
    //===
}
