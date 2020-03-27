using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _buttons;
    [SerializeField] private float _timeDelay;
    [SerializeField] private float _timeMove = 0.2f;

    void Awake()
    {
        for (int i = 0; i < _buttons.Count; i++)
            _buttons[i].transform.localScale = Vector3.zero;
    }
    void Start()
    {
        AnimIcon();
    }

    private void AnimIcon()
    {
        Sound.instance.Play(Sound.Scenes.HomeButton);
        TweenControl.GetInstance().DelayCall(transform, _timeDelay, () =>
        {
            StartCoroutine(ShowIcon());
        });
    }

    private IEnumerator ShowIcon()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            var btn = _buttons[i];
            yield return new WaitForSeconds(0.1f);
            TweenControl.GetInstance().ScaleFromZero(btn, 0.3f, null, EaseType.InOutBack);
            TweenControl.GetInstance().MoveRectY(btn.transform as RectTransform, -50, _timeMove, () =>
            {
                TweenControl.GetInstance().MoveRectY(btn.transform as RectTransform, -93, _timeMove, () =>
                {
                    TweenControl.GetInstance().MoveRectY(btn.transform as RectTransform, -73, _timeMove, () =>
                    {
                        TweenControl.GetInstance().MoveRectY(btn.transform as RectTransform, -87, _timeMove, () =>
                        {
                            TweenControl.GetInstance().MoveRectY(btn.transform as RectTransform, -85f, _timeMove, null, EaseType.Linear);
                        });
                    });
                });
            });
        }
    }
}
