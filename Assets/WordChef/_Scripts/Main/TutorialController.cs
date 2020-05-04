using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public static TutorialController instance { get; private set; }
    [HideInInspector] public bool isShowTut;
    [HideInInspector] public bool isBlockSwipe;
    public string contentPop;
    public string _contentWordAgain;
    public string _contentManipulation;
    public string _contentHintFree;
    [SerializeField] private GameObject _popText;
    [SerializeField] private TextMeshProUGUI _textTutorial;

    private string _answerTarget;

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

    public void ShowPopWordTut(string wordTarget, string contentPop)
    {
        _answerTarget = wordTarget;
        isShowTut = true;
        _popText.SetActive(true);
        _textTutorial.text = contentPop;
        foreach (var line in WordRegion.instance.Lines)
        {
            if(!line.isShown)
            {
                line.SetDataLetter(line.answers[0]);
                break;
            }
        }
    }

    public void ShowPopHintFreeTut()
    {
        isBlockSwipe = true;
        isShowTut = true;
        _popText.SetActive(true);
        _textTutorial.text = _contentHintFree;
    }

    public void HidenPopTut()
    {
        isShowTut = false;
        isBlockSwipe = false;
        _popText.SetActive(false);
        _textTutorial.text = "";
    }
}
