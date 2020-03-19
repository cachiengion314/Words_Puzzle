using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panigation : MonoBehaviour
{
    public Sprite selectedSprite;
    public Sprite unSelectSprite;

    private Image image;
    
    void Awake()
    {
        image = GetComponent<Image>();
    }
    
    public void SetSprite(bool selected)
    {
        if (selected)
        {
            image.sprite = selectedSprite;
        }
        else
        {
            image.sprite = unSelectSprite;
        }
    }
}
