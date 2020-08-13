using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CUtils;

public class FlagItemController : MonoBehaviour
{
    public int indexOfFlag;
    [Space]
    public int indexOfSmallFlagImage;
    public int indexOfBigFlagImage;

    public string flagName;
    public string subRegion;
    public string capital;
    public string population;
    public string area;

    public string flagUnlockWord;
   
    public bool isLocked;
    public Sprite iconFlagLock;

    [HideInInspector] public Sprite flagImage;
    [SerializeField] private Image flagImg;
    [SerializeField] private GameObject LockBgImg;
    [SerializeField] private Text nameTxt;
    [SerializeField] private Image lockImg;
    //[Space]
    //public Canvas canvas;

    private void Start()
    {
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
        //canvas.overrideSorting = false;

        if (TutorialController.instance != null)
        {
            Destroy(gameObject.GetComponent<GraphicRaycaster>());
            Destroy(gameObject.GetComponent<Canvas>());
            if (TutorialController.instance.isShowTut)
                CPlayerPrefs.SetBool("CLOSE_TUTORIAL_FLAG", true);
            TutorialController.instance.HidenPopTut();
        }
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

            FlagMeanDialog.indexOfFlagWhenClick = indexOfFlag;
            CheckConnection(this, (result) =>
            {
                if (result != 0) // no internet conection
                {
                    FlagTabController.instance.haveInternet = false;
                    DictionaryDialog.instance.flagMeanDialog.flagMeanItems.gameObject.SetActive(false);
                    DictionaryDialog.instance.flagMeanDialog.noInternet.gameObject.SetActive(true);
                }
                else
                {
                    FlagTabController.instance.haveInternet = true;
                    DictionaryDialog.instance.flagMeanDialog.flagMeanItems.gameObject.SetActive(true);
                    DictionaryDialog.instance.flagMeanDialog.noInternet.gameObject.SetActive(false);
                }
            });

            //StartCoroutine(FlagTabController.instance.GetCoutryInfoOffline(flagName));
            StartCoroutine(DictionaryDialog.instance.flagMeanDialog.OnOpenFlagMeanDialog());
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
            flagImg.sprite = FlagTabController.instance.smallFlags[indexOfSmallFlagImage];
            //LockBgImg.gameObject.SetActive(false);
            nameTxt.gameObject.SetActive(false);
            lockImg.gameObject.SetActive(false);
        }
        else
        {
            flagImg.sprite = iconFlagLock;
            //LockBgImg.gameObject.SetActive(true);
            nameTxt.gameObject.SetActive(true);
            lockImg.gameObject.SetActive(true);
        }
    }
    public void OnClickCloseUnLockTheFlagDialog()
    {
        TweenControl.GetInstance().ScaleFromOne(DictionaryDialog.instance.unlockTheFlagDialog.gameObject, 0.3f, () =>
        {
            DictionaryDialog.instance.OverLayDialog.SetActive(false);
        });
    }
}
