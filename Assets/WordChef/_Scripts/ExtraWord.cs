using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;

public class ExtraWord : MonoBehaviour
{
    public List<string> extraWords = new List<string>();
    public GameObject existMessage;
    public Transform beginPoint, endPoint;
    public GameObject lightEffect, lightOpenEffect, btnExtra;
    public ParticleSystem effectLight;
    public ParticleSystem effectLightLoop;

    private int world, subWorld, level;
    private CanvasGroup existMessageCG;
    private bool isMessageShowing;
    private Text flyText;

    public static ExtraWord instance;

    private void Awake()
    {
        instance = this;
        DialogController.instance.onDialogsOpened += OnDialogOpened;
        DialogController.instance.onDialogsClosed += OnDialogClosed;
    }

    private void OnDialogClosed()
    {
        if (effectLightLoop != null)
            effectLightLoop.gameObject.SetActive(false);
    }

    private void OnDialogOpened()
    {
        UpdateUI();
    }

    private void Start()
    {
        world = GameState.currentWorld;
        subWorld = GameState.currentSubWorld;
        level = GameState.currentLevel;

        extraWords = Prefs.IsSaveLevelProgress() ? Prefs.GetExtraWords(world, subWorld, level).ToList() : new List<string>();
        existMessage.SetActive(false);
        existMessageCG = existMessage.GetComponent<CanvasGroup>();

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (lightOpenEffect != null) lightOpenEffect.SetActive(Prefs.extraProgress >= Prefs.extraTarget);
        if (effectLightLoop == null)
            return;
        if (Prefs.extraProgress >= Prefs.extraTarget)
            effectLightLoop.gameObject.SetActive(true);
        else
            effectLightLoop.gameObject.SetActive(false);
    }

    public void ProcessWorld(string word)
    {
        if (extraWords.Contains(word))
        {
            if (isMessageShowing) return;
            isMessageShowing = true;

            ShowMessage("");
        }
        else
        {
            var tweenControl = TweenControl.GetInstance();
            var middlePoint = CUtils.GetMiddlePoint(beginPoint.position, endPoint.position, 0.4f);
            Vector3[] waypoint = { beginPoint.position, middlePoint, endPoint.position };

            flyText = Instantiate(MonoUtils.instance.letter);
            flyText.text = word[0].ToString();
            flyText.fontSize = 12;
            flyText.transform.position = beginPoint.position;
            flyText.transform.SetParent(MonoUtils.instance.textFlyTransform);
            flyText.transform.localScale = TextPreview.instance.textGrid.transform.localScale * 0.75f;
            //iTween.MoveTo(flyText.gameObject, iTween.Hash("path", waypoint, "time", 0.3f, "oncomplete", "OnTextMoveToComplete", "oncompletetarget", gameObject));
            tweenControl.JumpRect(flyText.transform as RectTransform, btnExtra.transform.localPosition, 100f, 1, 0.3f, false, OnTextMoveToComplete);
            tweenControl.Scale(flyText.gameObject, Vector3.zero, 0.3f);
            AddNewExtraWord(word);
        }
    }

    private void ShowMessage(string message)
    {
        existMessage.SetActive(true);
        existMessageCG.alpha = 0;
        iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", 0.3f, "OnUpdate", "OnMessageUpdate", "oncomplete", "OnMessageShowComplete"));
    }

    public void AddNewExtraWord(string word)
    {
        extraWords.Add(word);
        if (Prefs.IsSaveLevelProgress())
        {
            Prefs.SetExtraWords(world, subWorld, level, extraWords.ToArray());
            Prefs.extraProgress++;
            Prefs.totalExtraAdded++;
        }
    }

    private void OnMessageUpdate(float value)
    {
        existMessageCG.alpha = value;
    }

    private void OnMessageShowComplete()
    {
        Timer.Schedule(this, 0.5f, () =>
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", 1, "to", 0, "time", 0.3f, "OnUpdate", "OnMessageUpdate", "oncomplete", "OnMessageHideComplete"));
        });
    }

    private void OnMessageHideComplete()
    {
        isMessageShowing = false;
    }

    private void OnTextMoveToComplete()
    {
        UpdateUI();

        effectLight.gameObject.SetActive(true);
        effectLight.Play();
        TweenControl.GetInstance().DelayCall(transform, 2, OnLightRotateComplete);

        //if (!lightOpenEffect.activeSelf)
        //{
        //    lightEffect.SetActive(true);
        //    iTween.RotateAdd(lightEffect, iTween.Hash("z", -60, "time", 0.4f, "oncomplete", "OnLightRotateComplete", "oncompletetarget", gameObject));
        //}

        TweenControl.GetInstance().Shake(btnExtra, 0.3f, Vector3.one * 10f, 20, ShakeType.ShakeTypeRotate, 180, false, true);

        flyText.CrossFadeAlpha(0, 0.3f, true);
        Destroy(flyText.gameObject, 0.3f);
    }

    private void OnLightRotateComplete()
    {
        //lightEffect.SetActive(false);
        effectLight.gameObject.SetActive(false);
        UpdateUI();
    }

    public void OnClaimed()
    {
        UpdateUI();
    }
}
