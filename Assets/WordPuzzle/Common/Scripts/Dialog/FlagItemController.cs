using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlagItemController : MonoBehaviour
{
    [HideInInspector] public Sprite flagImage;
    [HideInInspector] public string flagName;

    [SerializeField] private Image flagImg;
    [SerializeField] private TextMeshProUGUI nameTxt;

    private Toggle thisFlagItemToggle;
    private void Awake()
    {
        thisFlagItemToggle = GetComponent<Toggle>();
    }
    private void Start()
    {
        flagImg.sprite = flagImage;
        nameTxt.text = flagName;
    }
}
