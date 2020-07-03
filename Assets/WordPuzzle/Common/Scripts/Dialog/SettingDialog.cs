using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SettingDialog : Dialog
{
    [SerializeField]
    GameObject soundButton;
    [SerializeField]
    GameObject musicButton;
    [SerializeField]
    GameObject notiButton;
    private GameObject notiBttOff;

    [SerializeField] private Slider _sliderSound;
    [SerializeField] private Slider _sliderMusic;

    GameObject gameMaster;
    Sound soundController;
    Music musicController;

    [SerializeField] private GameObject _btnLogout;
    [SerializeField] private Text _textNameUser;
    [SerializeField] private GameObject _panelExit;
    [SerializeField] private GameObject _panelOverlay;

    protected override void Start()
    {
        base.Start();

        notiBttOff = notiButton.transform.Find("Off").gameObject;
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


