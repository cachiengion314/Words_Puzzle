using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconController : MonoBehaviour
{
    [SerializeField] private float _timeDelay;
    [SerializeField] private float _timeMove = 0.2f;

    void Awake()
    {
        transform.localScale = Vector3.zero;
    }
    void Start()
    {
        AnimIcon();
    }

    private void AnimIcon()
    {
        TweenControl.GetInstance().DelayCall(transform, _timeDelay, () =>
        {
            TweenControl.GetInstance().ScaleFromZero(gameObject, 0.3f, null, EaseType.InOutBack);
            TweenControl.GetInstance().MoveRectY(transform as RectTransform, 50, _timeMove, () =>
            {
                TweenControl.GetInstance().MoveRectY(transform as RectTransform, -50, _timeMove, () =>
                {
                    TweenControl.GetInstance().MoveRectY(transform as RectTransform, 20, _timeMove, () =>
                    {
                        TweenControl.GetInstance().MoveRectY(transform as RectTransform, 0, _timeMove, null, EaseType.Linear);
                    });
                });
            });
        });
    }
}
