using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Common;

public class MailBtt : MonoBehaviour
{
    RectTransform thisRectTransf;
    Vector3 pos = new Vector3(-55.8f, -32.5f, 0);
    private void Start()
    {
        thisRectTransf = GetComponent<RectTransform>();
        RectTransform tinyRedNotify = Instantiate(NotifyMailDialogData.instance.tinyRedNotifyPrefb);
        tinyRedNotify.SetParent(thisRectTransf);
        tinyRedNotify.localScale = Vector3.one * .5f;
        tinyRedNotify.localPosition = pos;

        NotifyMailDialogData.MailBttAction = () => { tinyRedNotify.SetActive(false); };
        tinyRedNotify.SetActive(false);
        if (NotifyMailDialogData.instance.IsShowBefore == false)
        {
            tinyRedNotify.gameObject.SetActive(true);
        }
    }
}
