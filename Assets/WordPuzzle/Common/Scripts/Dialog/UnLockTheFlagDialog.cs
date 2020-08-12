using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnLockTheFlagDialog : Dialog
{
    public static int indexOfFlagWhenClick;

    public TextMeshProUGUI priceTxt;
    public TextMeshProUGUI useHoneyToUnlockTxt;
    [Space]
    public TextMeshProUGUI unlockByPlayingTxt;
    public Button unlockByPlayingBtt;

    public GameObject amin;
    private new void Start()
    {
        base.Start();

        priceTxt.text = FlagTabController.instance.priceToUnlockFlag.ToString();
        useHoneyToUnlockTxt.text = "Use " + FlagTabController.instance.priceToUnlockFlag.ToString() + " honey points to unlock the flag";
    }
    public void OnClickPlayTheGame()
    {
        if (DictionaryDialog.instance.HomeControllerGetter != null)
            DictionaryDialog.instance.HomeControllerGetter.OnClick(0);
        OnClickCloseUnLockTheFlagDialog();
        var dialogsShow = FindObjectsOfType<Dialog>();
        foreach (var dialog in dialogsShow)
        {
            dialog.Close();
        }
        AudienceNetworkBanner.instance.LoadBanner();
    }
    public void OnClickUnlockWithHoneyPoint()
    {
        // Success buying flag
        if (FacebookController.instance.HoneyPoints >= FlagTabController.instance.priceToUnlockFlag)
        {
            FlagItemController flagItemWhenClick = DictionaryDialog.instance.flagList[indexOfFlagWhenClick];
            flagItemWhenClick.UnlockSuccess();
            if (flagItemWhenClick.flagUnlockWord != string.Empty)
            {
                FlagTabController.instance.AddToUnlockedWordDictionary(flagItemWhenClick.flagUnlockWord);
            }
            else
            {
                FlagTabController.instance.AddToUnlockedWordDictionary(flagItemWhenClick.flagName);
            }
            DictionaryDialog.instance.WriteFlagTabTitleContent();
            FlagTabController.instance.SaveUnlockedWordData();
            OnClickCloseUnLockTheFlagDialog();
        }
        // Buying flag failed
        else
        {
            DictionaryDialog.instance.flagList[indexOfFlagWhenClick].UnlockFailed();
        }
    }
    public void OnClickCloseUnLockTheFlagDialog()
    {
        TweenControl.GetInstance().ScaleFromOne(DictionaryDialog.instance.unlockTheFlagDialog.gameObject, 0.3f, () =>
        {
            DictionaryDialog.instance.OverLayDialog.SetActive(false);
        });
    }
    public void CheckUnlockByPlayingOnOff()
    {
        if (DictionaryDialog.instance.flagList[indexOfFlagWhenClick].flagUnlockWord == null
            || DictionaryDialog.instance.flagList[indexOfFlagWhenClick].flagUnlockWord == string.Empty)
        {
            unlockByPlayingBtt.gameObject.SetActive(false);
            unlockByPlayingTxt.gameObject.SetActive(false);
            useHoneyToUnlockTxt.gameObject.SetActive(true);
            //amin.SetActive(true);
        }
        else
        {
            unlockByPlayingTxt.text = "Find *" + DictionaryDialog.instance.flagList[indexOfFlagWhenClick].flagUnlockWord + "* to unlock this flag.";
            unlockByPlayingBtt.gameObject.SetActive(true);
            unlockByPlayingTxt.gameObject.SetActive(true);
            useHoneyToUnlockTxt.gameObject.SetActive(false);
            //amin.SetActive(false);
        }
    }
}
