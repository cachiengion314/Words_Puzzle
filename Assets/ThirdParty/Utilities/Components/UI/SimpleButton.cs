using UnityEngine;
using UnityEngine.UI;

namespace Utilities.Components
{
    [AddComponentMenu("Utitlies/UI/SimpleButton")]
    public class SimpleButton : JustButton
    {
        [SerializeField]
        protected Text mLabel;
        public Text label
        {
            get
            {
                if (mLabel == null && !mFindLabel)
                {
                    mLabel = GetComponentInChildren<Text>();
                    mFindLabel = true;
                }
                return mLabel;
            }
        }
        protected bool mFindLabel;

#if UNITY_EDITOR
        [ContextMenu("Validate")]
        protected override void OnValidate()
        {
            if (mLabel == null)
                mLabel = GetComponentInChildren<Text>();
        }
#endif
    }
}