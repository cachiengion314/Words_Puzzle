using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIScaleController : MonoBehaviour
{
    public Button btnHintTarget;
    public Button btnHint;
    public Button btnMultipleHint;
    public Button btnShuffle;
    public Button btnRewardAds;
    public Button btnBonusBox;
    public Button btnHelp;

    private bool hasAssigned;
    private void Awake()
    {
        SceneManager.activeSceneChanged += ChangedActiveSceneToAssignAllButton;

    }

    private void ChangedActiveSceneToAssignAllButton(Scene currentScene, Scene nextScene)
    {
        int nextSceneIndex = nextScene.buildIndex;
        if (nextSceneIndex == 3 && !hasAssigned)
        {
            btnHint = WordRegion.instance.btnHint;
            btnHintTarget = WordRegion.instance.btnHintTarget;
            btnMultipleHint = WordRegion.instance.btnMultipleHint;
            btnShuffle = WordRegion.instance.btnShuffle;
            btnRewardAds = WordRegion.instance.btnRewardAds;
            btnBonusBox = WordRegion.instance.btnBonusBox;
            btnHelp = WordRegion.instance.btnHelp;

         
            hasAssigned = true;
        }
        else
        {
            hasAssigned = false;
        }
    }
}
