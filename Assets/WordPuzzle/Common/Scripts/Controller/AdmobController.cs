using UnityEngine;
using System;
using GoogleMobileAds.Api;

public class AdmobController : MonoBehaviour, IAds
{
    public BannerView bannerView;
    public InterstitialAd interstitial;
    public RewardBasedVideoAd rewardBasedVideo;

    public static AdmobController instance;
    public float bannerHeight;

    [HideInInspector] public string videoAdsId = "ca-app-pub-3940256099942544/5224354917";
    [HideInInspector] public string interstitialAdsId = "ca-app-pub-3940256099942544/1033173712";
    [HideInInspector] public string bannerAdsId = "ca-app-pub-3212738706492790/6113697308";

    private void Awake()
    {
        instance = this;
        MobileAds.Initialize(initStatus =>
        {

        });
    }
    public void InitRewardedVideo()
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
        string adUnitId = bannerAdsId;
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
        string adUnitId = interstitialAdsId;
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
        string adUnitId = videoAdsId;
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
        if (CUtils.IsAdsRemoved() || bannerAdsId == null) return;

        if (bannerView != null)
        {
            bannerView.Show();
            UIScaleController.instance.BannerShowAndScaleEvent();
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
    #region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        // HandleAdLoaded event received
        Debug.Log(transform.name + "_HandleAdLoaded");
        MonoBehaviour.print(String.Format("Banner Height: {0}, Banner width: {1}, " +
            "Device HeightDp: {2}, Device WidthDp: {3}, " +
            "Device Heigh: {4}, Device Width: {5}, " +
            "SafeAreana Height: {6}, SafeArena Width: {7}, " +
            "WordRegion.instance.RectCanvas.rect.height: {8}, WordRegion.instance.RectCanvas.rect.width: {9}",

            this.bannerView.GetHeightInPixels(),
            this.bannerView.GetWidthInPixels(),
            Screen.height / (int)MobileAds.Utils.GetDeviceScale(),
            Screen.width / (int)MobileAds.Utils.GetDeviceScale(),
            Screen.height,
            Screen.width,
            Screen.safeArea.height,
            Screen.safeArea.width,
            WordRegion.instance.RectCanvas.rect.height,
            WordRegion.instance.RectCanvas.rect.width
            ));

        bannerHeight = this.bannerView.GetHeightInPixels();
        UIScaleController.instance.BannerShowAndScaleEvent();
    }
    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleFailedToReceiveAd event received with message: " + args.Message);
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
        AdsManager.instance.onAdsClose?.Invoke();
        SceneAnimate.Instance.ShowOverLayPauseGame(false);
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        print("HandleInterstitialOpened event received");
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        print("HandleInterstitialClosed event received");
        AdsManager.instance.IsLoading = true;
        AdsManager.instance.onAdsClose?.Invoke();
        SceneAnimate.Instance.ShowOverLayPauseGame(false);
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
        MonoBehaviour.print(
            "HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);

        AdsManager.instance.onAdsClose?.Invoke();
        SceneAnimate.Instance.ShowOverLayPauseGame(false);
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
        MonoBehaviour.print("Admob Video close !");
        AdsManager.instance.IsLoading = true;
        // HandleRewardBasedVideoClosed event received;
        AdsManager.instance.onAdsClose?.Invoke();
        SceneAnimate.Instance.ShowOverLayPauseGame(false);
        RequestRewardBasedVideo();
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        //MonoBehaviour.print(
        //    "HandleRewardBasedVideoRewarded event received for " + amount.ToString() + " " + type);
        AdsManager.instance.onAdsRewarded?.Invoke();
        SceneAnimate.Instance.ShowOverLayPauseGame(false);
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
        if (videoAdsId == null) return;


        if (this.rewardBasedVideo.IsLoaded())
        {
            this.rewardBasedVideo.Show();
        }
        else
        {
            // Reward based video ad is not ready yet
            CUtils.CheckConnection(this, (result) =>
            {
                if (result == 0)
                {
                    adsNotReadyYetCallback?.Invoke();
                }
                else
                {
                    // no internet
                    noInternetCallback?.Invoke();                   
                }
            });
        }
    }

    public void ShowBannerAds()
    {

    }

    public void ShowInterstitialAds()
    {
        if (interstitialAdsId == null || CUtils.IsAdsRemoved()) return;

        ShowInterstitial();
        //CUtils.ShowInterstitialAd();
    }
}
