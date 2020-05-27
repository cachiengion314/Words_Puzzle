using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrMultipleHintFreeController : MonoBehaviour
{
    void Start()
    {
        UpdateMultiplehintFree();
        CurrencyController.onMultipleHintFreeChanged += OnMultipleHintFreChanged;
    }

    private void UpdateMultiplehintFree()
    {
        if (gameObject.GetComponent<TextMeshProUGUI>() != null)
            gameObject.SetText(CurrencyController.GetMultipleHintFree().ToString());
    }
    private void OnMultipleHintFreChanged()
    {
        UpdateMultiplehintFree();
    }

    private void OnDestroy()
    {
        CurrencyController.onHintFreeChanged -= OnMultipleHintFreChanged;
    }

}
