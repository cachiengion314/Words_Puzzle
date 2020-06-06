using UnityEngine;
using System;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;

public class HomeController : BaseController
{
    public static HomeController instance;

    private const int PLAY = 0;
    private const int FACEBOOK = 1;
    public Button btnChickenBank;
    public Animator animatorTitle;
    [SerializeField] private IconController _iconController;
    [SerializeField] private RectTransform _panelTopRect;
    [Space]
    [SerializeField] private Button _btnPlay;
    [SerializeField] private GameObject _btnPlayShadow;
    [SerializeField] private GameObject _maskShadow;
    [SerializeField] private SpineControl _spineAnimEgg;
    [SerializeField] private SpineControl _spineAnimShadow;
    [SerializeField] private SpineControl _spineAnimGia;
    [SerializeField] private string _showAnim = "animation";
    [SerializeField] private string _loopAnim = "Loop";
    [SerializeField] private string _showShadow = "Shadow";
    [SerializeField] private string _showShadowLoop = "Shadow Loop";
    [SerializeField] private string _showgiado = "animation2";

    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }

    protected override void Start()
    {
        base.Start();
        CUtils.CloseBannerAd();
        ShowChickenBank();
        var sceneAnimate = SceneAnimate.Instance;
        sceneAnimate._spineAnimEgg.gameObject.SetActive(true);
        sceneAnimate._spineAnimShadow.gameObject.SetActive(true);
        sceneAnimate._spineAnimEgg.SetAnimation(sceneAnimate._idleEgg, false);
        sceneAnimate._spineAnimShadow.SetAnimation(sceneAnimate._idleEggShadow, false);
        PlayAnimTitle();
        //var firstLoad = CPlayerPrefs.GetBool("First_Load", false);
        //if (!firstLoad)
        //{
        //    CPlayerPrefs.SetBool("First_Load", true);
        //    SceneAnimate.Instance.LoadSceneWithProgressLoading();
        //}
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
        SceneAnimate.Instance.btnTest.SetActive(true);
        Debug.Log("Play Animation Man Home");
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
        sceneAnimate._spineAnimEgg.gameObject.SetActive(true);
        sceneAnimate._spineAnimShadow.gameObject.SetActive(true);
        _spineAnimGia.gameObject.SetActive(true);
        sceneAnimate._spineAnimShadow.SetAnimation(_showShadow, false, () =>
        {
            sceneAnimate._spineAnimShadow.SetAnimation(_showShadowLoop, true);
        });
        sceneAnimate._spineAnimEgg.SetAnimation(_showAnim, false, () =>
        {
            sceneAnimate._spineAnimEgg.SetAnimation(_loopAnim, true);
        });
        sceneAnimate._loadingScreen.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.4f);
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
                tweenControl.MoveRectY(_panelTopRect, 0, 0.5f);
            });
        });
        _spineAnimGia.SetAnimation(_showgiado, false, () =>
        {
            _iconController.AnimIcon();
        });
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
}
