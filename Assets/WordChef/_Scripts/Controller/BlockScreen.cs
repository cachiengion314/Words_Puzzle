using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScreen : MonoBehaviour
{
    public static BlockScreen instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Block(bool block)
    {
        gameObject.SetActive(block);
    }
}
