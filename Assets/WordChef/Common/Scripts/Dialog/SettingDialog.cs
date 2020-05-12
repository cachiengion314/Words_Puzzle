using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingDialog : PauseDialog
{   
    [SerializeField]
    GameObject soundButton;
    [SerializeField]
    GameObject musicButton;
    [SerializeField]
    GameObject notiButton;
    [SerializeField]private Slider _sliderSound;
    [SerializeField]private Slider _sliderMusic;

    GameObject gameMaster;
    Sound soundController;
    Music musicController;

    [SerializeField] private GameObject _btnLogout;
    [SerializeField] private Text _textNameUser;
    [SerializeField] private GameObject _panelExit;

    protected override void Start()
    {
        base.Start();
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

    public override void OnHowToPlayClick()
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
        Sound.instance.Play(Sound.Others.PopupClose);
        FacebookController.instance.Logout();
    }

    public void OnClickExitGame()
    {
        Sound.instance.Play(Sound.Others.PopupOpen);
        TweenControl.GetInstance().ScaleFromZero(_panelExit,0.3f);
    }

    public void OnExitClick()
    {
        Sound.instance.Play(Sound.Others.PopupClose);
        Application.Quit();
    }

    public void OnNoExitClick()
    {
        Sound.instance.Play(Sound.Others.PopupClose);
        TweenControl.GetInstance().ScaleFromOne(_panelExit, 0.3f);
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


