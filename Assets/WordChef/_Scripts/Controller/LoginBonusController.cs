using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginBonusController : MonoBehaviour
{
    public static LoginBonusController instance;
    [SerializeField] private GameObject _root;
    [SerializeField] private GameObject _objSpin;
    [SerializeField] private int _numGift = 6;
    [SerializeField] private int _angleStart = 0;
    void Awake()
    {
        if (instance == null)
            instance = this;
        CheckToday();
    }

    private void CheckToday()
    {
        var date = DateTime.Parse(CPlayerPrefs.GetString("Daily", DateTime.Today.ToString()));
        if (DateTime.Compare(DateTime.Today,date) > 0)
        {
            TweenControl.GetInstance().ScaleFromZero(_root,0.3f,null);
        }
    }

    public void Spin()
    {
        CPlayerPrefs.SetString("Daily", DateTime.Today.ToString());
        var angle = Angle();
        TweenControl.GetInstance().LocalRotate(_objSpin.transform,new Vector3(0,0, -angle),5f,()=> { 
        
        },EaseType.OutQuad);
    }

    private float Angle()
    {
        var angle = _angleStart + 360f / _numGift + 360f * 5f;
        return angle;
    } 
}
