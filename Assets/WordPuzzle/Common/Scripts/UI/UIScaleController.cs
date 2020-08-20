using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public struct ScaleAndUIElement
{
    public float scaleValue;
    public GameObject uiElement;
}
public class UIScaleController : MonoBehaviour
{
    public static UIScaleController instance;

    public GameObject rootUI;
    public Vector3 rootUIOriginPos;
    public Vector3 rootUINewPos;

    public Vector2 originSize;
    public Vector2 newSize;
    [SerializeField] private RectTransform rectRoot;   // this object will be destroyed when load new scene
    private void Awake()
    {
        instance = this;
        SceneManager.sceneLoaded += OnSceneWasLoaded;
    }
    public void BannerShowAndScaleEvent(float bannerHeight = 0) // invoke in request and load banner
    {
        float bannerScale = bannerHeight / Screen.safeArea.height;
        float distance = bannerScale * WordRegion.instance.RectCanvas.rect.height;
        newSize = new Vector2(originSize.x, originSize.y - distance);
        rectRoot.sizeDelta = newSize;

        Pan.instance.ReloadLetterPositionPoints();
    }
    private void OnSceneWasLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 3)
        {
            rootUI = RootController.instance.gameObject;
            rectRoot = rootUI.GetComponent<RectTransform>();
            originSize = rectRoot.sizeDelta;

            BannerShowAndScaleEvent(AdmobController.instance.bannerHeight);
        }
    }
}
