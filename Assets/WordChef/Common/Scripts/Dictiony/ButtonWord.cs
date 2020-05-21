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
        DictionaryDialog.instance.ShowMeanDialog();

        if (!Dictionary.instance.CheckWExistInDictWordSaved(text))
        {
            Invoke("WaitTimeGetData", 0.6f);
        }
        else
        {
            DictionaryDialog.instance.SetTextMeanDialog(text, Dictionary.instance.dictWordSaved[text]);
            DictionaryDialog.instance.noInternet.SetActive(false);
            //MeanDialog.wordName = text;
            //MeanDialog.wordMean = Dictionary.instance.dictWordSaved[text];
        }
   }

     public override void OnButtonClick()
    {
        GetData();
        base.OnButtonClick();
        //DialogController.instance.ShowDialog(dialogType, dialogShow);
    }

     void WaitTimeGetData()
     {
        CUtils.CheckConnection(this, (result) =>
        {
            if (result == 0)
            {
                DictionaryDialog.instance.SetTextMeanDialog(text, "Loading...");
                Dictionary.instance.GetDataFromApi(text.ToLower());
                DictionaryDialog.instance.noInternet.SetActive(false);
            }
            else
            {
                DictionaryDialog.instance.SetTextMeanDialog(text, "");
                DictionaryDialog.instance.noInternet.SetActive(true);
            }
        });
        
         //DictionaryDialog.instance.SetTextMeanDialog(text, Dictionary.instance.dictWordSaved[text]);
     }
}
