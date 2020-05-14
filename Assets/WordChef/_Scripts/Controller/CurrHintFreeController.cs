using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrHintFreeController : MonoBehaviour
{
    public bool isMultipleHints;

    void Start()
    {
        if (!isMultipleHints)
            UpdatehintFree();
        else
            UpdateMultiplehintFree();
        CurrencyController.onHintFreeChanged += OnHintFreChanged;
        CurrencyController.onMultipleHintFreeChanged += OnMultipleHintFreChanged;
    }

    private void UpdatehintFree()
    {
        if (gameObject.GetComponent<TextMeshProUGUI>() != null)
            gameObject.SetText(CurrencyController.GetHintFree().ToString());
    }

    private void OnHintFreChanged()
    {
        UpdatehintFree();
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
        CurrencyController.onHintFreeChanged -= OnHintFreChanged;
        CurrencyController.onHintFreeChanged -= OnMultipleHintFreChanged;
    }

}
