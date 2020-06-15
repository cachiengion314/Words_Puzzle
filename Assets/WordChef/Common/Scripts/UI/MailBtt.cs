using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Common;

public class MailBtt : MonoBehaviour
{
    RectTransform thisRectTransf;
    Vector3 pos;
    private void Start()
    {
        pos = transform.Find("Pos").localPosition;

        if (NotifyMailDialogData.instance.IsShowBefore)
        {
            gameObject.SetActive(false);
        }

        thisRectTransf = GetComponent<RectTransform>();
        RectTransform tinyRedNotify = Instantiate(NotifyMailDialogData.instance.tinyRedNotifyPrefb);
        tinyRedNotify.SetParent(thisRectTransf);
        tinyRedNotify.localScale = Vector3.one * .5f;
        tinyRedNotify.localPosition = pos;

        NotifyMailDialogData.MailBttAction = () => { gameObject.SetActive(false); };
        tinyRedNotify.SetActive(false);
        if (NotifyMailDialogData.instance.IsShowBefore == false)
        {
            tinyRedNotify.gameObject.SetActive(true);
        }
    }
}
