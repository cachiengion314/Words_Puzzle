using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComingSoonDialog : Dialog
{
    [Header("THEME UI CHANGE")]
    [SerializeField] private Image _imgMessage;
    [SerializeField] private Button _btnClose;
    [SerializeField] private TextMeshProUGUI _txtMessage;
    [SerializeField] private TextMeshProUGUI _txtBtnClose;

    protected override void Start()
    {
        base.Start();
        CheckTheme();
    }

    private void CheckTheme()
    {
        if(MainController.instance != null)
        {
            var currTheme = ThemesControl.instance.CurrTheme;
            _imgMessage.sprite = currTheme.uiData.comingSoonData.message;
            _btnClose.image.sprite = currTheme.uiData.comingSoonData.btnClose;

            _imgMessage.SetNativeSize();
            _btnClose.image.SetNativeSize();

            _txtMessage.color = currTheme.fontData.colorContentDialog;
            _txtBtnClose.color = currTheme.uiData.comingSoonData.colorTextBtn;
        }
    }
}
