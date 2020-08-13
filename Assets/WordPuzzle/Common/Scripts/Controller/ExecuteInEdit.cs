using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

//[ExecuteInEditMode]
public class ExecuteInEdit : MonoBehaviour
{
    private readonly string COUNTRY_NAME = "Name";
    private readonly string SUB_REGION = "Subregion";
    private readonly string CAPITAL = "Capital";
    private readonly string AREA = "Area";
    private readonly string POPULATION = "Population";

    public FlagTabController FlagTabController;

    void Awake()
    {
        //for (int i = 0; i < FlagTabController.smallFlags.Length; i++)
        //{
        //    for (int ii = 0; ii < FlagTabController.flagItemList.Count; ii++)
        //    {
        //        if (FlagTabController.smallFlags[i].name.Equals(FlagTabController.flagItemList[ii].flagName, StringComparison.OrdinalIgnoreCase))
        //        {
        //            FlagTabController.flagItemList[ii].flagSmallImageIndex = i;
        //        }
        //    }
        //}
        //for (int i = 0; i < FlagTabController.bigFlags.Length; i++)
        //{
        //    for (int ii = 0; ii < FlagTabController.flagItemList.Count; ii++)
        //    {
        //        if (FlagTabController.bigFlags[i].name.Equals(FlagTabController.flagItemList[ii].flagName, StringComparison.OrdinalIgnoreCase))
        //        {
        //            FlagTabController.flagItemList[ii].flagBigImageIndex = i;
        //        }
        //    }
        //}

        Object jsonDataObject = Resources.Load("data");
        string jsonData = jsonDataObject.ToString();
        Dictionary<string, object> tempDic = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData);
        JArray jsonArr = JArray.Parse(tempDic["Sheet1"].ToString());
        for (int i = 0; i < jsonArr.Count; i++)
        {
            Dictionary<string, string> tempCountryDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonArr[i].ToString());

            for (int ii = 0; ii < FlagTabController.flagItemList.Count; ii++)
            {
                if (FlagTabController.flagItemList[ii].flagName.Equals(tempCountryDic[COUNTRY_NAME], System.StringComparison.OrdinalIgnoreCase))
                {
                    FlagTabController.flagItemList[ii].flagName = tempCountryDic[COUNTRY_NAME];
                    FlagTabController.flagItemList[ii].subRegion = tempCountryDic[SUB_REGION];
                    FlagTabController.flagItemList[ii].capital = tempCountryDic[CAPITAL];
                    FlagTabController.flagItemList[ii].population = tempCountryDic[POPULATION];
                    FlagTabController.flagItemList[ii].area = tempCountryDic[AREA];
                }
            }
        }

    }
}
