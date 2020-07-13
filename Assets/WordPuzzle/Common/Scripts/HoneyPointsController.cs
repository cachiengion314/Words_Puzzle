﻿using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class HoneyPointsController : MonoBehaviour
{
    public static HoneyPointsController instance;
    public Action onChangedHoneyPoints;
    public Action showHoneyPointsInThisLevel;

    private bool isGameplayEnd;
    private int timePoints;

    public GameObject honeyFrame;
    public TextMeshProUGUI honeyTxt;
    public TextMeshProUGUI visualHoneyPointsTxt;

    private int totalTitlePoints;
    private int[] titlePointsArray = new int[12] { 0, 0, 1, 2, 3, 4, 4, 4, 4, 4, 4, 4 };
    private int lineIndex;
    public int LineIndex
    {
        get
        {
            return lineIndex;

        }
        set
        {
            lineIndex = value;
            if (lineIndex > 1 && Prefs.IsSaveLevelProgress())
            {
                int titlePoints = titlePointsArray[lineIndex];
                totalTitlePoints = TotalTitlePoint(lineIndex, titlePointsArray);
                honeyTxt.text = (FacebookController.instance.HoneyPoints + totalTitlePoints).ToString();

                ShowAndFade(titlePoints, 0, visualHoneyPointsTxt);

                TweenControl.GetInstance().Scale(honeyTxt.gameObject, Vector3.one * 1.2f, .3f,
            () => { TweenControl.GetInstance().Scale(honeyTxt.gameObject, Vector3.one, .3f); });
            }
            else if (lineIndex == 0 && Prefs.IsSaveLevelProgress())
            {
                totalTitlePoints = TotalTitlePoint(LineIndex, titlePointsArray);
                honeyTxt.text = (FacebookController.instance.HoneyPoints + totalTitlePoints).ToString();
            }
        }
    }

    private float timeLeft = 20f;
    private float timeGameplay = 20f;
    private int numberOfWordsInLevelWithoutExtra;
    public int NumberOfWordsInLevelWithoutExtra
    {
        get
        {
            return numberOfWordsInLevelWithoutExtra;
        }
        set
        {
            numberOfWordsInLevelWithoutExtra = value;

            if (numberOfWordsInLevelWithoutExtra <= 3) { timeLeft = 60f; }
            else
            {
                int deltaNumber = numberOfWordsInLevelWithoutExtra - 3;
                timeLeft = 60f + deltaNumber * 30f;
            }
            timeGameplay = timeLeft;
        }
    }
    private void Awake()
    {
        instance = this;

    }
    void Update()
    {
        if (isGameplayEnd) return;

        timeLeft -= Time.deltaTime;
        //timeTxt.text = "TimeLeft: " + timeLeft.ToString("0");

        if (timeLeft < timeGameplay && timeLeft > 0)
        {
            timePoints = 10;
        }
        else if (timeLeft < 0 && timeLeft > -timeGameplay)
        {
            timePoints = Mathf.RoundToInt(10 - 10 * (Mathf.Abs(timeLeft)) / timeGameplay);
        }
        else
        {
            timePoints = 0;
        }
    }
    public void ShowHoneyPoints()
    {
        int honeyPoints;
        if (Prefs.IsSaveLevelProgress())
        {
            // Winning newest level
            totalTitlePoints = TotalTitlePoint(LineIndex, titlePointsArray);
            honeyPoints = 10 + WordRegion.instance.listWordCorrect.Count + totalTitlePoints + timePoints;
            int honeyWithoutTitlePoints = honeyPoints - totalTitlePoints;

            WinDialog.instance.honeyPoints = honeyPoints;
            StartCoroutine(ShowAndFadeDelay(honeyWithoutTitlePoints, visualHoneyPointsTxt));

            FacebookController.instance.HoneyPoints += honeyPoints;
            FacebookController.instance.SaveDataGame();
        }
        else
        {
            // Winning old level
        }
        isGameplayEnd = true;
    }
    private int TotalTitlePoint(int lineIndex, int[] titlePointsArray)
    {
        int total = 0;
        for (int i = 0; i < titlePointsArray.Length; i++)
        {
            total += titlePointsArray[i];
            if (lineIndex == i || lineIndex >= titlePointsArray.Length)
            {
                break;
            }
        }
        return total;
    }
    private IEnumerator ShowAndFadeDelay(int value, TextMeshProUGUI textCollect, float duration = 0.5f)
    {
        yield return new WaitForSeconds(.5f);

        onChangedHoneyPoints?.Invoke();

        var tweenControl = TweenControl.GetInstance();
        (textCollect).text = "X" + value;
        tweenControl.FadeAnfaText(textCollect, 1, duration, () => { tweenControl.FadeAnfaText(textCollect, 0, duration); });
    }
    public void ShowAndFade(int value, float delay, TextMeshProUGUI textCollect, float duration = 0.5f)
    {
        var tweenControl = TweenControl.GetInstance();
        (textCollect).text = "X" + value;

        tweenControl.DelayCall(textCollect.transform, delay,
            () =>
            {
                tweenControl.FadeAnfaText(textCollect, 1, duration, () => { tweenControl.FadeAnfaText(textCollect, 0, duration); });
            }
            );

    }
}
