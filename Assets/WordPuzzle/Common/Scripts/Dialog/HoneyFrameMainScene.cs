using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyFrameMainScene : MonoBehaviour
{
    [HideInInspector] public static bool isClickOnThis;
    public void HoneyFrameMainSceneClick()
    {
        isClickOnThis = true;
        AudienceNetworkBanner.instance.DisposeAllBannerAd();
    }
}
