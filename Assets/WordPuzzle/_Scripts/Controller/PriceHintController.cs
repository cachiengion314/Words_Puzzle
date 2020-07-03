using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PriceHintController : MonoBehaviour
{
    [SerializeField] private bool _isMultipleHint;
    [SerializeField] private bool _isSelectedHint;
    [SerializeField] private TextMeshProUGUI _textPrice;

    private void Start()
    {
        _textPrice.text = (_isMultipleHint ? Const.HINT_RANDOM_COST : Const.HINT_COST).ToString();
        if (_isSelectedHint)
            _textPrice.text = Const.HINT_TARGET_COST.ToString();
    }
}
