using UnityEngine;
using System.Collections;
using System;

public class Sound : MonoBehaviour
{
    public AudioSource audioSource, loopAudioSource;
    public enum Button { Default, Hint, MultipleHint, Shuffe, Beehive };
    public enum Others { Match, Win, PopupOpen, PopupClose, WordInvalid, WordAlready };
    public enum Collects { CoinCollect, CoinKeep, LevelClose, LevelOpen, LevelShow };
    public enum Scenes { CurtainClose, CurtainOpen, HomeButton, LevelClear, ChapterClear};

    //[HideInInspector]
    public AudioClip[] buttonClips;
    //[HideInInspector]
    public AudioClip[] otherClips;
    public AudioClip[] collectClips;
    public AudioClip[] sceneClips;

    public static Sound instance;

    [Header("SOUND FX NEW")]
    public AudioClip[] complimentSounds;
    public AudioClip[] lettersTouch;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateSetting();
    }

    public bool IsMuted()
    {
        return !IsEnabled();
    }

    public bool IsEnabled()
    {
        return CPlayerPrefs.GetBool("sound_enabled", true);
    }

    public void SetEnabled(bool enabled)
    {
        CPlayerPrefs.SetBool("sound_enabled", enabled);
        UpdateSetting();
    }

    public void Play(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void Play(AudioSource audioSource)
    {
        if (IsEnabled())
        {
            audioSource.Play();
        }
    }

    public void PlayButton(Button type = Button.Default)
    {
        int index = (int)type;
        audioSource.PlayOneShot(buttonClips[index]);
    }

    public void Play(Collects type, float volume = 1, Action onComplete = null)
    {
        int index = (int)type;
        audioSource.volume = volume;
        audioSource.PlayOneShot(collectClips[index]);
        TweenControl.GetInstance().DelayCall(transform, collectClips[index].length, () => {
            onComplete?.Invoke();
        });
    }

    public void Play(Scenes type, float volume = 1, Action onComplete = null)
    {
        int index = (int)type;
        audioSource.volume = volume;
        audioSource.PlayOneShot(sceneClips[index]);
        TweenControl.GetInstance().DelayCall(transform,sceneClips[index].length,()=> {
            onComplete?.Invoke();
        });
    }

    public void Play(Others type, float volume = 1, Action onComplete = null)
    {
        int index = (int)type;
        audioSource.volume = volume;
        audioSource.PlayOneShot(otherClips[index]);
        TweenControl.GetInstance().DelayCall(transform, otherClips[index].length, () => {
            onComplete?.Invoke();
        });
    }

    public void PlayLooping(Others type, float volume = 1, Action onComplete = null)
    {
        int index = (int)type;
        loopAudioSource.volume = volume;
        loopAudioSource.PlayOneShot(otherClips[index]);
        TweenControl.GetInstance().DelayCall(transform, otherClips[index].length, () => {
            onComplete?.Invoke();
        });
    }

    public void StopLooping()
    {
        loopAudioSource.Stop();
    }

    public void UpdateSetting()
    {
        audioSource.mute = IsMuted();
        loopAudioSource.mute = IsMuted();
    }
}