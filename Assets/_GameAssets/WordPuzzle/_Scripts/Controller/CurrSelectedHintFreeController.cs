using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrSelectedHintFreeController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textSelectedHintFree;

    void Start()
    {
        this.UpdateSelectedhintFree();
        CurrencyController.onSelectedHintFreeChanged += this.OnSelectedHintFreChanged;
    }

    private void UpdateSelectedhintFree()
    {
        if (_textSelectedHintFree != null)
            _textSelectedHintFree.text = CurrencyController.GetSelectedHintFree().ToString();
    }
    private void OnSelectedHintFreChanged()
    {
        this.UpdateSelectedhintFree();
    }

    private void OnDestroy()
    {
        CurrencyController.onSelectedHintFreeChanged -= this.OnSelectedHintFreChanged;
    }

}
