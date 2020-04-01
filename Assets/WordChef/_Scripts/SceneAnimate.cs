using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAnimate : MonoBehaviour
{
    public static SceneAnimate Instance { get; private set; }

    [SerializeField] private string _closeScene;
    public Animator animatorScene;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        animatorScene.gameObject.SetActive(false);
    }

    public void SceneClose(Action callback)
    {
        animatorScene.ResetTrigger(_closeScene);
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
        ScreenFader.instance.DelayCall(0.3f, () =>
        {
            callback?.Invoke();
        });
    }
}
