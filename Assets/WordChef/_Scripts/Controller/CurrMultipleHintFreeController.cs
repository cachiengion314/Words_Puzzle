using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrMultipleHintFreeController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMultipleHintFree;

    void Start()
    {
        this.UpdateMultiplehintFree();
        CurrencyController.onMultipleHintFreeChanged += this.OnMultipleHintFreChanged;
    }

    private void UpdateMultiplehintFree()
    {
        if (_textMultipleHintFree != null)
            _textMultipleHintFree.text = CurrencyController.GetMultipleHintFree().ToString();
    }
    private void OnMultipleHintFreChanged()
    {
        this.UpdateMultiplehintFree();
    }

    private void OnDestroy()
    {
        CurrencyController.onHintFreeChanged -= this.OnMultipleHintFreChanged;
    }

}
