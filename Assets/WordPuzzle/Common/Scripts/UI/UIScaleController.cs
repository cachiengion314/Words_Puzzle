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
 
    public Button btnHintTarget;
    public Button btnHint;
    public Button btnMultipleHint;
    public Button btnShuffle;
    public Button btnRewardAds;
    public Button btnBonusBox;
    public Button btnHelp;

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
    public void BannerShowAndScaleEvent() // invoke in request and load banner
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        float bannerScale = AdmobController.instance.bannerHeight / Screen.height;
        newSize = new Vector2(originSize.x, originSize.y - bannerScale * Screen.height);
        rectRoot.sizeDelta = newSize;

        Pan.instance.ReloadLetterPositionPoints();
#endif
#if UNITY_EDITOR && ! !UNITY_ANDROID
     
        newSize = new Vector2(originSize.x, originSize.y - 112f);
        rectRoot.sizeDelta = newSize;

        Pan.instance.ReloadLetterPositionPoints();
#endif
    }
    private void OnSceneWasLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 3)
        {
            rootUI = RootController.instance.gameObject;
            rectRoot = rootUI.GetComponent<RectTransform>();
            originSize = rectRoot.sizeDelta;
         
            if (AdmobController.instance.bannerHeight > 0)
            {
                BannerShowAndScaleEvent();
            }
        }
    }
    private void ArrangeUIElementWithScaleValue(float newScaleValue, GameObject uiElement, Camera mainCamera)
    {
        Vector3 uiElementScreenPoint = mainCamera.WorldToScreenPoint(uiElement.transform.position);
        uiElementScreenPoint = new Vector3(uiElementScreenPoint.x, newScaleValue * Screen.height, uiElementScreenPoint.z);
        Vector3 uiElementWorldPoint = mainCamera.ScreenToWorldPoint(uiElementScreenPoint);
        uiElement.transform.position = uiElementWorldPoint;
    }
    private float GetCurrentScaleOfUIElement(GameObject uiElement, Camera mainCamera)
    {
        Vector3 uiElementScreenPoint = mainCamera.WorldToScreenPoint(uiElement.transform.position);
        float scaleValue = uiElementScreenPoint.y / Screen.height;
        return scaleValue;
    }
    private List<ScaleAndUIElement> SortList(List<GameObject> uiElementList, Camera mainCamera)
    {
        List<ScaleAndUIElement> scaleUIElementPair = new List<ScaleAndUIElement>();
        for (int i = 0; i < uiElementList.Count; i++)
        {
            float scaleValue = GetCurrentScaleOfUIElement(uiElementList[i], mainCamera);
            scaleUIElementPair.Add(new ScaleAndUIElement() { scaleValue = scaleValue, uiElement = uiElementList[i] });
        }
        scaleUIElementPair.Sort(
            delegate (ScaleAndUIElement a, ScaleAndUIElement b)
            {
                return a.scaleValue.CompareTo(b.scaleValue);
            }
            );
        return scaleUIElementPair;
    }
}
