using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListGroupWord : MonoBehaviour
{
    public Transform groupWord;
    bool statusGroupWord = true;
    public void OnButtonClick()
    {
        transform.GetChild(1).gameObject.SetActive(statusGroupWord);
        statusGroupWord = !statusGroupWord;
    }
}
