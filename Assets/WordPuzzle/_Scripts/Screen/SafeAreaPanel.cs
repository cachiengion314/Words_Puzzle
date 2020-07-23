using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaPanel : MonoBehaviour
{
    public SafeAreaDetect areaDetect;
    [SerializeField] private RectTransform _rectTransform;

    private Rect _safeArea;

    void Awake()
    {
        _safeArea = Screen.safeArea;
        RefreshSafe(_safeArea);
    }

    public RectTransform ThisRect
    {
        get
        {
            return _rectTransform;
        }
    }
    //void Update()
    //{
    //if(_safeArea != Screen.safeArea)
    //    RefreshSafe(Screen.safeArea);
    //}

    private void RefreshSafe(Rect safeArea)
    {
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        _rectTransform.anchorMin = anchorMin;
        _rectTransform.anchorMax = anchorMax;
    }
}
