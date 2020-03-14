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
        //UpdateAmountBee();
        OnBeeButtonClick();
    }

    private void UpdateAmountBee()
    {
        _textAmountBee.text = BeeManager.instance.CurrBee.ToString();
    }

    public void OnBeeButtonClick()
    {
        if (BeeManager.instance.CurrBee > 3)
        {
            if (WordRegion.instance.IsUseBee())
                return;
            WordRegion.instance.BeeClick();
            UpdateAmountBee();
        }
        //else
        //{
        //    DialogController.instance.ShowDialog(DialogType.Bee);
        //}
    }

    #region TEST BEE
    public void AddAmountBee()
    {
        BeeManager.instance.SetAmountBee(_number);
        UpdateAmountBee();
    }
    #endregion
}
