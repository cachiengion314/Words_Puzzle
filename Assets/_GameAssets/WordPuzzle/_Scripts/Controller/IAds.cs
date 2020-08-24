using System;

public interface IAds
{
    void ShowVideoAds(Action adsNotReadyYetCallback = null, Action noInternetCallback = null);

    void ShowBannerAds();

    void ShowInterstitialAds();

}
