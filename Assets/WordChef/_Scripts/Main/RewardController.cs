using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardController : MonoBehaviour
{
    [SerializeField] private RewardedButton _rewardedButton;
    void Start()
    {
        _rewardedButton.onRewarded += OnCompleteRewardVideo;
    }

    private void OnCompleteRewardVideo()
    {
        _rewardedButton.gameObject.SetActive(false);
        foreach (var line in WordRegion.instance.Lines)
        {
            var cellAds = line.cells.FindAll(cell => cell.giftAds.activeInHierarchy);
            foreach (var cell in cellAds)
            {
                cell.OpenGiftAds();
            }
        }
    }
}
