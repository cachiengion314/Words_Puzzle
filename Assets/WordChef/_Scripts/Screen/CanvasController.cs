using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    int _count;
    [SerializeField] private float _delayShow = 0.5f;
    [SerializeField] private Transform _panelTop;
    [SerializeField] private Transform _panelCenter;
    [SerializeField] private Transform _panelBottom;


    void Awake()
    {
        Init();
    }
    void Start()
    {
        CanvasActive();
    }

    private void Init()
    {
        if (_panelCenter != null)
            _panelCenter.transform.localScale = Vector3.zero;
        //_panelBottom.transform.localScale = Vector3.zero;
    }

    void CanvasActive()
    {
        TweenControl.GetInstance().DelayCall(transform, _delayShow, () =>
        {
            ShowPanelTop();
            TweenControl.GetInstance().DelayCall(transform, _delayShow / 2, () =>
            {
                ShowPanelCenter();
                ShowPanelBottom();
            });
        });
    }

    private void ShowPanelTop()
    {
        if (_panelTop != null)
            TweenControl.GetInstance().MoveRect(_panelTop as RectTransform, Vector3.zero, 0.5f, null, EaseType.OutBack);
    }

    private void ShowPanelCenter()
    {
        if (_panelCenter != null)
        {
            TweenControl.GetInstance().ScaleFromZero(_panelCenter.gameObject, 0.5f, null, EaseType.InOutQuad);
            TweenControl.GetInstance().MoveRect(_panelCenter as RectTransform, Vector3.zero, 0.5f, null, EaseType.OutBack);
        }
    }

    private void ShowPanelBottom()
    {
        //TweenControl.GetInstance().ScaleFromZero(_panelBottom.gameObject, 0.3f, null, EaseType.InOutQuad);
    }

    public void OnPressedBtn(GameObject obj)
    {
        obj.transform.localScale = Vector3.one * 0.95f;
    }

    public void OnNormalBtn(GameObject obj)
    {
        obj.transform.localScale = Vector3.one;
    }

}
