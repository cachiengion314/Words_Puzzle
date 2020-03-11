using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListGroupWord : MonoBehaviour
{
    public Transform groupWord;
    bool statusGroupWord = true;
    public void OnButtonClick()
    {
        if (transform.GetChild(1).childCount > 0)
        {
            transform.GetChild(1).gameObject.SetActive(statusGroupWord);
            statusGroupWord = !statusGroupWord;
        }
        else
        {
            transform.GetChild(2).gameObject.SetActive(statusGroupWord);
            statusGroupWord = !statusGroupWord;

        }

    }
}
