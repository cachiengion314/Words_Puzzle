﻿using UnityEngine;
using System;
using GoogleMobileAds.Api;
using System;

public class AdmobController : MonoBehaviour, IAds
{
    public BannerView bannerView;
    public InterstitialAd interstitial;
    public RewardBasedVideoAd rewardBasedVideo;

    public static AdmobController instance;

    public float bannerHeight;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (!CUtils.IsAdsRemoved())
        {
            //RequestInterstitial();

            InitRewardedVideo();
            RequestRewardBasedVideo();

        }
    }
    private void InitRewardedVideo()
    {
        // Get singleton reward based video ad reference.
        this.rewardBasedVideo = RewardBasedVideoAd.Instance;

        // RewardBasedVideoAd is a singleton, so handlers should only be registered once.
        this.rewardBasedVideo.OnAdLoaded += this.HandleRewardBasedVideoLoaded;
        this.rewardBasedVideo.OnAdFailedToLoad += this.HandleRewardBasedVideoFailedToLoad;
        this.rewardBasedVideo.OnAdOpening += this.HandleRewardBasedVideoOpened;
        this.rewardBasedVideo.OnAdStarted += this.HandleRewardBasedVideoStarted;
        this.rewardBasedVideo.OnAdRewarded += this.HandleRewardBasedVideoRewarded;
        this.rewardBasedVideo.OnAdClosed += this.HandleRewardBasedVideoClosed;
        this.rewardBasedVideo.OnAdLeavingApplication += this.HandleRewardBasedVideoLeftApplication;
    }

    public void RequestBanner()
    {
        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = ConfigController.Config.admob.androidBanner.Trim();
#elif UNITY_IPHONE
        string adUnitId = ConfigController.Config.admob.iosBanner.Trim();
#else
        string adUnitId = "unexpected_platform";
#endif
        //Create a adaptive banner at the buttom of the screen.
#if UNITY_ANDROID && !UNITY_EDITOR
        int mobileScale = (int)MobileAds.Utils.GetDeviceScale();
        int width = Screen.width;
        int adWidth = width / mobileScale;
        int height = Screen.height;
        int adHeight = height / mobileScale;
        AdSize bannerAdSize = new AdSize(adWidth, 50);
        bannerAdSize = AdSize.GetPortraitAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        this.bannerView = new BannerView(adUnitId, bannerAdSize, AdPosition.Bottom);

        // Register for ad events.
        this.bannerView.OnAdLoaded += this.HandleAdLoaded;
        this.bannerView.OnAdFailedToLoad += this.HandleAdFailedToLoad;
        this.bannerView.OnAdOpening += this.HandleAdOpened;
        this.bannerView.OnAdClosed += this.HandleAdClosed;
        this.bannerView.OnAdLeavingApplication += this.HandleAdLeftApplication;

        // Load a banner ad.
        this.bannerView.LoadAd(this.CreateAdRequest());
#endif
    }

    public void RequestInterstitial()
    {
        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = ConfigController.Config.admob.androidInterstitial.Trim();
#elif UNITY_IPHONE
        string adUnitId = ConfigController.Config.admob.iosInterstitial.Trim();
#else
        string adUnitId = "unexpected_platform";
#endif

        // Create an interstitial.
        this.interstitial = new InterstitialAd(adUnitId);

        // Register for ad events.
        this.interstitial.OnAdLoaded += this.HandleInterstitialLoaded;
        this.interstitial.OnAdFailedToLoad += this.HandleInterstitialFailedToLoad;
        this.interstitial.OnAdOpening += this.HandleInterstitialOpened;
        this.interstitial.OnAdClosed += this.HandleInterstitialClosed;
        this.interstitial.OnAdLeavingApplication += this.HandleInterstitialLeftApplication;

        // Load an interstitial ad.
        this.interstitial.LoadAd(this.CreateAdRequest());
    }

    public void RequestRewardBasedVideo()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = ConfigController.Config.admob.androidRewarded.Trim();
#elif UNITY_IPHONE
        string adUnitId = ConfigController.Config.admob.iosRewarded.Trim();
#else
        string adUnitId = "unexpected_platform";
