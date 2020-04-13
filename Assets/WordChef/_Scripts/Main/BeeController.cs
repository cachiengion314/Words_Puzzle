using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        //OnBeeButtonClick();
    }

    private void UpdateAmountBee()
    {
        _textAmountBee.text = BeeManager.instance.CurrBee.ToString();
    }

    public void OnBeeButtonClick()
    {
        var canUseBee = WordRegion.instance.Lines.Any(line => line.cells.Count > 3);
        var isUsed = WordRegion.instance.Lines.Any(line => line.usedBee);
        if (BeeManager.instance.CurrBee > 0 && canUseBee && !isUsed)
        {
            MainController.instance.isBeePlay = true;
            if (WordRegion.instance.IsUseBee())
                return;
            WordRegion.instance.BeeClick();
            UpdateAmountBee();
        }
        else
        {
            MainController.instance.isBeePlay = false;
            //DialogController.instance.ShowDialog(DialogType.Bee);
        }
    }

    #region TEST BEE
    public void AddAmountBee()
    {
        BeeManager.instance.SetAmountBee(_number);
        UpdateAmountBee();
    }
    #endregion
}
