using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    public void EventAnimCallback()
    {
        if (Pan.instance != null)
        {
            Sound.instance.Play(Sound.Collects.LevelShow);
            ScaleLetters();
        }
    }

    public void SceneLoaded()
    {
        SceneAnimate.Instance.animatorScene.gameObject.SetActive(false);
    }

    public void LevelClearCallback()
    {

    }

    private void ScaleLetters()
    {
        if (Pan.instance != null && Pan.instance.LetterTexts.Count > 0)
        {
            for (int i = 0; i < Pan.instance.LetterTexts.Count; i++)
            {
                var letter = Pan.instance.LetterTexts[i];
                //letter.transform.localScale = Vector3.zero;
                TweenControl.GetInstance().Scale(letter.gameObject,Vector3.one * 1.1f, 0.3f,()=> {
                    TweenControl.GetInstance().Scale(letter.gameObject, Vector3.one, 0.2f, null,EaseType.InQuad);
                });
            }
        }
    }
}
