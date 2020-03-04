/**
 * Author NBear - nbhung71711@gmail.com - 2017
 **/

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilities.Common;

namespace Utilities.Pattern.UI
{
    public class PanelController : PanelStack
    {
        #region Internal Class

        #endregion

        //==============================

        #region Members

        public bool useOnce = false;
        public bool enableFXTransition = false;
        public Button btnBack;

        internal UnityAction onWillShow;
        internal UnityAction onWillHide;
        internal UnityAction onDidShow;
        internal UnityAction onDidHide;

        private bool mIsShowing;
        private bool mIsTransiting;
        /// <summary>
        /// When panel is lock, any action pop from itseft or its parent will be restricted
        /// Note: in one momment, there should be no-more one locked child
        /// </summary>
        private bool mIsLock;
        private CanvasGroup mCanvasGroup;
        private Canvas mCanvas;

        internal CanvasGroup CanvasGroup
        {
            get
            {
                if (mCanvasGroup == null)
                    mCanvasGroup = GetComponent<CanvasGroup>();
                if (mCanvasGroup == null)
                    mCanvasGroup = gameObject.AddComponent<CanvasGroup>();
                return mCanvasGroup;
            }
        }
        /// <summary>
        /// Optional, incase we need to control sorting order
        /// </summary>
        internal Canvas Canvas
        {
            get
            {
                if (mCanvas == null)
                {
                    var rayCaster = GetComponent<GraphicRaycaster>();
                    if (rayCaster == null)
                        rayCaster = gameObject.AddComponent<GraphicRaycaster>();

                    mCanvas = gameObject.GetComponent<Canvas>();
                    if (mCanvas == null)
                        mCanvas = gameObject.AddComponent<Canvas>();

                    WaitUtil.Enqueue(() => { mCanvas.overrideSorting = true; }); //Quick-fix
                }
                return mCanvas;
            }
        }
        internal int CanvasOrder
        {
            get { return Canvas.sortingOrder; }
        }
        internal string CanvasLayer
        {
            get { return Canvas.sortingLayerName; }
        }

        internal bool IsShowing { get { return mIsShowing; } }
        internal bool IsTransiting { get { return mIsTransiting; } }
        internal bool IsLock { get { return mIsLock; } }

        #endregion

        //=================================

        #region Hide

        internal virtual void Hide(UnityAction OnDidHide = null)
        {
            if (!gameObject.activeSelf)
            {
                Log(name + " Panel is hidden");
                return;
            }

            CoroutineUtil.StartCoroutine(IE_Hide(OnDidHide));
        }

        protected IEnumerator IE_Hide(UnityAction pOnDidHide)
        {
            if (this.onWillHide != null) this.onWillHide();

            BeforeHiding();

            //Wait till there is no sub active panel
            while (panelStack.Count > 0)
            {
                var subPanel = panelStack.Pop();
                subPanel.Hide();

                if (panelStack.Count == 0)
                    yield return new WaitUntil(() => !subPanel.gameObject.activeSelf);
                else
                    yield return null;
            }

            PopAllPanel();

            if (enableFXTransition)
                yield return CoroutineUtil.StartCoroutine(IE_HideFX());

            gameObject.SetActive(false);

            yield return null;

            AfterHiding();

            if (pOnDidHide != null) pOnDidHide();
            if (this.onDidHide != null) this.onDidHide();
        }

        protected virtual IEnumerator IE_HideFX()
        {
            yield break;
        }

        protected virtual void BeforeHiding()
        {
            LockWhileTransiting(true);
        }

        protected virtual void AfterHiding()
        {
            LockWhileTransiting(false);
            mIsShowing = false;

            if (useOnce)
                Destroy(gameObject, 0.1f);
        }

        #endregion

        //==================================

        #region Show

        internal virtual void Show(UnityAction pOnDidShow = null)
        {
            if (gameObject.activeSelf)
            {
                Log(name + " Panel is shown");
                return;
            }

            CoroutineUtil.StartCoroutine(IE_Show(pOnDidShow));
        }

        protected IEnumerator IE_Show(UnityAction pOnDidShow)
        {
            if (this.onWillShow != null) this.onWillShow();

            BeforeShowing();

            //Make the shown panel on the top of all other siblings
            transform.SetAsLastSibling();

            gameObject.SetActive(true);
            if (enableFXTransition)
                yield return CoroutineUtil.StartCoroutine(IE_ShowFX());

            yield return null;

            AfterShowing();

            if (pOnDidShow != null) pOnDidShow();
            if (this.onDidShow != null) this.onDidShow();
        }

        protected virtual IEnumerator IE_ShowFX()
        {
            yield break;
        }

        protected virtual void BeforeShowing()
        {
            LockWhileTransiting(true);
            mIsShowing = true;
        }

        protected virtual void AfterShowing()
        {
            LockWhileTransiting(false);
        }

        #endregion

        //===================================

        #region Monobehaviour

        protected override void Awake()
        {
            base.Awake();

            if (btnBack != null)
                btnBack.onClick.AddListener(BtnBack_Pressed);
        }

        protected virtual void OnDisable()
        {
            LockWhileTransiting(false);

            mIsShowing = false;
        }

        #endregion

        //======================================================

        #region Methods

        private void LockWhileTransiting(bool value)
        {
            mIsTransiting = value;

            if (enableFXTransition)
                CanvasGroup.interactable = !mIsTransiting;
        }

        internal virtual void Back()
        {
            if (mParentPanel == null)
                LogError("Parent panel is null");
            else
                //parentPanel.PopPanel();
                mParentPanel.PopChildrenThenParent();
        }

        protected void BtnBack_Pressed()
        {
            Back();
        }

        internal bool CanPop()
        {
            foreach (var p in panelStack)
            {
                if (p.mIsLock)
                    return true;
            }
            if (mIsLock)
                return true;
            return false;
        }

        internal virtual void Init() { }

        internal void Lock(bool pLock)
        {
            mIsLock = pLock;
        }

        public bool IsActiveAndEnabled()
        {
            return !gameObject.IsPrefab() && this.isActiveAndEnabled;
        }

        public PanelController GetHighestPanel()
        {
            if (TopPanel != null)
                return TopPanel.GetHighestPanel();
            return this;
        }

        #endregion
    }
}