﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimEvent : MonoBehaviour
{
    public void EventAnimCallback()
    {
        if (MainController.instance != null)
        {
            Sound.instance.Play(Sound.Collects.LevelShow);
            TweenControl.GetInstance().DelayCall(transform, 0.2f, () =>
            {
                ScaleLetters();
            });
        }
    }

    public void SceneAnimComplete()
    {
        if (MainController.instance != null)
        {
            BlockScreen.instance.Block(false);
            MainController.instance.beeController.OnBeeButtonClick();
            if (GameState.currentLevel == 0 && GameState.currentSubWorld == 0 && GameState.currentWorld == 0)
            {
                //Timer.Schedule(this, 1f, () =>
                //{
                var isTut = CPlayerPrefs.GetBool("TUTORIAL", false);
                //DialogController.instance.ShowDialog(DialogType.HowtoPlay);
                if (!isTut)
                    TutorialController.instance.ShowPopWordTut(TutorialController.instance.contentManipulation);
                //});
            }
            TutorialController.instance.CheckAndShowTutorial();
        }
    }

    public void SceneLoaded()
    {
        SceneAnimate.Instance.animatorScene.gameObject.SetActive(false);
    }

    public void OnShowSceneCallback()
    {
        //if (HomeController.instance != null)
        //    HomeController.instance.PlayAnimTitle();
    }

    public void LevelClearCallback()
    {
        StartCoroutine(HidenLetters());
    }

    public void LevelClearAnimComplete()
    {
        MainController.instance.OnComplete();
    }

    public void PlayParticleCompliment()
    {
        WordRegion.instance.compliment.PlayParticle();
    }

    public void HidenParticleCompliment()
    {
        WordRegion.instance.compliment.Hidenarticle();
    }

    private IEnumerator HidenLetters()
    {
        if (Pan.instance != null && Pan.instance.LetterTexts.Count > 0)
        {
            for (int i = 0; i < Pan.instance.LetterTexts.Count; i++)
            {
                var letter = Pan.instance.LetterTexts[i];
                var canvasGroup = letter.GetComponent<CanvasGroup>();
                canvasGroup.alpha = 1;
                //letter.transform.localScale = Vector3.zero;
                Sound.instance.audioSource.Stop();
                Sound.instance.PlayButton(Sound.Button.Hint);
                TweenControl.GetInstance().Scale(letter.gameObject, Vector3.one * 1.2f, 0.3f, () =>
                {
                    TweenControl.GetInstance().FadeAnfa(canvasGroup, 0, 0.5f);
                    TweenControl.GetInstance().Scale(letter.gameObject, Vector3.zero, 0.5f, () =>
                    {
                        var fxEffect = Instantiate(WordRegion.instance.compliment.fxHidenLetter, letter.transform);
                    }, EaseType.InQuad);
                });
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    private void ScaleLetters()
    {
        if (Pan.instance != null && Pan.instance.LetterTexts.Count > 0)
        {
            for (int i = 0; i < Pan.instance.LetterTexts.Count; i++)
            {
                var letter = Pan.instance.LetterTexts[i];
                letter.transform.localScale = Vector3.zero;
                var canvasGroup = letter.GetComponent<CanvasGroup>();
                canvasGroup.alpha = 0;
                TweenControl.GetInstance().FadeAnfa(canvasGroup, 1, 0.3f);
                TweenControl.GetInstance().Scale(letter.gameObject, Vector3.one * 1.1f, 0.3f, () =>
                {
                    TweenControl.GetInstance().Scale(letter.gameObject, Vector3.one, 0.2f, null, EaseType.InQuad);
                });
            }
        }
    }
}
