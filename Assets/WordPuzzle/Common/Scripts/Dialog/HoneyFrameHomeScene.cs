using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyFrameHomeScene : MonoBehaviour
{
    [HideInInspector] public static bool isClickOnThis;
    public void HoneyFrameHomeSceneClick()
    {
        isClickOnThis = true;
    }
}
