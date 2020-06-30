using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotifyMessage : MonoBehaviour
{
    public static NotifyMessage instance;
    [SerializeField] private TextMeshProUGUI _textContent;
    [SerializeField] private GameObject _panelMessage;
    [SerializeField] private float _timeShow = 1f;

    public string WORD_EXIST = "You found this word already";
    public string WORD_LENGTH_REQUIREMENT = "Let's seek for the words meeting the word length requirement!";
    public Image bgToast;
    private bool isShowMess;

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void ShowMessage(string content, Action callback = null)
    {
        if (isShowMess)
            return;
        isShowMess = true;
        _textContent.text = content;
        if (MainController.instance != null)
        {
            _textContent.color = ThemesControl.instance.CurrTheme.fontData.colorLetter;
            bgToast.sprite = ThemesControl.instance.CurrTheme.uiData.bgToast;
        }
        _panelMessage.SetActive(true);
        TweenControl.GetInstance().DelayCall(transform, _timeShow, () =>
        {
            HidenMessage();
            callback?.Invoke();
        });
    }

    public void HidenMessage()
    {
        _panelMessage.SetActive(false);
        isShowMess = false;
    }
}
