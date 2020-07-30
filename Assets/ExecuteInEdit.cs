using Newtonsoft.Json;
using System;
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
    }   
}
