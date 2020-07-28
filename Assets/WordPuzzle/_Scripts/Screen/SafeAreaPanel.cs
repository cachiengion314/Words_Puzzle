using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaPanel : MonoBehaviour
{
    public SafeAreaDetect areaDetect;
    [SerializeField] private RectTransform _rectTransform;

    private Rect _safeArea = new Rect(0, 0, 0, 0);

    void Awake()
    {
        var safeArea = Screen.safeArea;
        if (safeArea != _safeArea)
            RefreshSafe(safeArea);
    }

    public RectTransform ThisRect
    {
        get
        {
            return _rectTransform;
        }
    }

    private void RefreshSafe(Rect safeArea)
    {
        _safeArea = safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = anchorMin + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        _rectTransform.anchorMin = anchorMin;
        _rectTransform.anchorMax = anchorMax;
    }
}
