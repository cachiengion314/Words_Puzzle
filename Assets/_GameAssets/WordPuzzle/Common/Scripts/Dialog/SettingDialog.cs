using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SettingDialog : Dialog
{
    [SerializeField]
    private int bundleVersionCode;
    [SerializeField]
    GameObject soundButton;
    [SerializeField]
    GameObject musicButton;

    [SerializeField] private GameObject notiBttOff;

    [SerializeField] private GameObject effectBttOff;


    [SerializeField] private Slider _sliderSound;
    [SerializeField] private Slider _sliderMusic;

    GameObject gameMaster;
    Sound soundController;
    Music musicController;

    [SerializeField] private GameObject _btnLogout;
    [SerializeField] private Text _textNameUser;
    [SerializeField] private GameObject _panelExit;
    [SerializeField] private GameObject _panelOverlay;

    [Header("THEME UI CHANGE")]
    [SerializeField] private Image _iconSound;
    [SerializeField] private Image _iconMusic;
    [SerializeField] private Image _iconEffect;
    [SerializeField] private Image _iconEffectOn;
    [SerializeField] private Image _iconEffectOff;
    [SerializeField] private Image _iconNotification;
    [SerializeField] private Image _iconNotificationOn;
    [SerializeField] private Image _iconNotificationOff;
    [SerializeField] private Image _iconArrow;
    [SerializeField] private Image _iconArrow2;
    [SerializeField] private Image _iconArrow3;
    [SerializeField] private Image _handleSound;
    [SerializeField] private Image _bgProgressSound;
    [SerializeField] private Image _fillProgressSound;
    [SerializeField] private Image _fillSound;
    [SerializeField] private Image _frameMaskSound;
    [SerializeField] private Image _handleMusic;
    [SerializeField] private Image _bgProgressMusic;
    [SerializeField] private Image _fillProgressMusic;
    [SerializeField] private Image _fillMusic;
    [SerializeField] private Image _frameMaskMusic;
    [SerializeField] private Image _line;
    [SerializeField] private Image _line2;
    [SerializeField] private Image _btnFeedback;
    [SerializeField] private Image _btnRate;
    [SerializeField] private List<Text> _textContents;
    [SerializeField] private TextMeshProUGUI _textFeedback;
    [SerializeField] private TextMeshProUGUI _textRate;

    protected override void Start()
    {
        base.Start();
        _textContents[_textContents.Count - 1].text = "Build " + bundleVersionCode;
        CheckTheme();
        if (EffectController.instance.IsEffectOn)
        {
            effectBttOff.gameObject.SetActive(false);
        }
        else
        {
            effectBttOff.gameObject.SetActive(true);
        }

        if (NotificationController.instance.IsNotificationOn)
        {
            notiBttOff.gameObject.SetActive(false);
        }
        else
        {
            notiBttOff.gameObject.SetActive(true);
        }

        //gameMaster = GameObject.FindGameObjectWithTag("GameMaster");
        if (Sound.instance != null && Music.instance != null)
        {
            soundController = Sound.instance;
            musicController = Music.instance;
            ////ShowButtonSound(soundController.IsEnabled());
            ////ShowButtonMusic(musicController.IsEnabled());
            soundController.audioSource.volume = soundController.GetVolume();
            musicController.audioSource.volume = musicController.GetVolume();
            _sliderSound.value = soundController.audioSource.volume;
            _sliderMusic.value = musicController.audioSource.volume;
        }
        CheckLogin();
    }

    private void CheckTheme()
    {
        if(MainController.instance != null)
        {
            var currTheme = ThemesControl.instance.CurrTheme;
            _iconSound.sprite = currTheme.uiData.settingData.iconSound;
            _iconMusic.sprite = currTheme.uiData.settingData.iconMusic;
            _iconEffect.sprite = currTheme.uiData.settingData.iconEffect;
            _iconNotification.sprite = currTheme.uiData.settingData.iconNotification;

            _iconEffectOn.sprite = currTheme.uiData.settingData.iconOn;
            _iconEffectOff.sprite = currTheme.uiData.settingData.iconOff;
            _iconNotificationOn.sprite = currTheme.uiData.settingData.iconOn;
            _iconNotificationOff.sprite = currTheme.uiData.settingData.iconOff;

            _iconArrow.sprite = _iconArrow2.sprite = _iconArrow3.sprite = currTheme.uiData.settingData.iconArrow;
            _handleSound.sprite = _handleMusic.sprite = currTheme.uiData.settingData.handle;
            _frameMaskSound.sprite = _frameMaskMusic.sprite = currTheme.uiData.settingData.frameMask;
            _bgProgressSound.sprite = _bgProgressMusic.sprite = currTheme.uiData.settingData.bgProgress;
            _fillProgressSound.sprite = _fillProgressMusic.sprite = currTheme.uiData.settingData.fillProgress;
            _fillSound.sprite = _fillMusic.sprite = currTheme.uiData.settingData.fillProgress;

            _line.sprite = currTheme.uiData.settingData.line;
            _line2.sprite = currTheme.uiData.settingData.line2;
            _btnFeedback.sprite = currTheme.uiData.settingData.btnFeedback;
            _btnRate.sprite = currTheme.uiData.settingData.btnRate;

            _textFeedback.color = currTheme.uiData.settingData.colorTextBtnFeedback;
            _textRate.color = currTheme.uiData.settingData.colorTextBtnRate;

            foreach (var text in _textContents)
            {
                text.color = currTheme.fontData.colorContentDialog;
            }
        }
    }

    /*private void Update()
{
   if (soundController.IsMuted())
       soundButton.transform.Find("On").gameObject.SetActive(false);
   else
       soundButton.transform.Find("On").gameObject.SetActive(true);

   //music
   if (musicController.IsMuted())
       musicButton.transform.Find("On").gameObject.SetActive(false);
   else
       musicButton.transform.Find("On").gameObject.SetActive(true);
}*/

    private void ShowButtonMusic(bool status)
    {
        if (!status)
        {
            musicButton.transform.Find("On").gameObject.SetActive(false);
            musicButton.transform.Find("Off").gameObject.SetActive(true);
        }
        else
        {
            musicButton.transform.Find("Off").gameObject.SetActive(false);
            musicButton.transform.Find("On").gameObject.SetActive(true);
        }
    }

    private void ShowButtonSound(bool status)
    {
        if (!status)
        {
            soundButton.transform.Find("On").gameObject.SetActive(false);
            soundButton.transform.Find("Off").gameObject.SetActive(true);
        }
        else
        {
            soundButton.transform.Find("Off").gameObject.SetActive(false);
            soundButton.transform.Find("On").gameObject.SetActive(true);
        }
    }
    public void OnEffectClick()
    {
        if (!effectBttOff.gameObject.activeInHierarchy)
        {
            // effect off
            effectBttOff.gameObject.SetActive(true);
            EffectController.instance.IsEffectOn = false;
        }
        else
        {
            // effect on
            effectBttOff.gameObject.SetActive(false);
            EffectController.instance.IsEffectOn = true;
        }
    }
    public void OnNativeNotificationClick()
    {
        if (!notiBttOff.gameObject.activeInHierarchy)
        {
            // notification off
            notiBttOff.gameObject.SetActive(true);
            NotificationController.instance.IsNotificationOn = false;
        }
        else
        {
            // notification on
            notiBttOff.gameObject.SetActive(false);
            NotificationController.instance.IsNotificationOn = true;
        }

    }
    public void OnHowToPlayClick()
    {
        Sound.instance.Play(Sound.Others.PopupOpen);
        DialogController.instance.ShowDialog(DialogType.HowtoPlay, DialogShow.STACK_DONT_HIDEN);
    }

    public void OnSoundClick()
    {
        if (soundController)
        {
            ////bool _status = soundController.IsEnabled();
            ////soundController.SetEnabled(!_status);
            ////ShowButtonSound(soundController.IsEnabled());
            soundController.SetVolume(_sliderSound.value);
            soundController.audioSource.volume = soundController.GetVolume();
        }
    }
    public void OnMusicClick()
    {
        if (musicController)
        {
            ////bool _status = musicController.IsEnabled();
            ////musicController.SetEnabled(!_status, true);
            ////ShowButtonMusic(musicController.IsEnabled());
            musicController.SetVolume(_sliderMusic.value);
            musicController.audioSource.volume = musicController.GetVolume();
        }
    }

    public void OnClickLogout()
    {
        Sound.instance.Play(Sound.Others.PopupOpen);
        FacebookController.instance.Logout();
    }

    public void OnClickFeedBack()
    {
        Sound.instance.Play(Sound.Others.PopupOpen);
        DialogController.instance.ShowDialog(DialogType.ContactUsDialog, DialogShow.STACK_DONT_HIDEN);
    }

    public void OnClickRateUs()
    {
        Sound.instance.Play(Sound.Others.PopupOpen);
        CUtils.RateGame();
    }

    public void OnClickCommunity()
    {
        Sound.instance.Play(Sound.Others.PopupOpen);
        Application.OpenURL("https://www.facebook.com/wordgames.top");
    }

    public void OnClickExitGame()
    {
        Sound.instance.Play(Sound.Others.PopupOpen);
        _panelOverlay.SetActive(true);
        TweenControl.GetInstance().ScaleFromZero(_panelExit, 0.3f);
    }

    public void OnExitClick()
    {
        Sound.instance.Play(Sound.Others.PopupClose);
        Application.Quit();
    }

    public void OnNoExitClick()
    {
        Sound.instance.Play(Sound.Others.PopupClose);
        TweenControl.GetInstance().ScaleFromOne(_panelExit, 0.3f, () =>
        {
            _panelOverlay.SetActive(false);
        });
    }

    private void CheckLogin()
    {
        if (PlayFab.PlayFabClientAPI.IsClientLoggedIn())
        {
            _textNameUser.text = FacebookController.instance.user.name;
            _btnLogout.SetActive(true);
        }
        else
        {
            _textNameUser.text = "";
            _btnLogout.SetActive(false);
        }
    }
}


