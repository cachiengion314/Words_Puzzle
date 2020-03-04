using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace Utilities.Components
{
    [AddComponentMenu("Utitlies/UI/CustomToggleSlider")]
    public class CustomToggleSlider : Toggle
    {
        public TextMeshProUGUI txtLabel;
        public Vector2 onPosition;
        public Vector2 offPosition;
        public RectTransform toggleTransform;
        public GameObject onObject;
        public GameObject offObject;
        public Sprite sptOnButton;
        public Sprite sptOffButton;

#if UNITY_EDITOR
        [ContextMenu("Validate")]
        protected void Validate()
        {
            if (txtLabel == null)
                txtLabel = gameObject.GetComponentInChildren<TextMeshProUGUI>();

            if (targetGraphic == null || graphic == null)
            {
                var images = gameObject.GetComponentsInChildren<Image>();
                targetGraphic = images[0];
                graphic = images[1];
            }

            if (graphic != null)
            {
                if (isOn)
                    graphic.gameObject.SetActive(true);
                else
                    graphic.gameObject.SetActive(false);
            }
        }
#endif

        protected override void OnEnable()
        {
            base.OnEnable();

            Refresh();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            Refresh();
        }

        private void Refresh()
        {
            onObject.SetActive(isOn);
            offObject.SetActive(!isOn);
            image.sprite = isOn ? sptOnButton : sptOffButton;
            toggleTransform.anchoredPosition = isOn ? onPosition : offPosition;
        }
    }
}
