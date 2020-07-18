using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ThemesDialog : Dialog
{
    public static ThemesDialog instance;
    [SerializeField] private List<ThemeItem> _themes;
    [SerializeField] Button _btnShop;
    [SerializeField] Button _btnBack;
    private bool _themeExits;

    protected override void Start()
    {
        base.Start();
        instance = this;
        CheckShowSelectedTheme();
        CheckIsFirtGame();
    }

    private void CheckIsFirtGame()
    {
        var isTut = CPlayerPrefs.GetBool("TUTORIAL", false);
        if (!isTut && GameState.currentLevel == 0 && GameState.currentSubWorld == 0 && GameState.currentWorld == 0)
        {
            _btnShop.enabled = _btnBack.enabled  =  false;            
        }
    }

    public void SelectThemes(ThemeItem theme)
    {
        var isCloseFirstTheme = CPlayerPrefs.GetBool("CLOSE_THEME_DIALOG", false);
        GetComponent<GraphicRaycaster>().enabled = false;
        ClearItem();
        var iddthem = CPlayerPrefs.GetInt("CURR_THEMES", 0);
        if (_themes[iddthem] != theme)
            _themeExits = false;
        else
            _themeExits = true;
        CPlayerPrefs.SetInt("CURR_THEMES", theme.idTheme);
        theme.iconSelected.gameObject.SetActive(true);
        theme.btnTheme.interactable = false;
        TweenControl.GetInstance().DelayCall(transform, 0.5f, () =>
        {
            if (!isCloseFirstTheme || !_themeExits)
            {
                CPlayerPrefs.SetBool("CLOSE_THEME_DIALOG", true);
                Close();
                CUtils.LoadScene(Const.SCENE_MAIN, true);
            }
            else
                Close();
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
        //_themes[iddthem].btnTheme.interactable = false;
    }

    public override void Close()
    {
        var isCloseFirstTheme = CPlayerPrefs.GetBool("CLOSE_THEME_DIALOG", false);
        base.Close();
        if (!isCloseFirstTheme)
        {
            CPlayerPrefs.SetBool("CLOSE_THEME_DIALOG", true);
            CUtils.LoadScene(Const.SCENE_MAIN, true);
        }

        AudienceNetworkBanner.instance.LoadBanner();
    }
}
