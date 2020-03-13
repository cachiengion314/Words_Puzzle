using System.Collections;
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
        Debug.Log(text);
        DictionaryDialog.instance.GetDataFromApi(text);
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
