using Superpow;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Utilites;

public class WorldController : BaseController
{
    [HideInInspector] public List<WorldItem> worldItems;
    public int target;
    [SerializeField] private int countChapterMax = 650;


    public int TotalChapter
    {
        get
        {
            return countChapterMax;
        }
    }
}
