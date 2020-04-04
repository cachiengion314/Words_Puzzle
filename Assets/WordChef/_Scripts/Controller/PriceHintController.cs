using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PriceHintController : MonoBehaviour
{
    [SerializeField] private bool _isMultipleHint;
    [SerializeField] private TextMeshProUGUI _textPrice;

    private void Start()
    {
        _textPrice.text = (_isMultipleHint ? Const.HINT_RANDOM_COST : Const.HINT_COST).ToString();
    }
}
