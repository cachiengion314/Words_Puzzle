using UnityEngine;

public class HomeController : BaseController
{
    private const int PLAY = 0;
    private const int FACEBOOK = 1;

    protected override void Start()
    {
        base.Start();
        CUtils.CloseBannerAd();
    }

    public void OnClick(int index)
    {
        switch (index)
        {
            case PLAY:
                GameState.currentWorld = Prefs.unlockedWorld;
                GameState.currentSubWorld = Prefs.unlockedSubWorld;
                GameState.currentLevel = Prefs.unlockedLevel;

                CUtils.LoadScene(3, false);
                break;
            case FACEBOOK:
                CUtils.LikeFacebookPage(ConfigController.Config.facebookPageID);
                break;
        }
        Sound.instance.PlayButton();
    }

}
