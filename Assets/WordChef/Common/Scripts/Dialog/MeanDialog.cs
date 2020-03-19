using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MeanDialog : Dialog
{
    public string wordName;
    public string wordMean;
    public GameObject wordNameText;
    public GameObject wordMeantext;
    //public static GameObject instance;

    public void Start()
    {
        base.Start();
        showMean();
    }
    public void showMean()
    {
        wordNameText.GetComponent<TextMeshProUGUI>().text = wordName;
        wordMeantext.GetComponent<TextMeshProUGUI>().text = wordMean;
    }

}
