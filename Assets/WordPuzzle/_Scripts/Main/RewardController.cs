using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardController : MonoBehaviour
{
    public void OnShowAdsVideo()
    {
        Sound.instance.Play(Sound.Others.PopupOpen);
        DialogController.instance.ShowDialog(DialogType.FreeStarsPlay, DialogShow.REPLACE_CURRENT);
    }
}

