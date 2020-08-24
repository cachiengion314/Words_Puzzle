using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDialog_imgAddBtt : MonoBehaviour
{
   public void HideOrDestroyAllBanner()
    {
        AudienceNetworkBanner.instance.DisposeAllBannerAd();
    }

}
