using UnityEngine;
using System;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class HomeController : BaseController
{
    public static HomeController instance;

    private const int PLAY = 0;
    private const int FACEBOOK = 1;
    private const string TIME_REWARD_KEY = "TIME_REWARD";
    public Button btnChickenBank;
    public Button btnFreeBoosters;
    public GameObject FreeBoostersShadow;
    public GameObject notiChickenMax;
    public GameObject notiOpenFreeBoosters;
    public Animator animatorTitle;
    [SerializeField] private IconController _iconController;
    [SerializeField] private RectTransform _panelTopRect;
    [Space]
    [SerializeField] private GameObject _btnPlayShadow;
    [SerializeField] private SpineControl _spineAnimGia;
    [SerializeField] private string _showgiado = "animation2";
    [Header("THEME UI")]
    public Image BG;
    public Image imageStar;
    public Image imageAdd;
    public Image bgCurrency;
    public Image shadowCurrency;
    public TextMeshProUGUI txtStar;
    public SpineControl animChickenbank;
    public SpineControl animFreebooster;


    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }

    protected override void Start()
    {
        base.Start();
        var currTheme = CPlayerPrefs.GetInt("CURR_THEMES", 0);
        ThemesControl.instance.LoadThemeDataHome(currTheme);
        ThemesControl.instance.LoadThemeDataDialog(currTheme);
        //CUtils.CloseBannerAd();
        var sceneAnimate = SceneAnimate.Instance;
        sceneAnimate._btnPlay.interactable = true;
        sceneAnimate._spineAnimEgg.gameObject.SetActive(true);
        sceneAnimate._spineAnimShadow.gameObject.SetActive(true);
        sceneAnimate._spineAnimEgg.SetAnimation(sceneAnimate.idleEgg, false);
        sceneAnimate._spineAnimShadow.SetAnimation(sceneAnimate.idleEggShadow, false);
        ShowChickenBank();
        PlayAnimTitle();
        //var firstLoad = CPlayerPrefs.GetBool("First_Load", false);
        //if (!firstLoad)
        //{
        //    CPlayerPrefs.SetBool("First_Load", true);
        //    SceneAnimate.Instance.LoadSceneWithProgressLoading();
        //}

        if (!CPlayerPrefs.HasKey("FREEBOOSTERS_TUTORIAL"))
        {
            btnFreeBoosters.gameObject.SetActive(false);
            FreeBoostersShadow.SetActive(false);
        }
        ShowIconNoti();
    }

    public void OnClick(int index)
    {
        switch (index)
        {
            case PLAY:
                GameState.currentWorld = Prefs.unlockedWorld;
                GameState.currentSubWorld = Prefs.unlockedSubWorld;
                GameState.currentLevel = Prefs.unlockedLevel;
                //GameState.countSpell = Prefs.countSpell;
                //Debug.Log(Prefs.unlockedWorld.ToString() + Prefs.unlockedWorld.ToString() + Prefs.unlockedLevel.ToString());

                CUtils.LoadScene(Const.SCENE_MAIN, false);
                break;
            case FACEBOOK:
                CUtils.LikeFacebookPage(ConfigController.Config.facebookPageID);
                break;
        }
        Sound.instance.Play(Sound.Others.PopupOpen);
    }

    public void PlayAnimTitle()
    {
        //SceneAnimate.Instance.btnTest.SetActive(true);
        //animatorTitle.enabled = true;
        //animatorTitle.SetBool("Play", true);
        SceneAnimate.Instance._btnPlay.gameObject.SetActive(false);
        _btnPlayShadow.transform.localScale = Vector3.zero;
        StartCoroutine(PlayAnimButton());
    }

    private IEnumerator PlayAnimButton()
    {
        var tweenControl = TweenControl.GetInstance();
        var sceneAnimate = SceneAnimate.Instance;
        yield return new WaitForSeconds(1f);
        SceneAnimate.Instance.ShowTip(false);
        sceneAnimate._spineAnimEgg.gameObject.SetActive(true);
        sceneAnimate._spineAnimShadow.gameObject.SetActive(true);
        _spineAnimGia.gameObject.SetActive(true);
        sceneAnimate._spineAnimShadow.SetAnimation(sceneAnimate.showShadow, false, () =>
        {
            sceneAnimate._spineAnimShadow.SetAnimation(sceneAnimate.showShadowLoop, true);
        });
        sceneAnimate._spineAnimEgg.SetAnimation(sceneAnimate.showAnim, false, () =>
        {
            //Debug.Log("Play Animation Man Home");
            sceneAnimate._spineAnimEgg.SetAnimation(sceneAnimate.loopAnim, true);
        });
        sceneAnimate._loadingScreen.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        //Debug.Log("Play Animation Man Home Done");
        sceneAnimate._btnPlay.gameObject.SetActive(true);
        sceneAnimate._maskShadow.gameObject.SetActive(true);
        tweenControl.Scale(_btnPlayShadow, Vector3.one * 1.2f, 0.47f, () =>
        {
            tweenControl.Scale(_btnPlayShadow, Vector3.one, 0.53f);
        });
        tweenControl.MoveRectY(_panelTopRect, -30, 0.7f, () =>
        {
            tweenControl.MoveRectY(_panelTopRect, 20, 0.3f, () =>
            {
                tweenControl.MoveRectY(_panelTopRect, 0, 0.5f, () =>
                {
                    if ((!LoginBonusController.instance.isShowLoginbonus && !RemoteConfigFirebase.instance.isShowNoti) || LoginBonusController.instance.HidenSpin)
                        CheckShowFreeBooster();
                });
            });
        });
        _spineAnimGia.SetAnimation(_showgiado, false, () =>
        {
            _iconController.AnimIcon();
        });
    }

    public void CheckShowFreeBooster()
    {
        if (CPlayerPrefs.HasKey("HINT_TUTORIAL") && CPlayerPrefs.HasKey("SELECTED_HINT_TUTORIAL") && CPlayerPrefs.HasKey("MULTIPLE_HINT_TUTORIAL") && !CPlayerPrefs.HasKey("FREEBOOSTERS_TUTORIAL"))
        {
            btnFreeBoosters.gameObject.SetActive(true);
            FreeBoostersShadow.SetActive(false);
            TutorialController.instance.ShowPopFreeBoostersTut();
            CPlayerPrefs.SetBool("FREEBOOSTERS_TUTORIAL", true);
        }
    }

    public void StopAnimtitle()
    {
        animatorTitle.enabled = false;
        animatorTitle.SetBool("Play", false);
    }

    public void ShowChickenBank()
    {
        if (!CPlayerPrefs.HasKey("OPEN_CHICKEN"))
        {
            var valueShow = (ConfigController.instance.config.gameParameters.minBank * 10 / 100) + ConfigController.instance.config.gameParameters.minBank;
            var currStarBank = ChickenBankController.instance.CurrStarChicken;
            if (currStarBank < valueShow)
                btnChickenBank.gameObject.SetActive(false);
            else
            {
                btnChickenBank.gameObject.SetActive(true);
                CPlayerPrefs.SetBool("OPEN_CHICKEN", true);
            }
        }
    }

    public void ShowIconNoti()
    {
        if (ChickenBankController.instance.CurrStarChicken >= FacebookController.instance.user.maxbank)
            notiChickenMax.SetActive(true);
        else
            notiChickenMax.SetActive(false);

        if (CPlayerPrefs.HasKey(TIME_REWARD_KEY))
        {
            var _isReward = CPlayerPrefs.GetBool(TIME_REWARD_KEY, false);
            if (!_isReward)
                notiOpenFreeBoosters.SetActive(false);
            else
                notiOpenFreeBoosters.SetActive(true);
        }
        else
        {
            notiOpenFreeBoosters.SetActive(true);
        }
    }

    public void OnClickBeehiveButton()
    {
        Sound.instance.Play(Sound.Others.PopupOpen);
        if (CPlayerPrefs.HasKey("BEE_TUTORIAL") || BeeManager.instance.CurrBee > 0)
        {
            DialogController.instance.ShowDialog(DialogType.Bee, DialogShow.REPLACE_CURRENT);
        }
        else
        {
            DialogController.instance.ShowDialog(DialogType.ComingSoon, DialogShow.REPLACE_CURRENT, "Beehive", "Complete Level 40 to unlock this feature!");
        }
    }
}
