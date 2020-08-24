using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{
    bool _isLoad;
    [SerializeField] private Slider _slideLoading;

    void Update()
    {
        if(_isLoad)
        {
            _slideLoading.value += Time.deltaTime;
        }
    }

    public void PlayLoading()
    {
        _slideLoading.value = 0;
        _isLoad = true;
    }
}
