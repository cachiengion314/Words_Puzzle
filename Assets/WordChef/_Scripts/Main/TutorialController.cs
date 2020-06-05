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
    [SerializeField] private GameObject _popText;
    [SerializeField] private GameObject _popHint;
    [SerializeField] private GameObject _popShuffle;
    [SerializeField] private GameObject _overlay;
    [SerializeField] private TextMeshProUGUI _textTutorial;
    [SerializeField] private TextMeshProUGUI _textTutorialHint;
    [SerializeField] private TextMeshProUGUI _textTutorialShuffle;

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
        WordRegion.instance.btnHint.GetComponent<Canvas>().overrideSorting = true;
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        _popHint.SetActive(true);
        _textTutorialHint.text = contentHintFree;
    }

    public void ShowPopShuffleTut()
    {
        WordRegion.instance.btnShuffle.GetComponent<Canvas>().overrideSorting = true;
        isShowTut = true;
        isBlockSwipe = true;
        _overlay.SetActive(true);
        _popShuffle.SetActive(true);
        _textTutorialShuffle.text = contentShuffle;
    }

    public void HidenPopTut()
    {
        WordRegion.instance.btnHint.GetComponent<Canvas>().overrideSorting = false;
        WordRegion.instance.btnShuffle.GetComponent<Canvas>().overrideSorting = false;
        isShowTut = false;
        isBlockSwipe = false;
        _popText.SetActive(false);
        _popHint.SetActive(false);
        _popShuffle.SetActive(false);
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

            }
            else if (currlevel == 11)
            {

            }
            else if (currlevel == 12)
            {

            }
            else if (currlevel == 23)
            {

            }
            else if (currlevel == 30)
            {

            }
            else if (currlevel == 33)
            {

            }
            CPlayerPrefs.SetBool("LEVEL " + currlevel, true);
        }
    }
}
