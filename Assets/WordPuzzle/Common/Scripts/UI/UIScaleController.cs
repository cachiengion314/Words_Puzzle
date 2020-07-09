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
    private readonly float deltaScale = 0.06f;

    List<GameObject> uiElementList = new List<GameObject>();
    List<ScaleAndUIElement> scaleAndUIElemenOrigintList = new List<ScaleAndUIElement>();
    List<ScaleAndUIElement> scaleAndUIElementAfterSortList = new List<ScaleAndUIElement>();

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
    private void Awake()
    {
        instance = this;
        SceneManager.sceneLoaded += OnSceneWasLoaded;
    }
    public void BannerShowAndScaleEvent() // invoke in request and load banner
    {
        //if (scaleAndUIElementAfterSortList.Count < 1) return;

        //Camera mainCamera = Camera.main;

        //float bannerScale = AdmobController.instance.bannerHeight / Screen.height;

        //for (int i = 0; i < scaleAndUIElementAfterSortList.Count; i++)
        //{
        //    ArrangeUIElementWithScaleValue(scaleAndUIElementAfterSortList[i].scaleValue + bannerScale,
        //        scaleAndUIElementAfterSortList[i].uiElement,
        //        mainCamera);
        //}
        float bannerScale = AdmobController.instance.bannerHeight / Screen.height;
       
        //rootUINewPos = new Vector3(rootUIOriginPos.x, rootUIOriginPos.y + bannerScale * Screen.height, rootUIOriginPos.z);

        var rectRoot = rootUI.GetComponent<RectTransform>();
        newSize = new Vector2(originSize.x, originSize.y - bannerScale * Screen.height);
        rectRoot.sizeDelta = newSize;
        //rootUI.transform.localPosition = rootUINewPos;
        Pan.instance.ReloadLetterPositionPoints();
    }
    public void BannerHideAndScaleEvent()
    {
        //if (scaleAndUIElementAfterSortList.Count < 1) return;

        //Camera mainCamera = Camera.main;

        //for (int i = 0; i < scaleAndUIElementAfterSortList.Count; i++)
        //{
        //    ArrangeUIElementWithScaleValue(scaleAndUIElementAfterSortList[i].scaleValue,
        //        scaleAndUIElementAfterSortList[i].uiElement,
        //        mainCamera);
        //}
        //rootUI.transform.localPosition = rootUIOriginPos;

        var rectRoot = rootUI.GetComponent<RectTransform>();       
        rectRoot.sizeDelta = originSize;
        Pan.instance.ReloadLetterPositionPoints();
    }
    private void OnSceneWasLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 3)
        {
            //Camera mainCamera = Camera.main;
            //uiElementList.Clear();
            //scaleAndUIElementAfterSortList.Clear();

            //btnHint = WordRegion.instance.btnHint;
            //btnHintTarget = WordRegion.instance.btnHintTarget;
            //btnMultipleHint = WordRegion.instance.btnMultipleHint;
            //btnShuffle = WordRegion.instance.btnShuffle;
            //btnRewardAds = WordRegion.instance.btnRewardAds;
            //btnBonusBox = WordRegion.instance.btnBonusBox;
            //btnHelp = WordRegion.instance.btnHelp;

            //uiElementList.Add(btnHintTarget.gameObject);
            //uiElementList.Add(btnHint.gameObject);
            //uiElementList.Add(btnMultipleHint.gameObject);
            //uiElementList.Add(btnShuffle.gameObject);
            //uiElementList.Add(btnRewardAds.gameObject);
            //uiElementList.Add(btnBonusBox.gameObject);
            //uiElementList.Add(btnHelp.gameObject);

            //scaleAndUIElemenOrigintList = SortList(uiElementList, mainCamera);

            //for (int i = 0; i < scaleAndUIElemenOrigintList.Count; i++)
            //{
            //    float newScaleValue = scaleAndUIElemenOrigintList[i].scaleValue - deltaScale;
            //    ArrangeUIElementWithScaleValue(newScaleValue, scaleAndUIElemenOrigintList[i].uiElement, mainCamera);
            //    scaleAndUIElementAfterSortList.Add(new ScaleAndUIElement()
            //    {
            //        scaleValue = newScaleValue,
            //        uiElement = scaleAndUIElemenOrigintList[i].uiElement
            //    });
            //}
            rootUI = RootController.instance.gameObject;
            //rootUIOriginPos = rootUI.transform.localPosition;
            var rectRoot = rootUI.GetComponent<RectTransform>();
            originSize = rectRoot.sizeDelta;

            if (AdmobController.instance.bannerHeight > 0)
            {              
                rectRoot.sizeDelta = newSize;
                //rootUI.transform.localPosition = rootUINewPos;
                Pan.instance.ReloadLetterPositionPoints();
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
