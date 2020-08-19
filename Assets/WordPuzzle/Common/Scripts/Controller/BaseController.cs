using UnityEngine;
using System.Collections;

public class BaseController : MonoBehaviour
{
    public GameObject donotDestroyOnLoad;
    public string sceneName;
    public Music.Type music = Music.Type.None;
    protected int numofEnterScene;

    protected virtual void Awake()
    {
        Application.targetFrameRate = 60;
        if (!CPlayerPrefs.HasKey("INSTALLED"))
        {
            CPlayerPrefs.SetBool("INSTALLED", true);
        }
        if (DonotDestroyOnLoad.instance == null && donotDestroyOnLoad != null)
            Instantiate(donotDestroyOnLoad);
        iTween.dimensionMode = CommonConst.ITWEEN_MODE;
        CPlayerPrefs.useRijndael(CommonConst.ENCRYPTION_PREFS);

        //numofEnterScene = CUtils.IncreaseNumofEnterScene(sceneName);
    }

    protected virtual void Start()
    {
        CPlayerPrefs.Save();
        //if (JobWorker.instance.onEnterScene != null)
        //{
        //    JobWorker.instance.onEnterScene(sceneName);
        //}

#if UNITY_WSA && !UNITY_EDITOR
        StartCoroutine(SavePrefs());
#endif
        if (!Music.instance.audioSource.isPlaying)
            Music.instance.Play(music);
        if (TutorialController.instance != null && SceneAnimate.Instance.isShowTest)
        {
            TutorialController.instance.helpLevel = 13;
            TutorialController.instance.selectedHintLevel = 14;
            TutorialController.instance.multipleHintLevel = 15;
            TutorialController.instance.beehiveLevel = 16;
            TutorialController.instance.chickenBankLevel = 17;
        }
        //CUtils.ShowBannerAd();
    }

    public virtual void OnApplicationPause(bool pause)
    {
        Debug.Log("On Application Pause: " + pause);
        CPlayerPrefs.Save();
        //if (pause == false)
        //{
        //    Timer.Schedule(this, 0.5f, () =>
        //    {
        //        CUtils.ShowInterstitialAd();
        //    });
        //}
    }

    private IEnumerator SavePrefs()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            CPlayerPrefs.Save();
        }
    }
}
