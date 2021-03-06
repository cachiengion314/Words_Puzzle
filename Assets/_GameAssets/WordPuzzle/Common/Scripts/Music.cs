﻿using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour
{
    public AudioSource audioSource;
    public enum Type { None, Menu, Main_0, Main_1, Main_2 };
    public static Music instance;

    [HideInInspector]
    public AudioClip[] musicClips;

    private Type currentType = Type.None;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public bool IsMuted()
    {
        return !IsEnabled();
    }

    public float GetVolume()
    {
        return CPlayerPrefs.GetFloat("music_volume", 1);
    }

    public void SetVolume(float value)
    {
        CPlayerPrefs.SetFloat("music_volume", value);
        UpdateSetting();
    }

    public bool IsEnabled()
    {
        return CPlayerPrefs.GetBool("music_enabled", true);
    }

    public void SetEnabled(bool enabled, bool updateMusic = false)
    {
        CPlayerPrefs.SetBool("music_enabled", enabled);
        if (updateMusic)
            UpdateSetting();
    }

    public void Play(Music.Type type)
    {
        if (type == Type.None) return;
        if (currentType != type || !audioSource.isPlaying)
        {
            StartCoroutine(PlayNewMusic(type));
        }
    }

    public void Play()
    {
        Play(currentType);
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    private IEnumerator PlayNewMusic(Music.Type type)
    {
        while (audioSource.volume >= 0.1f)
        {
            audioSource.volume -= 0.2f;
            yield return new WaitForSeconds(0.1f);
        }
        audioSource.Stop();
        currentType = type;
        audioSource.clip = musicClips[(int)type];
        if (IsEnabled())
        {
            audioSource.Play();
        }
        audioSource.volume = GetVolume();
    }

    private void UpdateSetting()
    {
        if (audioSource == null) return;
        if (IsEnabled())
            Play();
        else
            audioSource.Stop();
    }
}
