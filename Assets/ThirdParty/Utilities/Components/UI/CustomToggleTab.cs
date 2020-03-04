using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Utilities.Common;
using Utilities.Inspector;
using UnityEngine.EventSystems;
using System;

namespace Utilities.Components
{
    [AddComponentMenu("Utitlies/UI/CustomToggleTab")]
    public class CustomToggleTab : Toggle
    {
        public TextMeshProUGUI txtLabel;
        public List<RectTransform> contentsInActive;
        public List<RectTransform> contentsInDeactive;
        public Sprite sptActiveBackground;
        public Sprite sptInactiveBackground;
        public Color colorActiveBackground;
        public Color colorInactiveBackground;
        public Vector2 sizeActive;
        public Vector2 sizeDeactive;
        [ReadOnly] public bool isLocked;

        public Image ImgBackground { get { return targetGraphic as Image; } }
        public Image ImgCheckmark { get { return graphic as Image; } }

        public Action OnClickOnLock;

#if UNITY_EDITOR
        [ContextMenu("Validate")]
        private void Validate()
        {
            base.OnValidate();

            if (txtLabel == null)
                txtLabel = gameObject.FindComponentInChildren<TextMeshProUGUI>();

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

            onValueChanged.AddListener(OnValueChanged);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            onValueChanged.RemoveListener(OnValueChanged);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (isLocked)
            {
                OnClickOnLock.Raise();
                return;
            }

            base.OnPointerClick(eventData);
        }

        private void Refresh()
        {
            foreach (var item in contentsInActive)
                item.SetActive(isOn);

            foreach (var item in contentsInDeactive)
                item.SetActive(!isOn);

            if (sptActiveBackground != null && sptInactiveBackground != null)
                ImgBackground.sprite = isOn ? sptActiveBackground : sptInactiveBackground;

            if (sizeActive != Vector2.zero && sizeDeactive != Vector2.zero)
                (transform as RectTransform).sizeDelta = isOn ? sizeActive : sizeDeactive;

            if (colorActiveBackground != default(Color) && colorInactiveBackground != default(Color))
                ImgBackground.color = isOn ? colorActiveBackground : colorInactiveBackground;
        }

        private void OnValueChanged(bool pIson)
        {
            Refresh();
        }
    }
}
