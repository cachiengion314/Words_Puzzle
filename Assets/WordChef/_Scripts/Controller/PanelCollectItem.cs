﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelCollectItem : MonoBehaviour
{
    [SerializeField] private int _priceAgain = 100;
    [SerializeField] private Image _imageItem;
    [SerializeField] private TextMeshProUGUI _textItem;
    [SerializeField] private TextMeshProUGUI _textPrice;
    public void ShowItemCollect(Sprite sprite, int value)
    {
        _textPrice.text = _priceAgain.ToString();
        if (sprite != null)
        {
            _imageItem.sprite = sprite;
            _imageItem.SetNativeSize();
        }
        _textItem.text = value.ToString();
    }

}