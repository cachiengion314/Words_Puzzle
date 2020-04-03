using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ListGroupWord : MonoBehaviour
{
    public Transform groupWord;
    public TextMeshProUGUI firstButtonText;
    public TextMeshProUGUI numberWordText;
    [HideInInspector] public bool statusGroupWord = false;

    private void Start()
    {
        statusGroupWord = false;
    }

    public void OnButtonClick()
    {
        CloseAllGroupWord();
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
            if (groupWord != this)
            {
                groupWord.transform.GetChild(1).gameObject.SetActive(false);
                groupWord.transform.GetChild(2).gameObject.SetActive(false);
                groupWord.statusGroupWord = false;
            }
        }
        statusGroupWord = !statusGroupWord;
    }
}
