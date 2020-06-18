using Superpow;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeeController : MonoBehaviour
{
    [SerializeField] private Image _beeImage;
    [SerializeField] private TextMeshProUGUI _textAmountBee;
    [Header("TEST")]
    [SerializeField] private int _number = 1;

    void Start()
    {
        //UpdateAmountBee();
        //OnBeeButtonClick();
    }

    private void UpdateAmountBee()
    {
        _textAmountBee.text = BeeManager.instance.CurrBee.ToString();
    }

    public void OnBeeButtonClick()
    {
        var numlevels = Utils.GetNumLevels(GameState.currentWorld, GameState.currentSubWorld);
        var currlevel = (GameState.currentLevel + numlevels * GameState.currentSubWorld + MainController.instance.gameData.words[0].subWords.Count * GameState.currentWorld * numlevels) + 1;
        var isUsed = WordRegion.instance.Lines.Any(line => line.usedBee);
        var isCellClear = WordRegion.instance.Lines.All(line => line.cells.All(cell => !cell.isShown));
        if (BeeManager.instance.CurrBee > 0 && !isUsed && isCellClear && Prefs.IsSaveLevelProgress())
        {
            MainController.instance.isBeePlay = true;
            if (WordRegion.instance.IsUseBee())
                return;
            if ((currlevel == 8 && !CPlayerPrefs.HasKey("BEE_TUTORIAL")) || currlevel <= 40 && !CPlayerPrefs.HasKey("BEE_TUTORIAL"))
            {
                MainController.instance.isBeePlay = false;
                TutorialController.instance.CheckAndShowTutorial();
            }
            else
                WordRegion.instance.BeeClick();
            UpdateAmountBee();
        }
        else
        {
            MainController.instance.isBeePlay = false;
            TutorialController.instance.CheckAndShowTutorial();
            //DialogController.instance.ShowDialog(DialogType.Bee);
        }
    }


    #region TEST BEE
    public void AddAmountBee()
    {
        BeeManager.instance.CreaditAmountBee(_number);
        UpdateAmountBee();
    }
    #endregion
}
