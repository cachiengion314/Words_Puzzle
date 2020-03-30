using UnityEngine;
using System;

public class HomeController : BaseController
{
    private const int PLAY = 0;
    private const int FACEBOOK = 1;

    private void Awake()
    {
        if (!CPlayerPrefs.HasKey("INSTALLED"))
        {
            CPlayerPrefs.DeleteAll();
            CPlayerPrefs.Save();
            CPlayerPrefs.SetBool("INSTALLED", true);
        }
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

}
