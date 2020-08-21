using UnityEngine;
using UnityEngine.UI;
using AudienceNetwork;
using UnityEngine.SceneManagement;
using AudienceNetwork.Utility;
using System.Collections;
using System;

public class AudienceNetworkFbAd : MonoBehaviour, IAds
{
  [HideInInspector]  public string rewardIdFaceAds;
  [HideInInspector]  public string intersititialIdFaceAds;

    public static AudienceNetworkFbAd instance;
    public RewardedVideoAd rewardedVideoAd;
    public bool isLoaded;
#pragma warning disable 0414
    public bool didClose;
#pragma warning restore 0414

    public InterstitialAd interstitialAd;
    public bool isIntersLoaded;
#pragma warning disable 0414
    public bool didIntersClose;
#pragma warning restore 0414
    private void Awake()
    {
        instance = this;
#if UNITY_ANDROID && !UNITY_EDITOR
        AudienceNetworkAds.Initialize();
#endif
     
    }
    public void LoadInterstitial()
    {
        //statusLabel.text = "Loading interstitial ad...";

        // Create the interstitial unit with a placement ID (generate your own on the Facebook app settings).
        // Use different ID for each ad placement in your app.
        interstitialAd = new InterstitialAd(intersititialIdFaceAds);

        interstitialAd.Register(gameObject);

        // Set delegates to get notified on changes or when the user interacts with the ad.
        interstitialAd.InterstitialAdDidLoad = delegate ()
        {
            isIntersLoaded = true;
            didIntersClose = false;
            string isAdValid = interstitialAd.IsValid() ? "valid" : "invalid";
            Debug.Log("Interstitial ad loaded." + isAdValid);
            //statusLabel.text = "Ad loaded and is " + isAdValid + ". Click show to present!";
        };
        interstitialAd.InterstitialAdDidFailWithError = delegate (string error)
        {
            Debug.Log("Interstitial ad failed to load with error: " + error);
            //statusLabel.text = "Interstitial ad failed to load. Check console for details.";
            AdsManager.instance.onAdsClose?.Invoke();
            SceneAnimate.Instance.ShowOverLayPauseGame(false);
        };
        interstitialAd.InterstitialAdWillLogImpression = delegate ()
        {
            Debug.Log("Interstitial ad logged impression.");
        };
        interstitialAd.InterstitialAdDidClick = delegate ()
        {
            Debug.Log("Interstitial ad clicked.");
        };
        interstitialAd.InterstitialAdDidClose = delegate ()
        {
            Debug.Log("Interstitial ad did close.");
            //AdmobController.instance.RequestInterstitial();
            AdsManager.instance.IsLoading = true;
            AdsManager.instance.onAdsClose?.Invoke();
            SceneAnimate.Instance.ShowOverLayPauseGame(false);
            didIntersClose = true;
            if (interstitialAd != null)
            {
                interstitialAd.Dispose();
            }
            LoadInterstitial();
        };

#if UNITY_ANDROID
        /*
         * Only relevant to Android.
         * This callback will only be triggered if the Interstitial activity has
         * been destroyed without being properly closed. This can happen if an
         * app with launchMode:singleTask (such as a Unity game) goes to
         * background and is then relaunched by tapping the icon.
         */
        interstitialAd.interstitialAdActivityDestroyed = delegate ()
        {
            if (!didIntersClose)
            {
                Debug.Log("Interstitial activity destroyed without being closed first.");
                Debug.Log("Game should resume.");
            }
        };
#endif
        // Initiate the request to load the ad.
        interstitialAd.LoadAd();
    }
    //public void ShowRewardedVideo()
    //{
    //    if (rewardIdFaceAds == null) return;

