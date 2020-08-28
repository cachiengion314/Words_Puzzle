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
        CheckTheme();
    }

    private void CheckTheme()
    {
        if(MainController.instance != null)
        {
            var currTheme = ThemesControl.instance.CurrTheme;
            selectedSprite = currTheme.uiData.meanWordData.selectedPanigation;
            unSelectSprite = currTheme.uiData.meanWordData.disablePanigation;
        }
        SetSprite(false);
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
