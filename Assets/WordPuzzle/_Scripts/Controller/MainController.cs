using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Superpow;
using TMPro;
using System;
using PlayFab;
using System.Linq;
using PlayFab.Internal;

public class MainController : BaseController
{
    public Action onLoadDataComplete;

    public GameData gameData;
    public TextMeshProUGUI levelNameText;
    public Animator animatorScene;
    public BeeController beeController;
    [Space]
    public Canvas canvasPopup;
    public Canvas canvasFx;
    public Canvas canvasCollect;

    private int world, subWorld, level;
    private bool _isGameComplete;
    private bool _isLevelClear;
    [SerializeField] private GameLevel gameLevel;
    public GameLevel GameLevel
    {
        get { return gameLevel; }
    }
    public static MainController instance;

    [HideInInspector] public bool isBeePlay;

    private string wordLevelSave;
    private string _wordPassed;
    public string WordPassed
    {
        get
        {
            return _wordPassed;
        }
    }
    public bool IsLevelClear
    {
        get
        {
            return _isLevelClear;
        }
        set
        {
            _isLevelClear = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        instance = this;
        isBeePlay = false;
        onLoadDataComplete = PlayAnimScene;
    }

    protected override void Start()
    {
        base.Start();
        //CUtils.CloseBannerAd();
        world = GameState.currentWorld;
        subWorld = GameState.currentSubWorld;
        level = GameState.currentLevel;
        var numlevels = Utils.GetNumLevels(world, subWorld);
        var currlevel = (level + numlevels * subWorld + world * gameData.words[0].subWords.Count * numlevels);
        //world = 4;
        //subWorld = 4;
        //level = 4;
        //Debug.Log(world + ", " + subWorld + ", " + level);
        //save level pass;
        canvasPopup.gameObject.SetActive(true);
        var currTheme = CPlayerPrefs.GetInt("CURR_THEMES", 0);
        ThemesControl.instance.LoadThemeData(currTheme);
        ThemesControl.instance.LoadThemeDataDialog(currTheme);

        gameLevel = Utils.Load(world, subWorld, level);
        //WordRegion.instance.CalculateScaleSizeBoardRegionAndPan();
        Pan.instance.Load(gameLevel);
        WordRegion.instance.Load(gameLevel, currlevel);
        BeeManager.instance.Load(CPlayerPrefs.GetInt("amount_bee", 0));

        //GameState.currentSubWorldName

        levelNameText.text = "LEVEL " + (currlevel + 1);

        var isFirstTheme = CPlayerPrefs.GetBool("THEME_DIALOG", false);
        if (!isFirstTheme)
        {
            CPlayerPrefs.SetBool("THEME_DIALOG", true);
            Sound.instance.Play(Sound.Others.PopupOpen);
            DialogController.instance.ShowDialog(DialogType.Themes, DialogShow.REPLACE_CURRENT);
            AudienceNetworkBanner.instance.DisposeAllBannerAd();
        }
        else
            onLoadDataComplete?.Invoke();
        ThemeCallEventFirebase(ThemesControl.instance.CurrTheme.nameTheme);
    }

    private void ThemeCallEventFirebase(string nameTheme)
    {
        // Log an event with multiple parameters, passed as an array:
        var parameters = new Firebase.Analytics.Parameter[]
        {
            new Firebase.Analytics.Parameter("theme_name", nameTheme),
        };

        Firebase.Analytics.FirebaseAnalytics.LogEvent("theme_used", parameters);
    }

    public void OnComplete()
    {
        if (_isGameComplete) return;
        _isGameComplete = true;
        canvasCollect.gameObject.SetActive(true);
        //CPlayerPrefs.DeleteKey("HINT_LINE_INDEX");
        //Save Passed Word
        //SaveWordComplete(gameLevel.answers);

        //if (PlayFabClientAPI.IsClientLoggedIn())
        //{
        //FacebookController.instance.user.wordPassed = _wordPassed;
        //FacebookController.instance.SaveDataGame();
        //}
        // 
        Timer.Schedule(this, 0.2f, () =>
        {
            DialogController.instance.ShowDialog(DialogType.Win);

            HoneyPointsController.instance.ShowHoneyPoints();
        });
    }
    public string wordDone;
    public void SaveWordComplete(string wordDone)
    {
        this.wordDone = wordDone;
        if (!CPlayerPrefs.HasKey("WordLevelSave"))
        {
            CPlayerPrefs.SetString("WordLevelSave", wordDone);
            _wordPassed = wordDone;
        }
        else
        {
            wordLevelSave = CPlayerPrefs.GetString("WordLevelSave");
            wordLevelSave += "|" + wordDone;
            CPlayerPrefs.SetString("WordLevelSave", wordLevelSave);
            _wordPassed = WordSaveDistinct();
            if (_wordPassed.Length > 0 && _wordPassed[_wordPassed.Length - 1].ToString() == "|")
                _wordPassed.Remove(_wordPassed.Length - 1);
        }
        FacebookController.instance.user.wordPassed = _wordPassed;
        FacebookController.instance.SaveDataGame();
    }

    private string WordSaveDistinct()
    {
        var valieSplit = wordLevelSave.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        var stringDistinct = valieSplit.Distinct().ToList();
        var result = "";
        for (int i = 0; i < stringDistinct.Count; i++)
        {
            var word = stringDistinct[i];
            result += word + "|";
        }
        return result;
    }

    private string BuildLevelName()
    {
        return world + "-" + subWorld + "-" + level;
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !DialogController.instance.IsDialogShowing())
        {
            DialogController.instance.ShowDialog(DialogType.Pause);
        }
    }
#endif

