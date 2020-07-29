using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlagMeanDialog : Dialog
{
    public TextMeshProUGUI countryNameTxt;
    public TextMeshProUGUI subRegionTxt;
    public TextMeshProUGUI capitalTxt;
    public TextMeshProUGUI areaTxt;
    public TextMeshProUGUI populationTxt;
    public Image flagImg;
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
        countryNameTxt.text = "Loading... ";
        subRegionTxt.text = "Loading... ";
        capitalTxt.text = "Loading... ";
        areaTxt.text = "Loading... ";
        populationTxt.text = "Loading... ";

        yield return new WaitUntil(() => FlagTabController.instance.isGetCountryRequestDone);

        flagImg.sprite = flagSprite != null ? flagSprite : defaultImage;
        countryNameTxt.text = FlagTabController.instance.countryInfo[COUNTRY_NAME].ToString();
        subRegionTxt.text = FlagTabController.instance.countryInfo[SUB_REGION].ToString();
        capitalTxt.text = FlagTabController.instance.countryInfo[CAPITAL].ToString();
        areaTxt.text = (float.Parse(FlagTabController.instance.countryInfo[AREA].ToString())).ToString("0,000") + " km²";
        int population = int.Parse(FlagTabController.instance.countryInfo[POPULATION].ToString());
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
    public void OnClickCloseFlagMeanDialog()
    {
        TweenControl.GetInstance().ScaleFromOne(DictionaryDialog.instance.flagMeanDialog.gameObject, 0.3f, () => {
            DictionaryDialog.instance.OverLayDialog.SetActive(false);
        });
        Sound.instance.Play(Sound.Others.PopupClose);
    }
}
