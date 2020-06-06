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
    public string contentNext;
    public string contentWordAgain;
    public string contentManipulation;
    public string contentHintFree;
    public string contentShuffle;
    public string contentSelectedHint;
    public string contentMultipleHint;
    public string contentBonusBox;
    public string contentCellStar;
    [SerializeField] private GameObject _popText;
    [SerializeField] private GameObject _popHint;
    [SerializeField] private GameObject _popShuffle;
    [SerializeField] private GameObject _popSelectedHint;
    [SerializeField] private GameObject _popMultipleHint;
    [SerializeField] private GameObject _popBonusBox;
    [SerializeField] private GameObject _popCellStar;
    [SerializeField] private GameObject _overlay;
    [SerializeField] private TextMeshProUGUI _textTutorial;
    [SerializeField] private TextMeshProUGUI _textTutorialHint;
    [SerializeField] private TextMeshProUGUI _textTutorialShuffle;
    [SerializeField] private TextMeshProUGUI _textTutorialSelectedHint;
    [SerializeField] private TextMeshProUGUI _textTutorialMultipleHint;
    [SerializeField] private TextMeshProUGUI _textTutorialBonusBox;
    [SerializeField] private TextMeshProUGUI _textTutorialCellStar;

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
            WordRegion.instance.btnHint.GetComponent<Canvas>().overrideSorting = true;
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        _popHint.SetActive(true);
        _textTutorialHint.text = contentHintFree;
    }

    public void ShowPopShuffleTut()
    {
        if (WordRegion.instance != null)
            WordRegion.instance.btnShuffle.GetComponent<Canvas>().overrideSorting = true;
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        _popShuffle.SetActive(true);
        _textTutorialShuffle.text = contentShuffle;
    }

    public void ShowPopBonusBoxTut()
    {
        if (WordRegion.instance != null)
            WordRegion.instance.btnBonusBox.GetComponent<Canvas>().overrideSorting = true;
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        _popBonusBox.SetActive(true);
        _textTutorialBonusBox.text = contentBonusBox;
    }

    public void ShowPopSelectedHintTut()
    {
        if (WordRegion.instance != null)
            WordRegion.instance.btnHintTarget.GetComponent<Canvas>().overrideSorting = true;
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        _popSelectedHint.SetActive(true);
        _textTutorialSelectedHint.text = contentSelectedHint;
    }

    public void ShowPopMultipleTut()
    {
        if (WordRegion.instance != null)
            WordRegion.instance.btnMultipleHint.GetComponent<Canvas>().overrideSorting = true;
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
        _textTutorialCellStar.text = contentMultipleHint;

        _lineTarget = WordRegion.instance.Lines[WordRegion.instance.Lines.Count - 1];
        var canvas = LineTarget.GetComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingLayerName = "UI";
        LineTarget.lineTutorialBG.gameObject.SetActive(true);
    }

    public void HidenPopTut()
    {
        if (WordRegion.instance != null)
        {
            WordRegion.instance.btnHint.GetComponent<Canvas>().overrideSorting = false;
            WordRegion.instance.btnShuffle.GetComponent<Canvas>().overrideSorting = false;
            WordRegion.instance.btnMultipleHint.GetComponent<Canvas>().overrideSorting = false;
            WordRegion.instance.btnHintTarget.GetComponent<Canvas>().overrideSorting = false;
            if (LineTarget != null)
            {
                LineTarget.GetComponent<Canvas>().overrideSorting = false;
                LineTarget.lineTutorialBG.gameObject.SetActive(false);
            }
        }
        isShowTut = false;
        isBlockSwipe = false;
        _popText.SetActive(false);
        _popHint.SetActive(false);
        _popShuffle.SetActive(false);
        _popSelectedHint.SetActive(false);
        _popMultipleHint.SetActive(false);
        _popBonusBox.SetActive(false);
        _popCellStar.SetActive(false);
        _overlay.SetActive(false);
        _textTutorial.text = "";
    }

    public void CheckAndShowTutorial()
    {
        var numlevels = Utils.GetNumLevels(GameState.currentWorld, GameState.currentSubWorld);
        var currlevel = (GameState.currentLevel + numlevels * (GameState.currentSubWorld + MainController.instance.gameData.words.Count * GameState.currentWorld)) + 1;

        if (!CPlayerPrefs.HasKey("LEVEL " + currlevel))
        {
            if (currlevel == 2)
            {
                CurrencyController.CreditHintFree(2);
                ShowPopHintFreeTut();
            }
            else if (currlevel == 6)
            {
                ShowPopShuffleTut();
            }
            else if (currlevel == 8)
            {
                ShowPopCellStarTut();
            }
            else if (currlevel == 11)
            {

            }
            else if (currlevel == 12 && !CPlayerPrefs.HasKey("TUT_EXTRA_WORD"))
            {
                ShowPopBonusBoxTut();
            }
            else if (currlevel == 23)
            {
                CurrencyController.CreditSelectedHintFree(2);
                ShowPopSelectedHintTut();
            }
            else if (currlevel == 30)
            {
                CurrencyController.CreditMultipleHintFree(1);
                ShowPopMultipleTut();
            }
            else if (currlevel == 33)
            {

            }
            CPlayerPrefs.SetBool("LEVEL " + currlevel, true);
        }
    }
}
