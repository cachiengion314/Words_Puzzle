using UnityEngine;
using AudienceNetwork;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AudienceNetwork.Utility;
using System.Collections;

public class AudienceNetworkBanner : MonoBehaviour
{
    public static AudienceNetworkBanner instance;

    private AdView adView;
    private AdPosition currentAdViewPosition;
    private ScreenOrientation currentScreenOrientation;
    public Text statusLabel;
    void OnDestroy()
    {
        // Dispose of banner ad when the scene is destroyed
        DisposeAllBannerAd();
    }
    bool hasLoadMainScene;
    private void Awake()
    {
        //AudienceNetworkAds.Initialize();
        instance = this;

        SceneManager.activeSceneChanged += ChangedActiveScene;
    }
    int nextSceneName;
    private IEnumerator ReLoadFacebookBanner()
    {
        while (nextSceneName == 3)
        {           
            yield return new WaitForSeconds(5);
            DisposeAllBannerAd();
            LoadBanner();
        }
        DisposeAllBannerAd();
    }
    private void ChangedActiveScene(Scene current, Scene next)
    {
        nextSceneName = next.buildIndex;

        if (nextSceneName == 3)
        {
            if (CUtils.IsAdsRemoved()) return;

            if (!hasLoadMainScene)
            {
#if UNITY_ANDROID && !UNITY_EDITOR
            StartCoroutine(ReLoadFacebookBanner());
#endif
            }
            hasLoadMainScene = true;
        }
        else if (nextSceneName != 3)
        {
            hasLoadMainScene = false;
        }
    }
    public void DisposeAllBannerAd()
    {
        if (adView)
        {
            adView.Dispose();
        }
        //Debug.Log("AdViewTest was destroyed!");

        if (AdmobController.instance.bannerView != null)
        {
            AdmobController.instance.bannerView.Destroy();
        }
    }

    // Load Banner button
    public void LoadBanner()
    {
        if (adView)
        {
            adView.Dispose();
        }

        //statusLabel.text = "Loading Banner...";

        // Create a banner's ad view with a unique placement ID (generate your own on the Facebook app settings).
        // Use different ID for each ad placement in your app.
        adView = new AdView("583616318955925_583618328955724", AdSize.BANNER_HEIGHT_50);
        adView.Register(gameObject);

        currentAdViewPosition = AdPosition.BOTTOM;

        // Set delegates to get notified on changes or when the user interacts with the ad.
        adView.AdViewDidLoad = delegate ()
        {
            currentScreenOrientation = Screen.orientation;
            adView.Show(AdPosition.BOTTOM);
            string isAdValid = adView.IsValid() ? "valid" : "invalid";
            //statusLabel.text = "Banner loaded and is " + isAdValid + ".";
        };
        adView.AdViewDidFailWithError = delegate (string error)
        {
            // admob controller show
            ShowAdmobBanner();
            //statusLabel.text = "Banner failed to load with error: " + error;
        };
        adView.AdViewWillLogImpression = delegate ()
        {
            //statusLabel.text = "Banner logged impression.";
            //SetAdViewPosition(AdPosition.BOTTOM);
        };
        adView.AdViewDidClick = delegate ()
        {
            //statusLabel.text = "Banner clicked.";
        };

        // Initiate a request to load an ad.
        adView.LoadAd();
    }

    // Change button
    // Change the position of the ad view when button is clicked
    // ad view is at top: move it to bottom
    // ad view is at bottom: move it to 100 pixels along y-axis
    // ad view is at custom position: move it to the top
    public void ChangePosition()
    {
        switch (currentAdViewPosition)
        {
            case AdPosition.TOP:
                SetAdViewPosition(AdPosition.BOTTOM);
                break;
            case AdPosition.BOTTOM:
                SetAdViewPosition(AdPosition.CUSTOM);
                break;
            case AdPosition.CUSTOM:
                SetAdViewPosition(AdPosition.TOP);
                break;
        }
    }

    private void OnRectTransformDimensionsChange()
    {
        if (adView && Screen.orientation != currentScreenOrientation)
        {
            SetAdViewPosition(currentAdViewPosition);
            currentScreenOrientation = Screen.orientation;
        }
    }

    private void SetAdViewPosition(AdPosition adPosition)
    {
        switch (adPosition)
        {
            case AdPosition.TOP:
                adView.Show(AdPosition.TOP);
                currentAdViewPosition = AdPosition.TOP;
                break;
            case AdPosition.BOTTOM:
                adView.Show(AdPosition.BOTTOM);
                currentAdViewPosition = AdPosition.BOTTOM;
                break;
            case AdPosition.CUSTOM:
                adView.Show(100);
                currentAdViewPosition = AdPosition.CUSTOM;
                break;
        }
    }

    public void ShowAdmobBanner()
    {
        if (CUtils.IsAdsRemoved()) return;
        if (AdmobController.instance.bannerView != null)
        {
            AdmobController.instance.bannerView.Show();
        }
        else
        {
            AdmobController.instance.RequestAdaptiveBanner();
        }
    }
}
