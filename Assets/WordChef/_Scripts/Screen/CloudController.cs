using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CloudController : MonoBehaviour
{
    [SerializeField] private List<Image> _backgrounds;
    [SerializeField] private float _speed = -5f;

    private Vector3 valueStart;

    void Start()
    {
        valueStart = transform.localPosition;
    }

    void Update()
    {
        MoveBG();
    }

    private void MoveBG()
    {
        if (transform.localPosition.x <= -(_backgrounds[0].transform as RectTransform).sizeDelta.x)
            transform.localPosition = valueStart;
        transform.localPosition = new Vector3(transform.localPosition.x - Time.deltaTime * _speed, valueStart.y);
    }

    private void OrderBG()
    {
        var bgOrder = _backgrounds.OrderBy(bg => bg.transform.transform.GetComponent<RectTransform>().anchoredPosition.x);
        _backgrounds = bgOrder.ToList();
        var firstBgRect = _backgrounds[0].transform.GetComponent<RectTransform>();
        firstBgRect.transform.SetAsFirstSibling();
        firstBgRect.anchoredPosition =
            new Vector2(firstBgRect.sizeDelta.x * 2, firstBgRect.anchoredPosition.y);
    }
}

