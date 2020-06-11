using Superpow;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public static TutorialController instance { get; private set; }
    [HideInInspector] public bool isShowTut;
    [HideInInspector] public bool isBlockSwipe;
    [HideInInspector] public bool isTutBeehive;
    public string contentNext;
    public string contentWordAgain;
    public string contentManipulation;
    public string contentHintFree;
    public string contentShuffle;
    public string contentSelectedHint;
    public string contentSelectedHint2;
    public string contentMultipleHint;
    public string contentBonusBox;
    public string contentCellStar;
    public string contentSetting;
    public string contentBeehive;
    public string contentHelp;
    public string contentChickenBank;
    [SerializeField] private GameObject _popText;
    [SerializeField] private GameObject _popHint;
    [SerializeField] private GameObject _popShuffle;
    [SerializeField] private GameObject _popSelectedHint;
    [SerializeField] private GameObject _popSelectedHint2;
    [SerializeField] private GameObject _popMultipleHint;
    [SerializeField] private GameObject _popBonusBox;
    [SerializeField] private GameObject _popCellStar;
    [SerializeField] private GameObject _popSetting;
    [SerializeField] private GameObject _popObjective;
    [SerializeField] private GameObject _popBeehive;
    [SerializeField] private GameObject _popHelp;
    [SerializeField] private GameObject _popChickenBank;
    [SerializeField] private GameObject _overlay;
    [SerializeField] private TextMeshProUGUI _textTutorial;
    [SerializeField] private TextMeshProUGUI _textTutorialHint;
    [SerializeField] private TextMeshProUGUI _textTutorialShuffle;
    [SerializeField] private TextMeshProUGUI _textTutorialSelectedHint;
    [SerializeField] private TextMeshProUGUI _textTutorialSelectedHint2;
    [SerializeField] private TextMeshProUGUI _textTutorialMultipleHint;
    [SerializeField] private TextMeshProUGUI _textTutorialBonusBox;
    [SerializeField] private TextMeshProUGUI _textTutorialCellStar;
    [SerializeField] private TextMeshProUGUI _textTutorialSetting;
    [SerializeField] private TextMeshProUGUI _textTutorialBeehive;
    [SerializeField] private TextMeshProUGUI _textTutorialHelp;
    [SerializeField] private TextMeshProUGUI _textTutorialChickenBank;

    private LineWord _lineTarget;
    private string _answerTarget;

    public LineWord LineTarget
    {
        get
        {
            return _lineTarget;
        }
    }

    public string AnswerTarget
    {
        get
        {
            return _answerTarget;
        }
    }

    void Start()
    {
        if (instance == null) instance = this;
    }

    public void ShowPopWordTut(string contentPop)
    {
        isShowTut = true;
        _overlay.SetActive(true);
        _overlay.GetComponent<Canvas>().sortingOrder = 0;
        _popText.SetActive(true);
        foreach (var line in WordRegion.instance.Lines)
        {
            if (!line.isShown)
            {
                _lineTarget = line;
                _answerTarget = line.answers[0];
                line.SetDataLetter(line.answers[0]);
                _textTutorial.text = contentPop + " <color=green>" + _answerTarget + "</color>";
                LineTarget.GetComponent<Canvas>().overrideSorting = true;
                LineTarget.lineTutorialBG.gameObject.SetActive(true);
                break;
            }
        }
    }

    public void ShowPopHintFreeTut()
    {
        if (WordRegion.instance != null)
        {
            WordRegion.instance.btnHint.gameObject.SetActive(true);
            WordRegion.instance.btnHint.GetComponent<Canvas>().overrideSorting = true;
        }
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        _popHint.SetActive(true);
        _textTutorialHint.text = contentHintFree;
    }

    public void ShowPopShuffleTut()
    {
        if (WordRegion.instance != null)
        {
            WordRegion.instance.btnShuffle.gameObject.SetActive(true);
            WordRegion.instance.btnShuffle.GetComponent<Canvas>().overrideSorting = true;
        }
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        _popShuffle.SetActive(true);
        _textTutorialShuffle.text = contentShuffle;
    }

    public void ShowPopBonusBoxTut()
    {
        if (WordRegion.instance != null)
        {
            WordRegion.instance.btnBonusBox.gameObject.SetActive(true);
            WordRegion.instance.btnBonusBox.GetComponent<Canvas>().overrideSorting = true;
        }
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        _popBonusBox.SetActive(true);
        _textTutorialBonusBox.text = contentBonusBox;
    }

    public void ShowPopSelectedHintTut()
    {
        if (WordRegion.instance != null)
        {
            WordRegion.instance.btnHintTarget.gameObject.SetActive(true);
            WordRegion.instance.btnHintTarget.GetComponent<Canvas>().overrideSorting = true;
        }
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        _popSelectedHint.SetActive(true);
        _textTutorialSelectedHint.text = contentSelectedHint;
    }

    public void ShowPopSelectedHint2Tut()
    {
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        _popSelectedHint2.SetActive(true);
        _textTutorialSelectedHint2.text = contentSelectedHint2;
    }

    public void ShowPopMultipleTut()
    {
        if (WordRegion.instance != null)
        {
            WordRegion.instance.btnMultipleHint.gameObject.SetActive(true);
            WordRegion.instance.btnMultipleHint.GetComponent<Canvas>().overrideSorting = true;
        }
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        _popMultipleHint.SetActive(true);
        _textTutorialMultipleHint.text = contentMultipleHint;
    }

    public void ShowPopCellStarTut()
    {
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        _popCellStar.SetActive(true);
        _textTutorialCellStar.text = contentCellStar;

        _lineTarget = WordRegion.instance.Lines[WordRegion.instance.Lines.Count - 1];
        var canvas = LineTarget.GetComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingLayerName = "UI";
        LineTarget.lineTutorialBG.gameObject.SetActive(true);

        foreach (var cellTut in _lineTarget.cells)
        {
            if (!cellTut.isShown)
                cellTut.iconCoin.transform.localScale = Vector3.one;
        }
    }

    public void ShowPopSettingTut()
    {
        if (WordRegion.instance != null)
            WordRegion.instance.btnSetting.GetComponent<Canvas>().overrideSorting = true;
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        _popSetting.SetActive(true);
        _textTutorialSetting.text = contentSetting;
    }

    public void ShowPopObjectiveTut()
    {
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        var canvasOverlay = _overlay.GetComponent<Canvas>();
        canvasOverlay.sortingLayerName = "UI2";
        _popObjective.SetActive(true);
    }

    public void ShowPopBeeTut()
    {
        isShowTut = true;
        isBlockSwipe = true;
        isTutBeehive = true;
        _overlay.SetActive(true);
        _popBeehive.SetActive(true);
        _textTutorialBeehive.text = contentBeehive;
    }

    public void BeeFly()
    {
        MainController.instance.beeController.OnBeeButtonClick();
    }

    public void ShowPopHelpTut()
    {
        if (WordRegion.instance != null)
        {
            WordRegion.instance.btnHelp.gameObject.SetActive(true);
            WordRegion.instance.btnHelp.GetComponent<Canvas>().overrideSorting = true;
        }
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        _popHelp.SetActive(true);
        _textTutorialHelp.text = contentHelp;
    }

    public void ShowPopChickenBankTut()
    {
        if (WinDialog.instance != null)
        {
            WinDialog.instance._chickenBank.SetActive(true);
            var canvas = WinDialog.instance._chickenBank.GetComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingLayerName = "UI2";
            canvas.sortingOrder = 5;
        }
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        var canvasOverlay = _overlay.GetComponent<Canvas>();
        canvasOverlay.sortingLayerName = "UI2";
        _popChickenBank.SetActive(true);
        _textTutorialChickenBank.text = contentChickenBank;
    }

    public void HidenPopTut()
    {
        if (isTutBeehive)
        {
            CPlayerPrefs.SetBool("BEE_TUTORIAL", true);
            MainController.instance.beeController.OnBeeButtonClick();
        }
        if (WordRegion.instance != null)
        {
            WordRegion.instance.btnHint.GetComponent<Canvas>().overrideSorting = false;
            WordRegion.instance.btnShuffle.GetComponent<Canvas>().overrideSorting = false;
            WordRegion.instance.btnMultipleHint.GetComponent<Canvas>().overrideSorting = false;
            WordRegion.instance.btnHintTarget.GetComponent<Canvas>().overrideSorting = false;
            WordRegion.instance.btnSetting.GetComponent<Canvas>().overrideSorting = false;
            WordRegion.instance.btnHelp.GetComponent<Canvas>().overrideSorting = false;
            if (LineTarget != null)
            {
                LineTarget.GetComponent<Canvas>().overrideSorting = false;
                LineTarget.lineTutorialBG.gameObject.SetActive(false);
            }
        }
        if (WinDialog.instance != null)
        {
            WinDialog.instance._chickenBank.GetComponent<Canvas>().overrideSorting = false;
        }
        var canvasOverlay = _overlay.GetComponent<Canvas>();
        canvasOverlay.sortingLayerName = "Default";
        canvasOverlay.sortingOrder = 3;
        isShowTut = false;
        isBlockSwipe = false;
        isTutBeehive = false;
        _popText.SetActive(false);
        _popHint.SetActive(false);
        _popShuffle.SetActive(false);
        _popSelectedHint.SetActive(false);
        _popSelectedHint2.SetActive(false);
        _popMultipleHint.SetActive(false);
        _popBonusBox.SetActive(false);
        _popCellStar.SetActive(false);
        _popSetting.SetActive(false);
        _popObjective.SetActive(false);
        _popBeehive.SetActive(false);
        _popHelp.SetActive(false);
        _popChickenBank.SetActive(false);
        _overlay.SetActive(false);
        _textTutorial.text = "";
    }

    public void CheckAndShowTutorial()
    {
        var numlevels = Utils.GetNumLevels(GameState.currentWorld, GameState.currentSubWorld);
        var currlevel = (GameState.currentLevel + numlevels * (GameState.currentSubWorld + MainController.instance.gameData.words.Count * GameState.currentWorld)) + 1;

        if (!CPlayerPrefs.HasKey("LEVEL " + currlevel))
        {
            if ((currlevel >= 11 && !CPlayerPrefs.HasKey("BEE_TUTORIAL")) || (BeeManager.instance.CurrBee > 0 && !CPlayerPrefs.HasKey("BEE_TUTORIAL")))
            {
                BeeManager.instance.CreaditAmountBee(3);
                ShowPopBeeTut();
            }
            else if ((currlevel >= 2 && !CPlayerPrefs.HasKey("HINT_TUTORIAL")) || (CurrencyController.GetHintFree() > 0 && !CPlayerPrefs.HasKey("HINT_TUTORIAL")))
            {
                CurrencyController.CreditHintFree(2);
                ShowPopHintFreeTut();
                CPlayerPrefs.SetBool("HINT_TUTORIAL", true);
            }
            else if ((currlevel >= 8 && !CPlayerPrefs.HasKey("OBJ_TUTORIAL")) || (Prefs.countLevelDaily >= 2 && !CPlayerPrefs.HasKey("OBJ_TUTORIAL")))
            {
                ShowPopSettingTut();
            }
            else if ((currlevel >= 9 && !CPlayerPrefs.HasKey("SELECTED_HINT_TUTORIAL")) || (CurrencyController.GetSelectedHintFree() > 0 && !CPlayerPrefs.HasKey("SELECTED_HINT_TUTORIAL")))
            {
                CurrencyController.CreditSelectedHintFree(2);
                ShowPopSelectedHintTut();
            }
            else if ((currlevel >= 10 && !CPlayerPrefs.HasKey("MULTIPLE_HINT_TUTORIAL")) || (CurrencyController.GetSelectedHintFree() > 0 && !CPlayerPrefs.HasKey("MULTIPLE_HINT_TUTORIAL")))
            {
                CurrencyController.CreditMultipleHintFree(1);
                ShowPopMultipleTut();
            }
            else if (currlevel >= 4 && !CPlayerPrefs.HasKey("SHUFFLE_TUTORIAL"))
            {
                CPlayerPrefs.SetBool("SHUFFLE_TUTORIAL", true);
                ShowPopShuffleTut();
            }
            else if (currlevel >= 5 && !CPlayerPrefs.HasKey("CELL_STAR_TUTORIAL"))
            {
                CPlayerPrefs.SetBool("CELL_STAR_TUTORIAL", true);
                ShowPopCellStarTut();
            }
            else if (currlevel >= 6 && !CPlayerPrefs.HasKey("HELP_TUTORIAL"))
            {
                CPlayerPrefs.SetBool("HELP_TUTORIAL", true);
                ShowPopHelpTut();
            }
            else if (currlevel >= 7 && !CPlayerPrefs.HasKey("TUT_EXTRA_WORD"))
            {
                CPlayerPrefs.SetBool("TUT_EXTRA_WORD", true);
                ShowPopBonusBoxTut();
            }

            CPlayerPrefs.SetBool("LEVEL " + currlevel, true);
        }
    }
}
