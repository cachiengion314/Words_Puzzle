﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _buttons;
    [SerializeField] private List<GameObject> _shadows;
    [SerializeField] private float _timeDelay;
    [SerializeField] private float _timeMove = 0.2f;

    public void AnimIcon()
    {
        for (int i = 0; i < _buttons.Count; i++)
            _buttons[i].transform.localScale = Vector3.zero;
        Sound.instance.Play(Sound.Scenes.HomeButton);
        StartCoroutine(ShowIcon());
    }

    private IEnumerator ShowIcon()
    {
        var tweenControl = TweenControl.GetInstance();
        yield return new WaitForSeconds(_timeDelay);
        for (int i = 0; i < _buttons.Count; i++)
        {
            var btn = _buttons[i];
            var shawdow = _shadows[i];
            tweenControl.DelayCall(shawdow.transform, 0.12f, () =>
            {
                tweenControl.ScaleFromZero(shawdow, 0.3f);
            });
            tweenControl.Scale(btn, Vector3.one,0.3f);
            tweenControl.MoveRectY(btn.transform as RectTransform, -55, 0.3f, () =>
            {
                tweenControl.Scale(shawdow, new Vector3(1.2f, 1.4f, 1.2f), 0.2f, () => {
                    tweenControl.Scale(shawdow, Vector3.one, 0.2f);
                });
                tweenControl.MoveRectY(btn.transform as RectTransform, -100, 0.2f, () =>
                {
                    tweenControl.MoveRectY(btn.transform as RectTransform, -73, 0.2f, () =>
                    {
                        tweenControl.MoveRectY(btn.transform as RectTransform, -87, 0.15f, () =>
                        {
                            tweenControl.MoveRectY(btn.transform as RectTransform, -85f, 0.15f, null, EaseType.Linear);
                        });
                    });
                });
            });
            yield return new WaitForSeconds(0.05f);
        }
    }
}
