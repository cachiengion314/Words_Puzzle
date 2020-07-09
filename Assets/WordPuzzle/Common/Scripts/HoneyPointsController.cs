using Google.Protobuf;
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

    private bool isGameplayEnd;
    private int timePoints;
    public TextMeshProUGUI timeTxt;
    public TextMeshProUGUI honeyPointsTxt;
    public TextMeshProUGUI titlePointsTxt;
    public TextMeshProUGUI wordCountPointsTxt;
    public TextMeshProUGUI timePointsTxt;

    public GameObject honeyFrame;
    public TextMeshProUGUI honeyTxt;

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
                totalTitlePoints = TotalTitlePoint(lineIndex, titlePointsArray);
                var point = 10 + WordRegion.instance.listWordCorrect.Count + totalTitlePoints + timePoints;
                FacebookController.instance.HoneyPoints += point;
                FacebookController.instance.SaveDataGame();
                onChangedHoneyPoints?.Invoke();
                TweenControl.GetInstance().Scale(honeyTxt.gameObject, Vector3.one * 2f, .2f,
            () => { TweenControl.GetInstance().Scale(honeyTxt.gameObject, Vector3.one, .2f); });
            }
            //else if (lineIndex == 1)
            //{
            //    honeyTxt.text = "0";
            //}
            //else if (lineIndex == 0)
            //{
            //    honeyTxt.text = "0";
            //    TweenControl.GetInstance().Scale(honeyTxt.gameObject, Vector3.one * 2f, .1f,
            //() => { TweenControl.GetInstance().Scale(honeyTxt.gameObject, Vector3.one, .1f); });
            //}

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
        timeTxt.text = "TimeLeft: " + timeLeft.ToString("0");

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
    public void ShowHoneyPoints(GameObject winDialog)
    {
        SetupTextUI(winDialog);
        int honeyPoints;

        if (Prefs.IsSaveLevelProgress())
        {
            // Winning newest level
            totalTitlePoints = TotalTitlePoint(LineIndex, titlePointsArray);
            honeyPoints = 10 + WordRegion.instance.listWordCorrect.Count + totalTitlePoints + timePoints;
            honeyPointsTxt.text = "Honey: " + honeyPoints.ToString();
            titlePointsTxt.text = "TitlePts: " + totalTitlePoints.ToString();
            wordCountPointsTxt.text = "WordPts: " + WordRegion.instance.listWordCorrect.Count.ToString();
            timePointsTxt.text = "TimePts: " + timePoints;

            //FacebookController.instance.HoneyPoints += honeyPoints;
        }
        else
        {
            // Winning old level
            honeyPoints = 0;
            honeyPointsTxt.text = "Honey: " + honeyPoints.ToString();
            //FacebookController.instance.HoneyPoints += honeyPoints;
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
    private void SetupTextUI(GameObject winDialog)
    {
        honeyPointsTxt.transform.SetParent(winDialog.transform);
        timeTxt.transform.SetParent(winDialog.transform);
        titlePointsTxt.transform.SetParent(winDialog.transform);
        wordCountPointsTxt.transform.SetParent(winDialog.transform);
        timePointsTxt.transform.SetParent(winDialog.transform);

        honeyPointsTxt.transform.localScale = Vector3.one;
        timeTxt.transform.localScale = Vector3.one;
        titlePointsTxt.transform.localScale = Vector3.one;
        wordCountPointsTxt.transform.localScale = Vector3.one;
        timePointsTxt.transform.localScale = Vector3.one;

        honeyPointsTxt.rectTransform.sizeDelta = new Vector2(400, 200);
        timeTxt.rectTransform.sizeDelta = new Vector2(400, 200);
        titlePointsTxt.rectTransform.sizeDelta = new Vector2(400, 200);
        wordCountPointsTxt.rectTransform.sizeDelta = new Vector2(400, 200);
        timePointsTxt.rectTransform.sizeDelta = new Vector2(400, 200);

        Vector3 offset = new Vector3(0, 200, 0);

        honeyPointsTxt.rectTransform.localPosition = new Vector3(365, -732) + offset;
        timeTxt.rectTransform.localPosition = new Vector3(349, -824) + offset;
        titlePointsTxt.rectTransform.localPosition = new Vector3(-400, -830) + offset;
        wordCountPointsTxt.rectTransform.localPosition = new Vector3(-407, -920) + offset;
        timePointsTxt.rectTransform.localPosition = new Vector3(347, -915) + offset;
    }
}
