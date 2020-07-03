using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneAnimate.Instance.LoadScenHomeWithProgress();
    }

}
