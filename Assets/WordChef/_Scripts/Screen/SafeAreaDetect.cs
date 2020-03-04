using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaDetect : MonoBehaviour
{
    public static Action<Rect> onSafeAreaCallback;

    private Rect _safeArea;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _safeArea = Screen.safeArea;
    }

    void Update()
    {
        if (_safeArea != Screen.safeArea)
        {
            _safeArea = Screen.safeArea;
            onSafeAreaCallback?.Invoke(_safeArea);
        }
    }
}
