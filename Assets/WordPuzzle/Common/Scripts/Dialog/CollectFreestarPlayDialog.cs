using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectFreestarPlayDialog : Dialog
{
    [Header("THEME UI CHANGE")]
    [SerializeField] private Image _iconGift;
    [SerializeField] private Image _iconItem;
    [SerializeField] private Image _imgBtn;
    [SerializeField] private TextMeshProUGUI _txtBtnWatch;
    [SerializeField] private TextMeshProUGUI _txtContent;
    [SerializeField] private TextMeshProUGUI _txtContent2;
    [SerializeField] private TextMeshProUGUI _txtItemValue;

    protected override void Start()
    {
        base.Start();
        CheckTheme();
        _txtItemValue.text = SceneAnimate.Instance.itemValue.ToString();
    }

    private void CheckTheme()
    {
        if (MainController.instance != null)
        {
            var currTheme = ThemesControl.instance.CurrTheme;
            _iconGift.sprite = currTheme.uiData.collectFreestarPlayData.iconGift;
            _imgBtn.sprite = currTheme.uiData.collectFreestarPlayData.btnCollect;

            _iconGift.SetNativeSize();
            _imgBtn.SetNativeSize();

            _txtBtnWatch.color = currTheme.uiData.collectFreestarPlayData.colorTextBtn;
            _txtContent.color = _txtContent2.color = _txtItemValue.color = currTheme.fontData.colorContentDialog;

            var item = SceneAnimate.Instance.itemType;
            if (item == ItemType.CURRENCY_BALANCE)
                _iconItem.sprite = currTheme.uiData.freestarPlayData.iconStar;
            else if (item == ItemType.HINT)
                _iconItem.sprite = currTheme.uiData.freestarPlayData.iconHint;
            else if (item == ItemType.HINT_SELECT)
                _iconItem.sprite = currTheme.uiData.freestarPlayData.iconSelectedHint;
            else if (item == ItemType.HINT_RANDOM)
                _iconItem.sprite = currTheme.uiData.freestarPlayData.iconMultipleHint;
        }
        else
        {
            var currTheme = ThemesControl.instance.ThemesDatas[0];
            var item = SceneAnimate.Instance.itemType;
            if (item == ItemType.CURRENCY_BALANCE)
                _iconItem.sprite = currTheme.uiData.freestarPlayData.iconStar;
            else if (item == ItemType.HINT)
                _iconItem.sprite = currTheme.uiData.freestarPlayData.iconHint;
            else if (item == ItemType.HINT_SELECT)
                _iconItem.sprite = currTheme.uiData.freestarPlayData.iconSelectedHint;
            else if (item == ItemType.HINT_RANDOM)
                _iconItem.sprite = currTheme.uiData.freestarPlayData.iconMultipleHint;
        }
    }

    public void OnCollectClick()
    {
        var itemValue = SceneAnimate.Instance.itemValue;
        switch (SceneAnimate.Instance.itemType)
        {
            case ItemType.HINT:
                CurrencyController.CreditHintFree(itemValue);
                SceneAnimate.Instance.ShowItemCollect();
                break;
            case ItemType.HINT_RANDOM:
                CurrencyController.CreditMultipleHintFree(itemValue);
                SceneAnimate.Instance.ShowItemCollect();
                break;
            case ItemType.HINT_SELECT:
                CurrencyController.CreditSelectedHintFree(itemValue);
                SceneAnimate.Instance.ShowItemCollect();
                break;
            case ItemType.CURRENCY_BALANCE:
                SceneAnimate.Instance.overlay.gameObject.SetActive(false);
                StartCoroutine(SceneAnimate.Instance.ShowEffectCollect(itemValue));
                MonoUtils.instance.ShowTotalStarCollect(itemValue, MonoUtils.instance.textCollectDefault, 1.33f);
                break;
        }
        Close();
    }
}
