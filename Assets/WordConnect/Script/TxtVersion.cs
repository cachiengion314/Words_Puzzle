using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TxtVersion : MonoBehaviour
{
    public Text mTxtText;

    // Start is called before the first frame update
    void Start()
    {
        mTxtText.text = Application.version;
    }
}
