using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Superpow;
using TMPro;
using System;
using PlayFab;
using System.Linq;

public class MainController : BaseController
{
    public Action onLoadDataComplete;

    public GameData gameData;
    public TextMeshProUGUI levelNameText;
    public Animator animatorScene;
    public BeeController beeController;

    private int world, subWorld, level;
    private bool _isGameComplete;
    private bool _isLevelClear;
    private GameLevel gameLevel;

    public static MainController instance;

    [HideInInspector] public bool isBeePlay;

    private string wordLevelSave;
    private string _wordPassed;

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
        CUtils.CloseBannerAd();
        world = GameState.currentWorld;
        subWorld = GameState.currentSubWorld;
        level = GameState.currentLevel;
        var numlevels = Utils.GetNumLevels(world, subWorld);
        var currlevel = (level + numlevels * (subWorld + gameData.words.Count * world));
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
                var isTut = CPlayerPrefs.GetBool("TUTORIAL", false);
                //DialogController.instance.ShowDialog(DialogType.HowtoPlay);
                if (!isTut)
                    TutorialController.instance.ShowPopWordTut(TutorialController.instance.contentManipulation);
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
        //CPlayerPrefs.DeleteKey("HINT_LINE_INDEX");
        //Save Passed Word
        //SaveWordComplete(gameLevel.answers);

        //if (PlayFabClientAPI.IsClientLoggedIn())
        //{
        //FacebookController.instance.user.wordPassed = _wordPassed;
        //FacebookController.instance.SaveDataGame();
        //}
        // 
        Timer.Schedule(this, 1f, () =>
        {
            DialogController.instance.ShowDialog(DialogType.Win);
        });
    }

    public void SaveWordComplete(string wordDone)
    {
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

    public void OpenChapterScene()
    {
        CUtils.LoadScene(Const.SCENE_CHAPTER, false);
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
