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
        GetComponent<GraphicRaycaster>().enabled = false;
        ClearItem();
        CPlayerPrefs.SetInt("CURR_THEMES", theme.idTheme);
        theme.iconSelected.gameObject.SetActive(true);
        theme.btnTheme.interactable = false;
        TweenControl.GetInstance().DelayCall(transform, 0.5f,()=> {
            Close();
            CUtils.LoadScene(Const.SCENE_MAIN, true);
        });
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
