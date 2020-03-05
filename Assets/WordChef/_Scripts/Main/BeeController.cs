using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeeController : MonoBehaviour
{
    [SerializeField] private Image _beeImage;
    [SerializeField] private TextMeshProUGUI _textAmountBee;
    [Header("TEST")]
    [SerializeField] private int _number = 1;

    void Start()
    {
        UpdateAmountBee();
    }

    private void UpdateAmountBee()
    {
        _textAmountBee.text = BeeManager.instance.CurrBee.ToString();
    }

    public void OnBeeButtonClick()
    {
        BeeManager.instance.SetAmountBee(-1);
        UpdateAmountBee();
    }

    #region TEST BEE
    public void AddAmountBee()
    {
        BeeManager.instance.SetAmountBee(_number);
        UpdateAmountBee();
    }
    #endregion
}
