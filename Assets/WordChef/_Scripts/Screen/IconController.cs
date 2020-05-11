using System.Collections;
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
        yield return new WaitForSeconds(_timeDelay);
        for (int i = 0; i < _buttons.Count; i++)
        {
            var btn = _buttons[i];
            var shawdow = _shadows[i];
            TweenControl.GetInstance().ScaleFromZero(shawdow, 0.3f);
            TweenControl.GetInstance().ScaleFromZero(btn, 0.3f, null, EaseType.InOutBack);
            TweenControl.GetInstance().MoveRectY(btn.transform as RectTransform, -55, _timeMove, () =>
            {
                TweenControl.GetInstance().MoveRectY(btn.transform as RectTransform, -100, 0.2f, () =>
                {
                    TweenControl.GetInstance().MoveRectY(btn.transform as RectTransform, -73, 0.2f, () =>
                    {
                        TweenControl.GetInstance().MoveRectY(btn.transform as RectTransform, -87, 0.15f, () =>
                        {
                            TweenControl.GetInstance().MoveRectY(btn.transform as RectTransform, -85f, 0.15f, null, EaseType.Linear);
                        });
                    });
                });
            });
            yield return new WaitForSeconds(0.05f);
        }
    }
}
