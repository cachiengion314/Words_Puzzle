using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChapter : MonoBehaviour
{
    // Start is called before the first frame update
  public void OnChapterClick()
    {
        CUtils.LoadScene(Const.SCENE_CHAPTER, false);
        Sound.instance.Play(Sound.Others.PopupOpen);
        Debug.Log("DSDSDSDSD");
    }
}
