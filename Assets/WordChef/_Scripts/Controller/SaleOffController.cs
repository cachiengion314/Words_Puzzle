using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaleOffController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textTime;
    private bool _isWeeken;
    private long _numDay;
    private DateTime _dayEnd;
    void Start()
    {
        CheckWeeken();
    }

    private void CheckWeeken()
    {
        DayOfWeek today = DateTime.Now.DayOfWeek;
        if (today == DayOfWeek.Saturday || today == DayOfWeek.Sunday)
        {
            _isWeeken = true;
            _dayEnd = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
        }
        else
        {
            _isWeeken = false;
            _dayEnd = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Saturday);
        }
        StartCoroutine(UpdateTimeCountDown());
    }

    private IEnumerator UpdateTimeCountDown()
    {
        while (_isWeeken || !_isWeeken)
        {
            var result = _dayEnd - DateTime.Now;
            _numDay = (int)(result.TotalSeconds);
            TimeSpan time = TimeSpan.FromSeconds(_numDay);
            _textTime.text = (time.Days * 24f + time.Hours) + time.ToString(@"\:mm\:ss");
            yield return new WaitForSeconds(1);
            if (_numDay > 0)
                _numDay--;
        }
    }

    public void OnSaleOffClick()
    {
        Sound.instance.Play(Sound.Others.PopupOpen);
        if (_isWeeken)
        {
            DialogController.instance.ShowDialog(DialogType.WeekenSale, DialogShow.REPLACE_CURRENT);
        }
        else
        {
            DialogController.instance.ShowDialog(DialogType.LimitedSaleDialog, DialogShow.REPLACE_CURRENT);
        }
    }

    private void OnDestroy()
    {
        StopCoroutine(UpdateTimeCountDown());
    }
}
