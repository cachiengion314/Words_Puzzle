using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrHintBeeController : MonoBehaviour
{
    [SerializeField] private Text _textBeeAmount;

    private Image _objAmount;

    void Awake()
    {
        _objAmount = gameObject.GetComponentInParent<Image>();
    }

    void Start()
    {
        UpdatehintFree();
        BeeManager.instance.onBeeChanged += OnBeeChanged;
    }

    private void UpdatehintFree()
    {
        if (BeeManager.instance.CurrBee > 0)
        {
            _objAmount.gameObject.SetActive(true);
            if (_textBeeAmount != null)
            {
                _textBeeAmount.text = BeeManager.instance.CurrBee.ToString();
            }
            else
            {
                if (gameObject.GetComponent<TextMeshProUGUI>() != null)
                    gameObject.SetText(BeeManager.instance.CurrBee.ToString());
            }
        }
        else
        {
            _objAmount.gameObject.SetActive(false);
        }
    }
    private void OnBeeChanged()
    {
        UpdatehintFree();
    }

    private void OnDestroy()
    {
        BeeManager.instance.onBeeChanged -= OnBeeChanged;
    }
}
