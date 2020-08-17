using System;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.RemoteConfig;
using TMPro;
using UnityScript.Steps;
using System.Collections;

public class RemoteConfigFirebase : MonoBehaviour
{
    //public TextMeshProUGUI textPrefab;
    public static RemoteConfigFirebase instance;
    public Action notifyIngameCall;

    private string tittle;
    private string contain;
    private bool notifyIngameOn;
    [HideInInspector] public bool isShowNoti;

    private bool isNeedToFetch = true;
    // Audience NetWork Id ads
    public string fan_level_clear;
    public string fan_chapter_clear;
    public string fan_bonus_box;
    public string fan_free_boosters;
    public string fan_free_stars;
    public string fan_free_letter;
    public string fan_level_transition;
    // Unity Id ads
    public string unity_level_clear;
    public string unity_chapter_clear;
    public string unity_bonus_box;
    public string unity_free_boosters;
    public string unity_free_stars;
    public string unity_free_letter;
    public string unity_level_transition;
    // Google id ads
    public string admob_level_clear;
    public string admob_chapter_clear;
    public string admob_bonus_box;
    public string admob_free_boosters;
    public string admob_free_stars;
    public string admob_free_letter;
    public string admob_level_transition;
    public string admob_banner;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);

        notifyIngameCall += () =>
        {
            if (!notifyIngameOn)
            {
                if (HomeController.instance != null)
                    HomeController.instance.CheckShowFreeBooster();
                return;
            }

            bool isNeedToNotify = NotifyMailDialogData.instance.Tittle != tittle;

            if (!NotifyMailDialogData.instance.IsShowBefore && !CPlayerPrefs.GetBool("FIRST")
           || isNeedToNotify && !CPlayerPrefs.GetBool("FIRST"))
            {
                MailDialog.CreateNewNotify(tittle, contain);
                DialogController.instance.ShowDialog(DialogType.Mail, DialogShow.STACK_DONT_HIDEN);
                isShowNoti = true;
            }
            else
            {
                if (HomeController.instance != null)
                    HomeController.instance.CheckShowFreeBooster();
            }
        };
        StartCoroutine(GetAllIdAvertisementWhenFetchingDone());
    }
    private void Start()
    {
        FetchFireBase();

        //Invoke("ShowIngameNotify", 1.7f);
        //Invoke("GetAllIdAvertisement", 1.7f);

        //StartCoroutine(AdsManager.instance.LoadAndConfigAdsId());
    }

    public void FetchFireBase()
    {
        CUtils.CheckConnection(this, (result) =>
        {
            if (result == 0)
            {
                FetchDataAsync();
            }
        });
    }
    private IEnumerator GetAllIdAvertisementWhenFetchingDone()
    {
        yield return new WaitUntil(() => IsFetchingDone);
        GetAllIdAvertisement();
    }
    private string ConvertFirebaseStringToNormal(string firebasestr)
    {
        return firebasestr;
    }
    public void GetAllIdAvertisement()
    {
        CUtils.CheckConnection(this, (result) =>
        {
            if (result == 0)
            {
                // Facebook Audience Network
                fan_level_clear = ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("fan_level_clear").StringValue);
                fan_chapter_clear = ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("fan_chapter_clear").StringValue);
                fan_bonus_box = ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("fan_bonus_box").StringValue);
                fan_free_boosters = ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("fan_free_boosters").StringValue);
                fan_free_stars = ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("fan_free_stars").StringValue);
                fan_free_letter = ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("fan_free_letter").StringValue);
                fan_level_transition = ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("fan_level_transition").StringValue);
                // UnityAd
                unity_level_clear = ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("unity_level_clear").StringValue);
                unity_chapter_clear = ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("unity_chapter_clear").StringValue);
                unity_bonus_box = ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("unity_bonus_box").StringValue);
                unity_free_boosters = ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("unity_free_boosters").StringValue);
                unity_free_stars = ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("unity_free_stars").StringValue);
                unity_free_letter = ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("unity_free_letter").StringValue);
                unity_level_transition = ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("unity_level_transition").StringValue);
                // Admob google
                admob_level_clear = ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("admob_level_clear").StringValue);
                admob_chapter_clear = ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("admob_chapter_clear").StringValue);
                admob_bonus_box = ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("admob_bonus_box").StringValue);
                admob_free_boosters = ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("admob_free_boosters").StringValue);
                admob_free_stars = ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("admob_free_stars").StringValue);
                admob_free_letter = ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("admob_free_letter").StringValue);
                admob_level_transition = ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("admob_level_transition").StringValue);
                admob_banner = ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("admob_banner").StringValue);
                // Implement id advertisement
                // Facebook Audience Network
                ConfigController.instance.config.facebookAdsId.rewardedLevelClear = CheckNull(fan_level_clear, AudienceNetworkFbAd.instance.rewardIdFaceAds);
                ConfigController.instance.config.facebookAdsId.rewardedChapterClear = CheckNull(fan_chapter_clear, AudienceNetworkFbAd.instance.rewardIdFaceAds);
                ConfigController.instance.config.facebookAdsId.rewardedBonusBox = CheckNull(fan_bonus_box, AudienceNetworkFbAd.instance.rewardIdFaceAds);
                ConfigController.instance.config.facebookAdsId.rewardedFreeBoosters = CheckNull(fan_free_boosters, AudienceNetworkFbAd.instance.rewardIdFaceAds);
                ConfigController.instance.config.facebookAdsId.rewardedFreeStars = CheckNull(fan_free_stars, AudienceNetworkFbAd.instance.rewardIdFaceAds);
                ConfigController.instance.config.facebookAdsId.rewardedFreeLetter = CheckNull(fan_free_letter, AudienceNetworkFbAd.instance.rewardIdFaceAds);
                ConfigController.instance.config.facebookAdsId.intersititial = CheckNull(fan_level_transition, AudienceNetworkFbAd.instance.intersititialIdFaceAds);
                // UnityAd
                ConfigController.instance.config.unityAdsId.rewardedLevel = CheckNull(unity_level_clear, UnityAdTest.instance.myPlacementId);
                ConfigController.instance.config.unityAdsId.rewardedChapter = CheckNull(unity_chapter_clear, UnityAdTest.instance.myPlacementId);
                ConfigController.instance.config.unityAdsId.rewardedBonusBox = CheckNull(unity_bonus_box, UnityAdTest.instance.myPlacementId);
                ConfigController.instance.config.unityAdsId.rewardedFreeBoosters = CheckNull(unity_free_boosters, UnityAdTest.instance.myPlacementId);
                ConfigController.instance.config.unityAdsId.rewardedFreeStars = CheckNull(unity_free_stars, UnityAdTest.instance.myPlacementId);
                ConfigController.instance.config.unityAdsId.rewardedFreeLetter = CheckNull(unity_free_letter, UnityAdTest.instance.myPlacementId);
                ConfigController.instance.config.unityAdsId.interstitialLevel = CheckNull(unity_level_transition, UnityAdTest.instance.myInterstitialId);
                // Admob google
                ConfigController.instance.config.admob.rewardedLevel = CheckNull(admob_level_clear, AdmobController.instance.videoAdsId);
                ConfigController.instance.config.admob.rewardedChapter = CheckNull(admob_chapter_clear, AdmobController.instance.videoAdsId);
                ConfigController.instance.config.admob.rewardedBonusBox = CheckNull(admob_bonus_box, AdmobController.instance.videoAdsId);
                ConfigController.instance.config.admob.rewardedFreeBoosters = CheckNull(admob_free_boosters, AdmobController.instance.videoAdsId);
                ConfigController.instance.config.admob.rewardedFreeStars = CheckNull(admob_free_stars, AdmobController.instance.videoAdsId);
                ConfigController.instance.config.admob.rewardedFreeLetter = CheckNull(admob_free_letter, AdmobController.instance.videoAdsId);
                ConfigController.instance.config.admob.interstitialLevel = CheckNull(admob_level_transition, AdmobController.instance.interstitialAdsId);
                ConfigController.instance.config.admob.bannerLevel = CheckNull(admob_banner, AdmobController.instance.bannerAdsId);
                // Min Level to load banner
                AdsManager.instance.MinLevelToLoadBanner = CheckIntParse(ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("active_banner_level").StringValue));
                LogController.Debug("MinLevelToLoadBanner: " + AdsManager.instance.MinLevelToLoadBanner);
                // Min level to load rewarded video ads
                AdsManager.instance.MinLevelToLoadRewardVideo = CheckIntParse(ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("active_rewarded_level").StringValue));
                LogController.Debug("MinLevelToLoadBanner: " + AdsManager.instance.MinLevelToLoadRewardVideo);
                // Percent to load interstitial ads
                AdsManager.instance.PercentToloadInterstitial = CheckIntParse(ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("interstitial_showing_ratio").StringValue));
                LogController.Debug("MinLevelToLoadBanner: " + AdsManager.instance.PercentToloadInterstitial);
                // Min level to load interstitial ads
                AdsManager.instance.MinLevelToLoadInterstitial = CheckIntParse(ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("active_interstitial_level").StringValue));
                LogController.Debug("MinLevelToLoadBanner: " + AdsManager.instance.MinLevelToLoadInterstitial);

                //AdsManager.instance.LoadAndConfigAdsId();
            }
            else
            {
                // no internet conection
                AdsManager.instance.MinLevelToLoadBanner = 50;
                AdsManager.instance.MinLevelToLoadRewardVideo = 50;
                AdsManager.instance.PercentToloadInterstitial = 50;
                AdsManager.instance.MinLevelToLoadInterstitial = 50;

                //AdsManager.instance.LoadAndConfigAdsId();
            }
        });

    }
    public int CheckIntParse(string stringValue)
    {
        int intValue = 50;
        if (stringValue != null || stringValue != string.Empty)
        {
            if (int.TryParse(stringValue, out intValue))
            {
                Debug.Log("intValue: " + intValue);
            }
        }
        return intValue;
    }
    public string CheckNull(string idAds, string idAds2)
    {
        if (idAds == null)
        {
            return idAds2;
        }
        else
        {
            return idAds;
        }
    }
    public void ShowIngameNotify()
    {
        if (!notifyIngameOn) return;

        tittle = ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("TittleMail").StringValue);
        contain = ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("ContainMail").StringValue);

        bool isNeedToNotify = NotifyMailDialogData.instance.Tittle != tittle;

        if (!NotifyMailDialogData.instance.IsShowBefore && CPlayerPrefs.GetBool("FIRST")
            || isNeedToNotify && CPlayerPrefs.GetBool("FIRST"))
        {
            MailDialog.CreateNewNotify(tittle, contain);
            DialogController.instance.ShowDialog(DialogType.Mail, DialogShow.STACK_DONT_HIDEN);
        }
    }

    // Start a fetch request.
    public Task FetchDataAsync()
    {
        //Debug.Log("Fetching data...");
        // FetchAsync only fetches new data if the current data is older than the provided
        // timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
        // By default the timespan is 12 hours, and for production apps, this is a good
        // number.  For this example though, it's set to a timespan of zero, so that
        // changes in the console will always show up immediately.

        Task fetchTask = FirebaseRemoteConfig.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWith(FetchComplete);
    }

    public bool IsFetchingDone { get; private set; }
    void FetchComplete(Task fetchTask)
    {
        if (fetchTask.IsCanceled)
        {
            //Debug.Log("Fetch canceled.");
        }
        else if (fetchTask.IsFaulted)
        {
            //Debug.Log("Fetch encountered an error.");
        }
        else if (fetchTask.IsCompleted)
        {
            //Debug.Log("Fetch completed successfully!");
            // don't put callback in here. It doesn work properly.

        }

        var info = FirebaseRemoteConfig.Info;
        switch (info.LastFetchStatus)
        {
            case LastFetchStatus.Success:
                FirebaseRemoteConfig.ActivateFetched();
                //Debug.Log(String.Format("Remote data loaded and ready (last fetch time {0}).",
                //    info.FetchTime));
                IsFetchingDone = true;

                Debug.Log("isFetchingDone: " + IsFetchingDone);
                break;
            case LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case FetchFailureReason.Error:
                        //Debug.Log("Fetch failed for unknown reason");
                        break;
                    case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                        //Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }
                break;
            case LastFetchStatus.Pending:
                //Debug.Log("Latest Fetch call still pending.");
                break;
        }
    }
}