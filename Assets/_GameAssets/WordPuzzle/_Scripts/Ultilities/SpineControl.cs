﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using System;


public class SpineControl : MonoBehaviour
{
    private Dictionary<string, Action> callBackEndDic = new Dictionary<string, Action>();
    private Dictionary<string, Action> callBackStartDic = new Dictionary<string, Action>();

    public Action<Spine.Event> onEventAction;

    // [HideInInspector]
    public SkeletonGraphic thisSkeletonControl
    {
        get
        {
            if (ske == null)
            {
                ske = transform.GetComponent<SkeletonGraphic>();
            }
            if (ske == null)
            {
                ske = transform.GetComponentInChildren<SkeletonGraphic>();
            }

            return ske;
        }
    }

    public SkeletonGraphic ske;


    public RectTransform thisRect
    {
        get
        {
            if (_rect == null)
            {
                _rect = transform.GetComponent<RectTransform>();
            }
            return _rect;
        }
    }
    private RectTransform _rect;
    public void Start()
    {
        try
        {
            var stage = thisSkeletonControl.AnimationState/*new Spine.AnimationState(thisSkeletonControl.skeletonDataAsset.GetAnimationStateData())*/;

            //thisSkeletonControl.AnimationState = stage;

            stage.Complete += AnimationState_Complete;
            stage.Start += AnimationState_Start;
            try
            {
                thisSkeletonControl.AnimationState.Event += AnimationState_Event;
            }
            catch
            {
                stage.Event += AnimationState_Event;
            }
        }
        catch
        {

        }
    }

    private void AnimationState_Event(TrackEntry trackEntry, Spine.Event e)
    {
        onEventAction?.Invoke(e);
    }

    private void AnimationState_Start(TrackEntry trackEntry)
    {
        var key = trackEntry.Animation.Name;

        if (!callBackStartDic.ContainsKey(key)) return;


        var action = callBackStartDic[key];
        action?.Invoke();
    }

    private void AnimationState_Complete(TrackEntry trackEntry)
    {
        var key = trackEntry.Animation.Name;
        if (!callBackEndDic.ContainsKey(key)) return;
        var action = callBackEndDic[key];
        action?.Invoke();
    }
    public void RemoveCallBack(string anim, Action callBack)
    {
        if (callBackEndDic.ContainsKey(anim))
        {
            var action = callBackEndDic[anim];

            action = null;
        }

        if (callBackStartDic.ContainsKey(anim))
        {
            var action = callBackStartDic[anim];
            action = null;
        }
    }

    public void SetAnimation(string animationName, bool loop, System.Action completeAnimation = null, System.Action startAnimation = null)
    {
        if (thisSkeletonControl == null || thisSkeletonControl.AnimationState == null)
        {
            Debug.Log("null skeleton");
            return;
        }

        thisSkeletonControl.AnimationState.SetAnimation(0, animationName, loop);

        AddCallBack(animationName, completeAnimation);
        AddStartCallBack(animationName, startAnimation);
    }
    public void ChangeSkin(string skinName)
    {
        thisSkeletonControl.Skeleton.SetSkin(skinName);
        thisSkeletonControl.Skeleton.SetSlotsToSetupPose();
        thisSkeletonControl.AnimationState.Apply(thisSkeletonControl.Skeleton);
    }

    public void SetAnimation(string skinName, string animationName, bool loop, System.Action completeAnimation = null, System.Action startAnimation = null, bool removeCallBack = true)
    {
        thisSkeletonControl.Skeleton.SetSkin(skinName);
        thisSkeletonControl.Skeleton.SetSlotsToSetupPose();
        thisSkeletonControl.AnimationState.Apply(thisSkeletonControl.Skeleton);
        thisSkeletonControl.AnimationState.SetAnimation(0, animationName, loop);

        AddCallBack(animationName, completeAnimation);
        AddStartCallBack(animationName, startAnimation);
    }

    public void SetSkin(string skinName)
    {
        thisSkeletonControl.Skeleton.SetSkin(skinName);
        thisSkeletonControl.Skeleton.SetSlotsToSetupPose();
        thisSkeletonControl.AnimationState.Apply(thisSkeletonControl.Skeleton);

    }

    void AddCallBack(string anim, Action callBack)
    {
        if (callBackEndDic.ContainsKey(anim))
        {
            callBackEndDic[anim] = callBack;
        }
        else
        {
            callBackEndDic.Add(anim, callBack);
        }
    }
    void AddStartCallBack(string anim, Action callBack)
    {
        if (callBackStartDic.ContainsKey(anim))
        {
            callBackStartDic[anim] = callBack;
        }
        else
        {
            callBackStartDic.Add(anim, callBack);
        }
    }
}
