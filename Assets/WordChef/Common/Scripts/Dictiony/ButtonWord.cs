using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWord : MyButton
{
   public DialogType dialogType;
   public DialogShow dialogShow;

   private string text;
   public void GetData()
   {
        text = transform.GetChild(0).GetComponent<Text>().text.ToString();
        if (!Dictionary.instance.CheckWExistInDictWordSaved(text))
        {
            Debug.Log("getDataApi");
            Invoke("WaitTimeGetData", 0.6f);
        }
        else
        {
            Debug.Log("word had in device");
            DictionaryDialog.instance.SetTextMeanDialog(text, Dictionary.instance.dictWordSaved[text]);
            //MeanDialog.wordName = text;
            //MeanDialog.wordMean = Dictionary.instance.dictWordSaved[text];
        }
   }

     public override void OnButtonClick()
    {
        DictionaryDialog.instance.ShowMeanDialog();
        GetData();
        base.OnButtonClick();
        //DialogController.instance.ShowDialog(dialogType, dialogShow);
    }

     void WaitTimeGetData()
     {
         Dictionary.instance.GetDataFromApi(text);
         DictionaryDialog.instance.SetTextMeanDialog(text, Dictionary.instance.dictWordSaved[text]);
     }
}