    //    if (isLoaded)
    //    {
    //        rewardedVideoAd.Show();
    //        isLoaded = false;
    //        statusLabel.text = "";
    //    }
    //    else
    //    {
    //        statusLabel.text = "Ad not loaded. Click load to request an ad.";
    //    }
    //}
    void OnDestroy()
    {
        // Dispose of rewardedVideo ad when the scene is destroyed
        if (rewardedVideoAd != null)
        {
            rewardedVideoAd.Dispose();
        }
        //Debug.Log("RewardedVideoAdTest was destroyed!");

        // Dispose of interstitial ad when the scene is destroyed
        if (interstitialAd != null)
        {
            interstitialAd.Dispose();
        }
        Debug.Log("Interstitial audience ad AdTest was destroyed!");
    }
    private void ShowInterstitial()
    {
        if (isIntersLoaded)
        {
            interstitialAd.Show();
            isIntersLoaded = false;
            //statusLabel.text = "";
        }
        else
        {
            //statusLabel.text = "Ad not loaded. Click load to request an ad.";
        }
    }
    /// <summary>
    /// Implement Interface
    /// </summary>
    public void ShowVideoAds(Action adsNotReadyYetCallback = null, Action noInternetCallback = null)
    {
        rewardedVideoAd.Show();
        isLoaded = false;
    }
    public void ShowBannerAds()
    {

    }
    public void ShowInterstitialAds()
    {
        if (intersititialIdFaceAds == null) { isIntersLoaded = false; return; }

        ShowInterstitial();
    }
    public void LoadVideoAds()
    {
        //Debug.Log("Loading rewardedVideo ad...");

        // Create the rewarded video unit with a placement ID (generate your own on the Facebook app settings).
        // Use different ID for each ad placement in your app.
        rewardedVideoAd = new RewardedVideoAd(rewardIdFaceAds);

        // For S2S validation you can create the rewarded video ad with the reward data
        // Refer to documentation here:
        // https://developers.facebook.com/docs/audience-network/android/rewarded-video#server-side-reward-validation
        // https://developers.facebook.com/docs/audience-network/ios/rewarded-video#server-side-reward-validation
        RewardData rewardData = new RewardData
        {
            UserId = "USER_ID",
            Currency = "REWARD_ID"
        };
#pragma warning disable 0219
        RewardedVideoAd s2sRewardedVideoAd = new RewardedVideoAd(rewardIdFaceAds, rewardData);
#pragma warning restore 0219

        rewardedVideoAd.Register(gameObject);

        // Set delegates to get notified on changes or when the user interacts with the ad.
        rewardedVideoAd.RewardedVideoAdDidLoad = delegate ()
        {
            //Debug.Log("RewardedVideo ad loaded.");
            isLoaded = true;
            didClose = false;
            string isAdValid = rewardedVideoAd.IsValid() ? "valid" : "invalid";
            //Debug.Log("Ad loaded and is " + isAdValid + ". Click show to present!");

            Debug.Log("FacebookAds video isLoad: " + isLoaded);
        };
        rewardedVideoAd.RewardedVideoAdDidFailWithError = delegate (string error)
        {
            Debug.Log("RewardedVideo ad failed to load with error: " + error);
            //Debug.Log("RewardedVideo ad failed to load. Check console for details.");
            //LoadVideoAds();
            AdsManager.instance.onAdsClose?.Invoke();
            SceneAnimate.Instance.ShowOverLayPauseGame(false);
        };
        rewardedVideoAd.RewardedVideoAdWillLogImpression = delegate ()
        {
            //Debug.Log("RewardedVideo ad logged impression.");
        };
        rewardedVideoAd.RewardedVideoAdDidClick = delegate ()
        {
            //Debug.Log("RewardedVideo ad clicked.");
        };

        // For S2S validation you need to register the following two callback
        // Refer to documentation here:
        // https://developers.facebook.com/docs/audience-network/android/rewarded-video#server-side-reward-validation
        // https://developers.facebook.com/docs/audience-network/ios/rewarded-video#server-side-reward-validation
        rewardedVideoAd.RewardedVideoAdDidSucceed = delegate ()
        {
            Debug.Log("Facebook Ads - Rewarded video ad validated by server");
        };

        rewardedVideoAd.RewardedVideoAdDidFail = delegate ()
        {
            Debug.Log("Rewarded video ad not validated - FaceBook Ads Failed!");
            AdsManager.instance.onAdsClose?.Invoke();
            SceneAnimate.Instance.ShowOverLayPauseGame(false);
        };

        rewardedVideoAd.RewardedVideoAdDidClose = delegate ()
        {
            Debug.Log("Rewarded video ad did close.");
            //AdmobController.instance.RequestRewardBasedVideo();
            AdsManager.instance.IsLoading = true;
            AdsManager.instance.onAdsRewarded?.Invoke();
            SceneAnimate.Instance.ShowOverLayPauseGame(false);
            didClose = true;
            if (rewardedVideoAd != null)
            {
                rewardedVideoAd.Dispose();
            }
            LoadVideoAds();
        };

#if UNITY_ANDROID
        /*
         * Only relevant to Android.
         * This callback will only be triggered if the Rewarded Video activity
         * has been destroyed without being properly closed. This can happen if
         * an app with launchMode:singleTask (such as a Unity game) goes to
         * background and is then relaunched by tapping the icon.
         */
        rewardedVideoAd.RewardedVideoAdActivityDestroyed = delegate ()
        {
            if (!didClose)
            {
                //Debug.Log("Rewarded video activity destroyed without being closed first.");
                //Debug.Log("Game should resume. User should not get a reward.");
            }
        };
#endif

        // Initiate the request to load the ad.
        rewardedVideoAd.LoadAd();
    }
}
