using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.Internal;
using System;

public class FlagMeanDialog : Dialog
{
    public static int indexOfFlagWhenClick;

    public TextMeshProUGUI countryNameTxt;
    public TextMeshProUGUI subRegionTxt;
    public TextMeshProUGUI capitalTxt;
    public TextMeshProUGUI areaTxt;
    public TextMeshProUGUI populationTxt;
    [Space]
    public Image flagImg;
    public TextMeshProUGUI titleNameTxt;
    [Space]
    public GameObject flagMeanItems;
    public GameObject noInternet;
    [Space]
    [Header("Default image when loading is not finish yet")]
    public Sprite defaultImage;

    private readonly string COUNTRY_NAME = "name";
    private readonly string SUB_REGION = "subregion";
    private readonly string CAPITAL = "capital";
    private readonly string AREA = "area";
    private readonly string POPULATION = "population";
    public IEnumerator OnOpenFlagMeanDialog()
    {
        DictionaryDialog.instance.OverLayDialog.SetActive(true);

        TweenControl.GetInstance().ScaleFromZero(DictionaryDialog.instance.flagMeanDialog.gameObject, 0.3f);
        Sound.instance.Play(Sound.Others.PopupOpen);

        flagImg.sprite = FlagTabController.instance.bigFlags[DictionaryDialog.instance.flagList[indexOfFlagWhenClick].indexOfBigFlagImage];
        flagImg.SetNativeSize();

        titleNameTxt.text = "Loading...";
        countryNameTxt.text = "Loading... ";
        subRegionTxt.text = "Loading... ";
        capitalTxt.text = "Loading... ";
        areaTxt.text = "Loading... ";
        populationTxt.text = "Loading... ";

        yield return new WaitUntil(() => FlagTabController.instance.isGetCountryRequestDone);

        if (FlagTabController.instance.countryInfo.Count > 0)
        {
            titleNameTxt.text = CheckNullObject(FlagTabController.instance.countryInfo[COUNTRY_NAME]);
            countryNameTxt.text = CheckNullObject(FlagTabController.instance.countryInfo[COUNTRY_NAME]);
            subRegionTxt.text = CheckNullObject(FlagTabController.instance.countryInfo[SUB_REGION]);
            capitalTxt.text = CheckNullObject(FlagTabController.instance.countryInfo[CAPITAL]);
            areaTxt.text = (float.Parse(CheckNullObject(FlagTabController.instance.countryInfo[AREA]))).ToString("0,000") + " km²";
            int population = int.Parse(CheckNullObject(FlagTabController.instance.countryInfo[POPULATION]));
            string populationStr = string.Empty;
            if (population > 1000000000f)
            {
                populationStr = (population / 1000000000f).ToString("0.000") + " billion people";
            }
            else if (population > 1000000f)
            {
                populationStr = (population / 1000000f).ToString("0.000") + " million people";
            }
            else if (population < 1000000f)
            {
                populationStr = (population / 1000f).ToString("0.000") + " thousand people";
            }
            populationTxt.text = populationStr;
        }
    }
    public void OnClickCloseFlagMeanDialog()
    {
        var closeTutFlagKey = CPlayerPrefs.HasKey("CLOSE_TUTORIAL_FLAG");
        TweenControl.GetInstance().ScaleFromOne(DictionaryDialog.instance.flagMeanDialog.gameObject, 0.3f, () =>
        {
            DictionaryDialog.instance.OverLayDialog.SetActive(false);
            if (closeTutFlagKey)
            {
                CPlayerPrefs.DeleteKey("CLOSE_TUTORIAL_FLAG");
                var dialogsShow = FindObjectsOfType<Dialog>();
                foreach (var dialog in dialogsShow)
                {
                    dialog.Close();
                }
            }
        });
        Sound.instance.Play(Sound.Others.PopupClose);
    }
    private string CheckNullObject(object text)
    {
        string info;
        if (text != null)
        {
            info = text.ToString();
            if (info.IndexOf(',') != -1)
            {
                string[] infoArr = info.Split(new char[1] { ',' });
                info = null;
                for (int i = 0; i < infoArr.Length; i++)
                {
                    info += infoArr[i];
                }
            }
        }
        else
        {
            info = "0";
        }
        return info;
    }
}
