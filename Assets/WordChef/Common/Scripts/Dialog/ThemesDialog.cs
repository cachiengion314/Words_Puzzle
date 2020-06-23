using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemesDialog : Dialog
{
    [SerializeField] private List<ThemeItem> _themes;

    protected override void Start()
    {
        base.Start();
        CheckShowSelectedTheme();
    }

    public void SelectThemes(ThemeItem theme)
    {
        ClearItem();
        CPlayerPrefs.SetInt("CURR_THEMES", theme.idTheme);
        theme.iconSelected.gameObject.SetActive(true);
        theme.btnTheme.interactable = false;
    }

    public override void Close()
    {
        base.Close();
        CUtils.LoadScene(Const.SCENE_MAIN, true);
    }

    private void ClearItem()
    {
        foreach (var item in _themes)
        {
            item.iconSelected.gameObject.SetActive(false);
            item.btnTheme.interactable = true;
        }
    }

    private void CheckShowSelectedTheme()
    {
        ClearItem();
        var iddthem = CPlayerPrefs.GetInt("CURR_THEMES", 0);
        _themes[iddthem].iconSelected.gameObject.SetActive(true);
        _themes[iddthem].btnTheme.interactable = false;
    }
}
