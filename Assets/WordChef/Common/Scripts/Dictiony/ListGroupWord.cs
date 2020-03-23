using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListGroupWord : MonoBehaviour
{
    public Transform groupWord;
    bool statusGroupWord = true;
    public void OnButtonClick()
    {
        CloseAllGroupWord();
        statusGroupWord = !statusGroupWord;
        if (transform.GetChild(1).childCount > 0)
        {
            transform.GetChild(1).gameObject.SetActive(statusGroupWord);
        }
        else
        {
            transform.GetChild(2).gameObject.SetActive(statusGroupWord);
        }

    }

    private void CloseAllGroupWord()
    {
        for (int i = 0; i < DictionaryDialog.instance.groupWords.Count; i++)
        {
            var groupWord = DictionaryDialog.instance.groupWords[i];
            groupWord.transform.GetChild(1).gameObject.SetActive(false);
            groupWord.transform.GetChild(2).gameObject.SetActive(false);
            statusGroupWord = false;
        }
    }
}
