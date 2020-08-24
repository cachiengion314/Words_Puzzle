using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class IngameNotify : MonoBehaviour
{
    public TextMeshProUGUI containText;
    public TextMeshProUGUI tittleText;

    public void CreateNotify(string title, string contain)
    {
        containText.text = contain;
        tittleText.text = title;
        NotifyMailDialogData.instance.CreatePlayerPrefsNotify(contain);
    }
}
