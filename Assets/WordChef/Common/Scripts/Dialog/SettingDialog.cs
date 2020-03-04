﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    protected override void Start()
    {
        base.Start();
        gameMaster = GameObject.FindGameObjectWithTag("GameMaster");
        if (gameMaster)
        {
            soundController = gameMaster.transform.Find("SoundController").GetComponent<Sound>();
            musicController = gameMaster.transform.Find("MusicController").GetComponent<Music>();
        }
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
}


