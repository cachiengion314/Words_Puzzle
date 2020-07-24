using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CUtils;

public class FlagItemController : MonoBehaviour
{
    public int indexOfFlag;
    public string flagName;
    public bool isLocked;

    [HideInInspector] public Sprite flagImage;

    [SerializeField] private Image flagImg;
    [SerializeField] private GameObject LockBgImg;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private Image lockImg;


    private void Start()
    {
        flagImg.sprite = flagImage;
        nameTxt.text = flagName;
        if (isLocked)
        {
            FlagItemOnOff(false);
        }
        else
        {
            FlagItemOnOff(true);
        }
    }
    public void OnClickToTheFlag()
    {
        if (isLocked)
        {
            LogController.Debug("Try to unlock the flag");

            UnLockTheFlagDialog.indexOfFlagWhenClick = indexOfFlag;

            TweenControl.GetInstance().ScaleFromZero(DictionaryDialog.instance.unlockTheFlagDialog.gameObject, 0.3f);
            Sound.instance.Play(Sound.Others.PopupOpen);
        }
        else
        {
            LogController.Debug("Open flag mean dialog");
            UnLockTheFlagDialog.indexOfFlagWhenClick = indexOfFlag;

            StartCoroutine(FlagTabController.instance.GetCountryInfo(flagName));           
            StartCoroutine(DictionaryDialog.instance.flagMeanDialog.OnOpenFlagMeanDialog(flagImage));
        }
    }
    public void UnlockFailed()
    {
        Toast.instance.ShowMessage("You don't have enought honey points to unlock this");
    }
    public void UnlockSuccess()
    {
        isLocked = false;
        FlagItemOnOff(true);
        HoneyPointsController.ShowAndFade(string.Empty, -FlagTabController.instance.priceToUnlockFlag, 0, DictionaryDialog.instance.visualHoneyTxt);
        FacebookController.instance.HoneyPoints -= FlagTabController.instance.priceToUnlockFlag;
        DictionaryDialog.instance.honeyTxt.text = AbbrevationUtility.AbbreviateNumber(FacebookController.instance.HoneyPoints);
        FacebookController.instance.SaveDataGame();
    }
    private void FlagItemOnOff(bool isOn)
    {
        if (isOn)
        {
            LockBgImg.gameObject.SetActive(false);
            nameTxt.gameObject.SetActive(false);
            lockImg.gameObject.SetActive(false);
        }
        else
        {
            LockBgImg.gameObject.SetActive(true);
            nameTxt.gameObject.SetActive(true);
            lockImg.gameObject.SetActive(true);
        }
    }
    public void OnClickCloseUnLockTheFlagDialog()
    {
        TweenControl.GetInstance().ScaleFromOne(DictionaryDialog.instance.unlockTheFlagDialog.gameObject, 0.3f);
    }
}
