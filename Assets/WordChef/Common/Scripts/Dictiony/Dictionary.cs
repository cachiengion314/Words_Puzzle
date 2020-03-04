using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;
using UnityEngine;

public class Dictionary : MonoBehaviour
{
    public void GetData()
    {
        //Debug.Log("hdasdasd");
        var client = new WebClient();
        var text = client.DownloadString("https://slack.com/intl/en-vn/");
        Debug.Log(text.ToString());
    }
}
