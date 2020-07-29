using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlagMeanDialog : Dialog
{
    public static int indexOfFlagWhenClick;

    public TextMeshProUGUI countryNameTxt;
    public TextMeshProUGUI subRegionTxt;
    public TextMeshProUGUI capitalTxt;
    public TextMeshProUGUI areaTxt;
    public TextMeshProUGUI populationTxt;
    [Space]
    public Sprite[] bigFlags;
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
    public IEnumerator OnOpenFlagMeanDialog(Sprite flagSprite)
    {
        DictionaryDialog.instance.OverLayDialog.SetActive(true);
        TweenControl.GetInstance().ScaleFromZero(DictionaryDialog.instance.flagMeanDialog.gameObject, 0.3f);
        Sound.instance.Play(Sound.Others.PopupOpen);
        flagImg.sprite = defaultImage;
        titleNameTxt.text = "Loading...";
        countryNameTxt.text = "Loading... ";
        subRegionTxt.text = "Loading... ";
        capitalTxt.text = "Loading... ";
        areaTxt.text = "Loading... ";
        populationTxt.text = "Loading... ";

        yield return new WaitUntil(() => FlagTabController.instance.isGetCountryRequestDone);

        if (FlagTabController.instance.countryInfo.Count > 0)
        {
            for (int i = 0; i < bigFlags.Length; i++)
            {
                if (bigFlags[i].name == DictionaryDialog.instance.flagList[indexOfFlagWhenClick].flagName)
                {
                    flagImg.sprite = bigFlags[i];
                }
            }
            flagImg.sprite = flagSprite != null ? flagSprite : defaultImage;
            titleNameTxt.text = CheckNullObject(FlagTabController.instance.countryInfo[COUNTRY_NAME]);
            countryNameTxt.text = CheckNullObject(FlagTabController.instance.countryInfo[COUNTRY_NAME]);
            subRegionTxt.text = CheckNullObject(FlagTabController.instance.countryInfo[SUB_REGION]);
            capitalTxt.text = CheckNullObject(FlagTabController.instance.countryInfo[CAPITAL]);
            areaTxt.text = (float.Parse(CheckNullObject(FlagTabController.instance.countryInfo[AREA]))).ToString("0,000") + " km²";
            int population = int.Parse(CheckNullObject(FlagTabController.instance.countryInfo[POPULATION]));
            string populationStr = string.Empty;
            if (population > 1000000)
            {
                populationStr = (population / 1000000f).ToString("0.000") + " million people";
            }
            else
            {
                populationStr = (population).ToString("0,000") + " thousand people";
            }
            populationTxt.text = populationStr;
        }
    }
    public void OnClickCloseFlagMeanDialog()
    {
        TweenControl.GetInstance().ScaleFromOne(DictionaryDialog.instance.flagMeanDialog.gameObject, 0.3f, () =>
        {
            DictionaryDialog.instance.OverLayDialog.SetActive(false);
        });
        Sound.instance.Play(Sound.Others.PopupClose);
    }
    private string CheckNullObject(object text)
    {
        string info = "0";
        if (text != null) info = text.ToString();

        return info;
    }
}
