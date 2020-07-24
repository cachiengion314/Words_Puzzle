using TMPro;
public class UnLockTheFlagDialog : Dialog
{
    public static int indexOfFlagWhenClick;
    public TextMeshProUGUI priceTxt;
    public TextMeshProUGUI useHoneyToUnlockTxt; 
    private new void Start()
    {
        base.Start();

        priceTxt.text = FlagTabController.instance.priceToUnlockFlag.ToString();
        useHoneyToUnlockTxt.text = "Use " + FlagTabController.instance.priceToUnlockFlag.ToString() + " honey points to unlock the flag";
    }
    public void OnClickUnlockWithHoneyPoint()
    {
        if (FacebookController.instance.HoneyPoints >= FlagTabController.instance.priceToUnlockFlag)
        {
            DictionaryDialog.instance.flagList[indexOfFlagWhenClick].UnlockSuccess();
            OnClickCloseUnLockTheFlagDialog();
        }
        else
        {
            DictionaryDialog.instance.flagList[indexOfFlagWhenClick].UnlockFailed();
        }
    }
    public void OnClickCloseUnLockTheFlagDialog()
    {
        TweenControl.GetInstance().ScaleFromOne(DictionaryDialog.instance.unlockTheFlagDialog.gameObject, 0.3f);
    }
}
