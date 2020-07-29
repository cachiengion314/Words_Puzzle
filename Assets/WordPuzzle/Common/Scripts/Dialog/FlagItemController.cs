using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CUtils;

public class FlagItemController : MonoBehaviour
{
    public int indexOfFlag;
    public string flagName;
    public string flagUnlockWord;
    public bool isLocked;
    public Sprite defaultFlagImage;

    [HideInInspector] public Sprite flagImage;
    [SerializeField] private Image flagImg;
    [SerializeField] private GameObject LockBgImg;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private Image lockImg;


    private void Start()
    {
        if(flagImage != null)
        {
            flagImg.sprite = flagImage;
        }
        else
        {
            flagImg.sprite = defaultFlagImage;
        }      
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
            LogController.Debug("Open unlock flag dialog");

            UnLockTheFlagDialog.indexOfFlagWhenClick = indexOfFlag;
            DictionaryDialog.instance.unlockTheFlagDialog.CheckUnlockByPlayingOnOff();
            DictionaryDialog.instance.OverLayDialog.SetActive(true);
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
        TweenControl.GetInstance().ScaleFromOne(DictionaryDialog.instance.unlockTheFlagDialog.gameObject, 0.3f,()=> {
            DictionaryDialog.instance.OverLayDialog.SetActive(false);
        });
    }
}
