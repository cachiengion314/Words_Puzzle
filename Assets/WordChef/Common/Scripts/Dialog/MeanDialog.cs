using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MeanDialog : Dialog
{
    public static string wordName;
    public static string wordMean;
    public GameObject wordNameText;
    public GameObject wordMeantext;
    //public static GameObject instance;

    public void Start()
    {
        base.Start();
        TweenControl.GetInstance().DelayCall(transform, 0.1f, () =>
        {
            showMean();

        });
    }
    public void showMean()
    {
        wordNameText.GetComponent<TextMeshProUGUI>().text = wordName;
        wordMeantext.GetComponent<TextMeshProUGUI>().text = wordMean;
    }

}
