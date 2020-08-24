using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Components;
using DG.Tweening;

public class FirstScreen : MonoBehaviour
{
    public Image imgBlack;
    public ResolutionMatch resolutionMatch;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return null;
        resolutionMatch.Match();
        imgBlack.DOFade(0f, 0.4f).SetDelay(0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
