using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Superpow;
using TMPro;
using System;
using PlayFab;

public class MainController : BaseController
{
    public Action onLoadDataComplete;

    public GameData gameData;
    public TextMeshProUGUI levelNameText;
    public Animator animatorScene;

    private int world, subWorld, level;
    private bool _isGameComplete;
    private bool _isLevelClear;
    private GameLevel gameLevel;

    public static MainController instance;

    [HideInInspector] public RewardVideoController rewardVideoController;

    private string wordLevelSave;

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
        onLoadDataComplete = PlayAnimScene;
    }

    protected override void Start()
    {
        base.Start();
        CUtils.CloseBannerAd();
        world = GameState.currentWorld;
        subWorld = GameState.currentSubWorld;
        level = GameState.currentLevel;
        var numlevels = Utils.GetNumLevels(world, subWorld);
        var currlevel = (level + numlevels * (subWorld + 5 * world));
        //world = 4;
        //subWorld = 4;
        //level = 4;
        //Debug.Log(world + ", " + subWorld + ", " + level);
        //save level pass;
        
        gameLevel = Utils.Load(world, subWorld, level);
        Pan.instance.Load(gameLevel);
        WordRegion.instance.Load(gameLevel, currlevel);
        BeeManager.instance.Load(CPlayerPrefs.GetInt("amount_bee", 0));

        if (world == 0 && subWorld == 0 && level == 0)
        {
            Timer.Schedule(this, 0.5f, () =>
            {
                //DialogController.instance.ShowDialog(DialogType.HowtoPlay);
            });
        }
        //GameState.currentSubWorldName
        
        levelNameText.text = "LEVEL " + (currlevel + 1);
        
        onLoadDataComplete?.Invoke();
    }

    public void OnComplete()
    {
        if (_isGameComplete) return;
        _isGameComplete = true;

        //Save Passed Word
        if (!CPlayerPrefs.HasKey("WordLevelSave"))
        {
            CPlayerPrefs.SetString("WordLevelSave", gameLevel.answers + "|");
        }
        else
        {
            wordLevelSave = CPlayerPrefs.GetString("WordLevelSave");
            wordLevelSave += gameLevel.answers;
            CPlayerPrefs.SetString("WordLevelSave", wordLevelSave);
        }

        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            var dicData = new Dictionary<string, string>();
            dicData.Add("DICTIONARY", wordLevelSave);
            FacebookController.instance.UpdateUserData(dicData);
        }
        // 
        Timer.Schedule(this, 1f, () =>
        {
            DialogController.instance.ShowDialog(DialogType.Win);
        });
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

    public void OpenChapterScene()
    {
        CUtils.LoadScene(1, false);
        Sound.instance.Play(Sound.Others.PopupOpen);
    }

    public void PlayAnimScene()
    {
        if (ScreenFader.instance.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ScreenFader_Out"))
        {
            animatorScene.enabled = true;
            animatorScene.SetBool("PlayAnimScene", true);
            ScreenFader.instance.FadeIn(null);
        }
        else
        {
            SceneAnimate.Instance.SceneOpen();
            ScreenFader.instance.DelayCall(1f, () =>
            {
                animatorScene.enabled = true;
                animatorScene.SetBool("PlayAnimScene", true);
            });
        }
    }

    public void PlayAnimSceneDone()
    {
        animatorScene.enabled = true;
        animatorScene.SetBool("PlayAnimScene", true);
    }
}
