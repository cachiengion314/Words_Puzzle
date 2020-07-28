using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//[ExecuteInEditMode]
public class ExecuteInEdit : MonoBehaviour
{
    public FlagTabController FlagTabController; 
    void Awake()
    {
        //string path = Application.persistentDataPath + "/FlagItemDic.json";
        //string jsonData = File.ReadAllText(path);
        //Dictionary<string, string> flagItemDic = new Dictionary<string, string>();
        //flagItemDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonData);

        //List<FlagItem> tempList = new List<FlagItem>();
        //foreach (var pair in flagItemDic)
        //{
        //    FlagItem flagItem = new FlagItem();
        //    flagItem.flagName = pair.Key;
        //    flagItem.flagUnlockWord = pair.Value;
        //    tempList.Add(flagItem);
        //}
        //FlagTabController.flagItemList.Clear();
        //FlagTabController.flagItemList.AddRange(tempList); 
    }


}
