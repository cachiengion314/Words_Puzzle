using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class CExtension
{
    public static void SetText(this GameObject obj, string value)
    {
        TextMeshProUGUI text = obj.GetComponent<TextMeshProUGUI>();
        if (text != null)
        {
            text.text = value;
        }
    }

    public static void SetText(this TextMeshProUGUI objText, string value)
    {
        objText.text = value;
    }

    public static void SetTimeText(this TextMeshProUGUI text, String preFix, int time)
    {
        TimeSpan t = TimeSpan.FromSeconds(time);
        text.text = preFix + string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
    }
}
