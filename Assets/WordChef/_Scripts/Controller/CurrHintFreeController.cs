using System.Collections;
using System.Collections.Generic;
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
