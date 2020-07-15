using System;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.RemoteConfig;
using TMPro;
using UnityScript.Steps;

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
    }
    private void Update()
    {
        if (!isNeedToFetch) return;
        isNeedToFetch = false;

        FetchFireBase();

        Invoke("ShowIngameNotify", 1.7f);
        Invoke("GetAllIdAvertisement", 1.7f);
    }

    public void FetchFireBase()
    {
        FetchDataAsync();
    }
    private string ConvertFirebaseStringToNormal(string firebasestr)
    {
        string[] tempFirebaseStrArr = firebasestr.Split(new char[1] { '"' });
        firebasestr = null;
        foreach (string item in tempFirebaseStrArr)
        {
            firebasestr += item;
        }

        return firebasestr;
    }
    public void GetAllIdAvertisement()
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
        ConfigController.instance.config.facebookAdsId.rewardedLevelClear = fan_level_clear;
        ConfigController.instance.config.facebookAdsId.rewardedChapterClear = fan_chapter_clear;
        ConfigController.instance.config.facebookAdsId.rewardedBonusBox = fan_bonus_box;
        ConfigController.instance.config.facebookAdsId.rewardedFreeBoosters = fan_free_boosters;
        ConfigController.instance.config.facebookAdsId.rewardedFreeStars = fan_free_stars;
        ConfigController.instance.config.facebookAdsId.rewardedFreeLetter = fan_free_letter;
        ConfigController.instance.config.facebookAdsId.intersititial = fan_level_transition;
        // UnityAd
        ConfigController.instance.config.unityAdsId.rewardedLevel = unity_level_clear;
        ConfigController.instance.config.unityAdsId.rewardedChapter = unity_chapter_clear;
        ConfigController.instance.config.unityAdsId.rewardedBonusBox = unity_bonus_box;
        ConfigController.instance.config.unityAdsId.rewardedFreeBoosters = unity_free_boosters;
        ConfigController.instance.config.unityAdsId.rewardedFreeStars = unity_free_stars;
        ConfigController.instance.config.unityAdsId.rewardedFreeLetter = unity_free_letter;
        ConfigController.instance.config.unityAdsId.interstitialLevel = unity_level_transition;
        // Admob google
        ConfigController.instance.config.admob.rewardedLevel = admob_level_clear;
        ConfigController.instance.config.admob.rewardedChapter = admob_chapter_clear;
        ConfigController.instance.config.admob.rewardedBonusBox = admob_bonus_box;
        ConfigController.instance.config.admob.rewardedFreeBoosters = admob_free_boosters;
        ConfigController.instance.config.admob.rewardedFreeStars = admob_free_stars;
        ConfigController.instance.config.admob.rewardedFreeLetter = admob_free_letter;
        ConfigController.instance.config.admob.interstitialLevel = admob_level_transition;
        ConfigController.instance.config.admob.bannerLevel = admob_banner;
        // Min Level to load banner
        AudienceNetworkBanner.instance.MinLevelToLoadBanner = int.Parse(ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("active_banner_level").StringValue));
        // Min level to load rewarded video ads
        AdsManager.instance.MinLevelToLoadRewardVideo = int.Parse(ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("active_rewarded_level").StringValue));
        // Percent to load interstitial ads
        AdsManager.instance.PercentToloadInterstitial = int.Parse(ConvertFirebaseStringToNormal(FirebaseRemoteConfig.GetValue("interstitial_showing_ratio").StringValue));
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