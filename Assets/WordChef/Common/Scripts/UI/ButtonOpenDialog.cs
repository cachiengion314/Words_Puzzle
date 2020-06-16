using UnityEngine;
using System.Collections;
using Superpow;
using Utilities.Common;

public class ButtonOpenDialog : MyButton
{
    public bool isObjectives;
    public string contentTitle;
    public string contentMesage;
    public DialogType dialogType;
    public DialogShow dialogShow;
    public override void OnButtonClick()
    {
        ConfigController.instance.isShopHint = false;
        base.OnButtonClick();
        if (isObjectives)
            CheckShowObjectivesDialog();
        else
            DialogController.instance.ShowDialog(dialogType, dialogShow, contentTitle, contentMesage);
    }

    private void CheckShowObjectivesDialog()
    {
        var gameData = Resources.Load<GameData>("GameData");
        var numlevels = Utils.GetNumLevels(GameState.currentWorld, GameState.currentSubWorld);
        var currlevel = (GameState.currentLevel + numlevels * GameState.currentSubWorld + gameData.words[0].subWords.Count * numlevels * GameState.currentWorld) + 1;
        Sound.instance.Play(Sound.Others.PopupOpen);
        if ((currlevel < 11 && !CPlayerPrefs.HasKey("OBJ_TUTORIAL")) || (Prefs.countLevelDaily < 2 && !CPlayerPrefs.HasKey("OBJ_TUTORIAL")))
            DialogController.instance.ShowDialog(DialogType.ComingSoon, DialogShow.STACK_DONT_HIDEN, contentTitle, contentMesage.Replace("\\n", "\n"));
        else
            DialogController.instance.ShowDialog(DialogType.Objective, DialogShow.STACK_DONT_HIDEN);
    }
}
