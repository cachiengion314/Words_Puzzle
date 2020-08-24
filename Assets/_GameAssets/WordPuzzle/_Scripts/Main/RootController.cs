using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootController : MonoBehaviour
{
    public static RootController instance;
  
    private void Awake()
    {
        instance = this;

    }

}
