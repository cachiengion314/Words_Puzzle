using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static CUtils;

public class CurrHoneyPointController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textHoney;

    void Start()
    {
        if (!CPlayerPrefs.GetBool("HONEY_TUTORIAL", false))
            transform.parent.gameObject.SetActive(false);
        UpdateHoneyPoint();
        if (HoneyPointsController.instance != null)
            HoneyPointsController.instance.onChangedHoneyPoints += OnChangeHoneyPoint;
    }

    void OnChangeHoneyPoint()
    {
        UpdateHoneyPoint();
    }

    private void UpdateHoneyPoint()
    {
        _textHoney.text = AbbrevationUtility.AbbreviateNumber(FacebookController.instance.HoneyPoints);
    }

    private void OnDestroy()
    {
        if (HoneyPointsController.instance != null)
            HoneyPointsController.instance.onChangedHoneyPoints -= OnChangeHoneyPoint;
    }
}