    private void OpenSceneWithAnim(float timeDelay = 0.7f)
    {
        //SceneAnimate.Instance.SceneOpen();
        //ScreenFader.instance.DelayCall(timeDelay, () =>
        //{
        //    animatorScene.enabled = true;
        //    animatorScene.SetBool("PlayAnimScene", true);
        //});
        SceneAnimate.Instance.ShowTip(false, () =>
        {
            animatorScene.enabled = true;
            animatorScene.SetBool("PlayAnimScene", true);
        });
    }

    public void OpenChapterScene()
    {
        CUtils.LoadScene(Const.SCENE_CHAPTER, false);
        Sound.instance.Play(Sound.Others.PopupOpen);

        AudienceNetworkBanner.instance.DisposeAllBannerAd();
    }

    public void PlayAnimScene()
    {
        var isTut = CPlayerPrefs.GetBool("TUTORIAL", false);
        if (world == 0 && subWorld == 0 && level == 0 && !isTut)
        {
            if (SceneAnimate.Instance.animatorScene.GetCurrentAnimatorStateInfo(0).IsName(SceneAnimate.Instance.CloseSceneName))
            {
                OpenSceneWithAnim();
            }
            else
            {
                Pan.instance.centerPoint.gameObject.SetActive(false);
                TweenControl.GetInstance().DelayCall(transform, 0.1f, () =>
                {
                    Pan.instance.centerPoint.gameObject.SetActive(true);
                    BlockScreen.instance.Block(true);
                    //animatorScene.enabled = true;
                    //animatorScene.SetBool("PlayAnimScene", true);
                    OpenSceneWithAnim();
                });
            }
        }
        else
        {
            Pan.instance.centerPoint.gameObject.SetActive(false);
            if (ScreenFader.instance.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ScreenFader_Out"))
            {
                Pan.instance.centerPoint.gameObject.SetActive(true);
                animatorScene.enabled = true;
                animatorScene.SetBool("PlayAnimScene", true);
                ScreenFader.instance.FadeIn(null);
            }
            else
            {
                Pan.instance.centerPoint.gameObject.SetActive(true);
                OpenSceneWithAnim();
            }
        }
    }

    public void PlayAnimSceneDone()
    {
        animatorScene.enabled = true;
        animatorScene.SetBool("PlayAnimScene", true);
    }
}
