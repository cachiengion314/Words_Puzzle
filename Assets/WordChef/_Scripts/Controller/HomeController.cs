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
    [SerializeField] private SpineControl _spineAnimEgg;
    [SerializeField] private string _showAnim = "animation";
    [SerializeField] private string _loopAnim = "Loop";

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
                GameState.countSpell = Prefs.countSpell;
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
        _btnPlay.gameObject.SetActive(false);
        _btnPlayShadow.transform.localScale = Vector3.zero;
        StartCoroutine(PlayAnimButton());
    }

    private IEnumerator PlayAnimButton()
    {
        var tweenControl = TweenControl.GetInstance();
        yield return new WaitForSeconds(1f);
        _spineAnimEgg.gameObject.SetActive(true);
        _spineAnimEgg.SetAnimation(_showAnim, false, () => {
            _spineAnimEgg.SetAnimation(_loopAnim, true);
        });
        yield return new WaitForSeconds(0.4f);
        _btnPlay.gameObject.SetActive(true);
        tweenControl.Scale(_btnPlayShadow, Vector3.one * 1.2f, 0.47f, () => {
            tweenControl.Scale(_btnPlayShadow, Vector3.one, 0.53f);
        });
        tweenControl.MoveRectY(_panelTopRect, -30, 0.7f, () => {
            tweenControl.MoveRectY(_panelTopRect, 0, 0.3f);
        });
        _iconController.AnimIcon();
    }

    public void StopAnimtitle()
    {
        animatorTitle.enabled = false;
        animatorTitle.SetBool("Play", false);
    }

    public void ShowChickenBank()
    {
        var valueShow = (ConfigController.instance.config.gameParameters.minBank * 10 / 100) + ConfigController.instance.config.gameParameters.minBank;
        var currStarBank = ChickenBankController.instance.CurrStarChicken;
        if (currStarBank < valueShow)
            btnChickenBank.gameObject.SetActive(false);
        else
            btnChickenBank.gameObject.SetActive(true);
    }
}
