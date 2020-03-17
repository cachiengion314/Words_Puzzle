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

    GameObject gameMaster;
    Sound soundController;
    Music musicController;

    [SerializeField] private GameObject _btnLogout;
    [SerializeField] private Text _textNameUser;
    [SerializeField] private GameObject _panelExit;

    protected override void Start()
    {
        base.Start();
        gameMaster = GameObject.FindGameObjectWithTag("GameMaster");
        if (gameMaster)
        {
            soundController = gameMaster.transform.Find("SoundController").GetComponent<Sound>();
            musicController = gameMaster.transform.Find("MusicController").GetComponent<Music>();
        }
        CheckLogin();
    }
    private void Update()
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
    }

    public override void OnHowToPlayClick()
    {
        Sound.instance.PlayButton();
        DialogController.instance.ShowDialog(DialogType.HowtoPlay, DialogShow.STACK);
    }

    public void OnSoundClick()
    {
        if (soundController)
        {
            bool _status = soundController.IsEnabled();
            soundController.SetEnabled(!_status);
        }
    }
    public void OnMusicClick()
    {
        if (musicController)
        {
            bool _status = musicController.IsEnabled();
            musicController.SetEnabled(!_status, true);
        }
    }

    public void OnClickLogout()
    {
        Sound.instance.PlayButton();
        FacebookController.instance.Logout();
    }

    public void OnClickExitGame()
    {
        Sound.instance.PlayButton();
        TweenControl.GetInstance().ScaleFromZero(_panelExit,0.3f);
    }

    public void OnExitClick()
    {
        Sound.instance.PlayButton();
        Application.Quit();
    }

    public void OnNoExitClick()
    {
        Sound.instance.PlayButton();
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