#endif

        this.rewardBasedVideo.LoadAd(this.CreateAdRequest(), adUnitId);
    }

    // Returns an ad request with custom ad targeting.
    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
                .AddTestDevice(AdRequest.TestDeviceSimulator)
                .AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
                .AddKeyword("game")
                .TagForChildDirectedTreatment(false)
                .AddExtra("color_bg", "9B30FF")
                .Build();
    }

    public void ShowInterstitial(InterstitialAd ad)
    {
        if (ad != null && ad.IsLoaded())
        {
            ad.Show();
        }
    }

    public void ShowBanner()
    {
        if (CUtils.IsAdsRemoved()) return;
        if (bannerView != null)
        {
            bannerView.Show();
        }
        else
        {
            RequestBanner();
        }
    }

    public void HideBanner()
    {
        if (bannerView != null)
        {
            bannerView.Hide();
        }
    }

    public bool ShowInterstitial(bool video = false)
    {
        if (interstitial != null && interstitial.IsLoaded())
        {
            interstitial.Show();
            return true;
        }
        return false;
    }

    public void ShowRewardBasedVideo(bool showToast = true, Action adsNotReadyYetCallback = null, Action noInternetCallback = null)
    {
        if (this.rewardBasedVideo.IsLoaded())
        {
            this.rewardBasedVideo.Show();
        }
        else
        {
            //MonoBehaviour.print("Reward based video ad is not ready yet");

            CUtils.CheckConnection(this, (result) =>
            {
                if (result == 0)
                {
                    if (showToast)
                        Toast.instance.ShowMessage("This feature can not be used right now. Please try again later!");
                    RequestRewardBasedVideo();
                    adsNotReadyYetCallback?.Invoke();
                }
                else
                {
                    if (showToast)
                        Toast.instance.ShowMessage("No Internet Connection");
                    noInternetCallback?.Invoke();
                }
            });
        }
    }
    #region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        // HandleAdLoaded event received

        MonoBehaviour.print(String.Format("Ad Height: {0}, width: {1}, ad HeightDp: {2}, ad WidthDp: {3}",
            this.bannerView.GetHeightInPixels(),
            this.bannerView.GetWidthInPixels(),
            Screen.height / (int)MobileAds.Utils.GetDeviceScale(),
            Screen.width / (int)MobileAds.Utils.GetDeviceScale()
            ));
        bannerHeight = this.bannerView.GetHeightInPixels();

        UIScaleController.instance.BannerShowAndScaleEvent();

    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleFailedToReceiveAd event received with message: " + args.Message);
        UIScaleController.instance.BannerHideAndScaleEvent();
    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        print("HandleAdOpened event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        print("HandleAdClosed event received");
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        print("HandleAdLeftApplication event received");
    }

    #endregion


    #region Interstitial callback handlers

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        print("HandleInterstitialLoaded event received.");
    }

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleInterstitialFailedToLoad event received with message: " + args.Message);
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        print("HandleInterstitialOpened event received");
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        print("HandleInterstitialClosed event received");
        RequestInterstitial();
    }

    public void HandleInterstitialLeftApplication(object sender, EventArgs args)
    {
        print("HandleInterstitialLeftApplication event received");
    }

    #endregion

    #region RewardBasedVideo callback handlers

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //MonoBehaviour.print(
        //    "HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        RequestRewardBasedVideo();
        //MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
        AdsManager.instance.onAdsClose?.Invoke();
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        //MonoBehaviour.print(
        //    "HandleRewardBasedVideoRewarded event received for " + amount.ToString() + " " + type);
    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
    }
    #endregion

    /// <summary>
    /// Implement Interface
    /// </summary>
    public void LoadVideoAds()
    {

    }

    public void ShowVideoAds(Action adsNotReadyYetCallback = null, Action noInternetCallback = null)
    {
        if (this.rewardBasedVideo.IsLoaded())
        {
            this.rewardBasedVideo.Show();
        }
        else
        {
            //MonoBehaviour.print("Reward based video ad is not ready yet");
            CUtils.CheckConnection(this, (result) =>
            {
                if (result == 0)
                {
                    RequestRewardBasedVideo();
                    adsNotReadyYetCallback?.Invoke();
                }
                else
                {
                    noInternetCallback?.Invoke();
                    //Debug.Log("No Internet Connection");
                }
            });
        }
    }

    public void ShowBannerAds()
    {

    }

    public void ShowInterstitialAds()
    {
        CUtils.ShowInterstitialAd();
    }
}
