using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cell : MonoBehaviour
{
    public TextMeshProUGUI letterText;
    public Image bg;
    public Image iconCoin;
    public string letter;
    public bool isShown;
    public bool isShownHint;

    private Vector3 originLetterScale;

    public void Animate()
    {
        Vector3 beginPosition = TextPreview.instance.transform.position;
        originLetterScale = letterText.transform.localScale;
        Vector3 middlePoint = CUtils.GetMiddlePoint(beginPosition, transform.position, -0.3f);
        Vector3[] waypoint = { beginPosition, middlePoint, transform.position };

        ShowText();
        letterText.transform.position = beginPosition;
        letterText.transform.localScale = TextPreview.instance.textGrid.transform.localScale;
        letterText.transform.SetParent(MonoUtils.instance.textFlyTransform);
        iTween.MoveTo(letterText.gameObject, iTween.Hash("path", waypoint, "time", 0.2f, "oncomplete", "OnMoveToComplete", "oncompletetarget", gameObject));
        iTween.ScaleTo(letterText.gameObject, iTween.Hash("scale", originLetterScale, "time", 0.2f));
    }

    private void OnMoveToComplete()
    {
        letterText.transform.SetParent(transform);
        iTween.ScaleTo(letterText.gameObject, iTween.Hash("scale", originLetterScale * 1.3f, "time", 0.15f, "oncomplete", "OnScaleUpComplete", "oncompletetarget", gameObject));
    }

    private void OnScaleUpComplete()
    {
        iTween.ScaleTo(letterText.gameObject, iTween.Hash("scale", originLetterScale, "time", 0.15f));
    }

    public void ShowHint()
    {
        isShown = true;
        isShownHint = true;
        originLetterScale = letterText.transform.localScale;
        ShowText();
        bg.color = new Color(1, 1, 1, 0.5f);
        OnMoveToComplete();
    }

    public void ShowText()
    {
        letterText.text = letter;
        bg.color = new Color(1, 1, 1, 1);
        bg.gameObject.SetActive(true);
    }
}
