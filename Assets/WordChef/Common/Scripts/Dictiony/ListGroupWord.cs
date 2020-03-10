using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListGroupWord : MonoBehaviour
{
    bool statusGroupWord = true;
    public void OnButtonClick()
    {
        transform.GetChild(1).gameObject.SetActive(statusGroupWord);
        statusGroupWord = !statusGroupWord;
    }
}
