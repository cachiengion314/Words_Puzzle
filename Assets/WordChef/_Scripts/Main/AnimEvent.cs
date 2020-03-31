using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    public void EventAnimCallback()
    {
        if (Pan.instance != null)
        {
            Pan.instance.centerPoint.localScale = Vector3.zero;
            Sound.instance.Play(Sound.Collects.LevelShow);
        }
    }

    public void LevelClearCallback()
    {

    }
}
