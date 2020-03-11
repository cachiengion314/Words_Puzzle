using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Superpow;
using TMPro;

public class MainController : BaseController {
    public TextMeshProUGUI levelNameText;

    private int world, subWorld, level;
    private bool isGameComplete;
    private GameLevel gameLevel;

    public static MainController instance;

    private string wordLevelSave;

    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }

    protected override void Start()
    {
        base.Start();
        CUtils.CloseBannerAd();

        world = GameState.currentWorld;
        subWorld = GameState.currentSubWorld;
        level = GameState.currentLevel;
        //world = 4;
        //subWorld = 4;
        //level = 4;
        Debug.Log(world + ", " + subWorld + ", " + level);
        //save level pass;


        gameLevel = Utils.Load(world, subWorld, level);
        Pan.instance.Load(gameLevel);
        WordRegion.instance.Load(gameLevel);
        BeeManager.instance.Load(CPlayerPrefs.GetInt("amount_bee",0));

        if (world == 0 && subWorld == 0 && level == 0)
        {
            Timer.Schedule(this, 0.5f, () =>
            {
                //DialogController.instance.ShowDialog(DialogType.HowtoPlay);
            });
        }

        //GameState.currentSubWorldName
        levelNameText.text = "LEVEL " + (level + 1);
    }

    public void OnComplete()
    {
        if (isGameComplete) return;
        isGameComplete = true;
        
        //Save Passed Word
        if (!CPlayerPrefs.HasKey("WordLevelSave"))
        {
            CPlayerPrefs.SetString("WordLevelSave", gameLevel.answers);
        }
        else
        {
            wordLevelSave = CPlayerPrefs.GetString("WordLevelSave");
            wordLevelSave += gameLevel.answers;
            CPlayerPrefs.SetString("WordLevelSave", wordLevelSave);
        }
        // 

        Timer.Schedule(this, 1f, () =>
        {
            DialogController.instance.ShowDialog(DialogType.Win);
            Sound.instance.Play(Sound.Others.Win);
        });
    }

    private string BuildLevelName()
    {
        return world + "-" + subWorld + "-" + level;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !DialogController.instance.IsDialogShowing())
        {
            DialogController.instance.ShowDialog(DialogType.Pause);
        }
    }

    public void OpenChapterScene()
    {
        CUtils.LoadScene(1, false);
        Sound.instance.PlayButton();
    }
}
