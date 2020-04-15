using UnityEngine;
using System;
using DG.Tweening;
using System.Collections;

public class HomeController : BaseController
{
    public static HomeController instance;

    private const int PLAY = 0;
    private const int FACEBOOK = 1;
    public Animator animatorTitle;
    [SerializeField] private IconController _iconController;
    [SerializeField] private DOTweenAnimation _dotweenAnimation;

    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }

    protected override void Start()
    {
        base.Start();
        CUtils.CloseBannerAd();
        //if(!CPlayerPrefs.HasKey("TheNextDay"))
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

                CUtils.LoadScene(3, false);
                break;
            case FACEBOOK:
                CUtils.LikeFacebookPage(ConfigController.Config.facebookPageID);
                break;
        }
        Sound.instance.Play(Sound.Others.PopupOpen);
    }

    public void PlayAnimTitle()
    {
        Debug.Log("Play Animation Man Home");
        animatorTitle.enabled = true;
        animatorTitle.SetBool("Play", true);
        StartCoroutine(PlayAnimButton());
    }

    private IEnumerator PlayAnimButton()
    {
        yield return new WaitForSeconds(0.4f);
        _dotweenAnimation.DOPlayAllById("0");
        _iconController.AnimIcon();
    }

    public void StopAnimtitle()
    {
        animatorTitle.enabled = false;
        animatorTitle.SetBool("Play", false);
    }

    public override void OnApplicationPause(bool pause)
    {
        base.OnApplicationPause(pause);
        if (!pause)
            PlayAnimTitle();
    }
}
