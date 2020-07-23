using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CUtils;

public class FlagItemController : MonoBehaviour
{
    [HideInInspector] public Sprite flagImage;
    [HideInInspector] public string flagName;
    [HideInInspector] public bool isLocked;

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
        if (isLocked && FacebookController.instance.HoneyPoints < FlagTabController.instance.priceToUnlockFlag)
        {
            LogController.Debug("Try to unlock the flag and failed");
            Toast.instance.ShowMessage("You don't have enought honey points to unlock this");
        }
        else if (isLocked && FacebookController.instance.HoneyPoints >= FlagTabController.instance.priceToUnlockFlag)
        {
            LogController.Debug("Try to unlock the flag and success");
            isLocked = false;
            FlagItemOnOff(true);

            HoneyPointsController.ShowAndFade(string.Empty, -FlagTabController.instance.priceToUnlockFlag, 0, DictionaryDialog.instance.visualHoneyTxt);
            FacebookController.instance.HoneyPoints -= FlagTabController.instance.priceToUnlockFlag;
            DictionaryDialog.instance.honeyTxt.text = AbbrevationUtility.AbbreviateNumber(FacebookController.instance.HoneyPoints);
            FacebookController.instance.SaveDataGame();
        }
        else if (!isLocked)
        {
            LogController.Debug("Try to open another dialog");

            TweenControl.GetInstance().ScaleFromZero(DictionaryDialog.instance.flagMeanDialog.gameObject, 0.3f);
            Sound.instance.Play(Sound.Others.PopupOpen);
        }
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
}
