﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWord : MyButton
{
   public DialogType dialogType;
   public DialogShow dialogShow; 
    public void GetData()
    {
        string text = transform.GetChild(0).GetComponent<Text>().text.ToString();

        if (!Dictionary.instance.CheckWExistInDictWordSaved(text))
        {
            Debug.Log("getDataApi");
            DictionaryDialog.instance.GetDataFromApi(text);
        }
        else
        {
            Debug.Log("word had in device");
            MeanDialog.wordName = text;
            MeanDialog.wordMean = Dictionary.dictWordSaved[text];

        }
        
    }

     public override void OnButtonClick()
    {
        GetData();
       
        TweenControl.GetInstance().DelayCall(transform, 0.2f, () =>
        {
            base.OnButtonClick();
            DialogController.instance.ShowDialog(dialogType, dialogShow);
        });
       
        
    }
}
