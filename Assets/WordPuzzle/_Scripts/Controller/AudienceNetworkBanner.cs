using UnityEngine;
using AudienceNetwork;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AudienceNetwork.Utility;
using System.Collections;
using Superpow;

public class AudienceNetworkBanner : MonoBehaviour
{
    public static AudienceNetworkBanner instance;

    public int currlevel;
    public GameData gameData;
    private AdView adView;
    private AdPosition currentAdViewPosition;
    private ScreenOrientation currentScreenOrientation;
    public Text statusLabel;

    void OnDestroy()
    {
        // Dispose of banner ad when the scene is destroyed
        DisposeAllBannerAd();
    }

    private void Awake()
    {
        instance = this;

        SceneManager.activeSceneChanged += ChangedActiveSceneToLoadBanner;
    }
    int nextSceneName;

    private void ChangedActiveSceneToLoadBanner(Scene current, Scene next)
    {
        nextSceneName = next.buildIndex;

        if (nextSceneName == 3)
        {
            if (CUtils.IsAdsRemoved()) return;

            LoadBanner();
        }
        else if (nextSceneName != 3)
        {
            DisposeAllBannerAd();
        }
    }
    public void DisposeAllBannerAd()
    {
        if (adView)
        {
            adView.Dispose();
        }
        //Debug.Log("AdViewTest was destroyed!");

        AdmobController.instance.HideBanner();
    }
    public void LoadBanner()
    {
        if (AdsManager.instance.MinLevelToLoadBanner == 0) return;
        int currentLevel = CheckCurrentLevel();

        if (currentLevel >= AdsManager.instance.MinLevelToLoadBanner && MainController.instance != null)
        {
            CUtils.CheckConnection(this, (result) =>
            {
                if (result == 0)
                {
                    StartCoroutine(LoadBannerWithDelay());
                }
            });
        }
    }
    public void LoadAudienceNetworkBanner()
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


            // "Banner failed to load with error: " + error;
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

    private IEnumerator LoadBannerWithDelay()
    {
        yield return new WaitForSeconds(0.5f);

        AdmobController.instance.ShowBanner();
    }
    public int CheckCurrentLevel()
    {
        var world = Prefs.unlockedWorld;
        var subWorld = Prefs.unlockedSubWorld;
        var level = Prefs.unlockedLevel;
        var numlevels = Utils.GetNumLevels(world, subWorld);
        // int chapter = Prefs.unlockedSubWorld + Prefs.unlockedWorld * gameData.words[0].subWords.Count;
        currlevel = (level + numlevels * subWorld + world * gameData.words[0].subWords.Count * numlevels) + 1;

        return currlevel;
    }
}
