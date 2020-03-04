using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Utilities.Components
{
    [AddComponentMenu("Utitlies/UI/SimpleTMPButton")]
    public class SimpleTMPButton : JustButton
    {
        [SerializeField]
        protected TextMeshProUGUI mLabelTMP;
        public TextMeshProUGUI labelTMP
        {
            get
            {
                if (mLabelTMP == null && !mFindLabel)
                {
                    mLabelTMP = GetComponentInChildren<TextMeshProUGUI>();
                    mFindLabel = true;
                }
                return mLabelTMP;
            }
        }
        private bool mFindLabel;

#if UNITY_EDITOR
        [ContextMenu("Validate")]
        protected override void OnValidate()
        {
            base.OnValidate();

            if (mLabelTMP == null)
                mLabelTMP = GetComponentInChildren<TextMeshProUGUI>();
        }
#endif
    }
}