using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrHintFreeController : MonoBehaviour
{
    void Start()
    {
        UpdatehintFree();
        CurrencyController.onHintFreeChanged += OnHintFreChanged;
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

    private void OnDestroy()
    {
        CurrencyController.onHintFreeChanged -= OnHintFreChanged;
    }
}
