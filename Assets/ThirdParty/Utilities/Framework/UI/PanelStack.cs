/**
 * Author NBear - nbhung71711@gmail.com - 2017
 **/

using System.Collections.Generic;
using UnityEngine;
using Utilities.Common;

namespace Utilities.Pattern.UI
{
    public class PanelStack : MonoBehaviour
    {
        protected Stack<PanelController> panelStack = new Stack<PanelController>();
        protected Dictionary<int, PanelController> mCachedOnceUsePanels = new Dictionary<int, PanelController>();

        protected PanelStack mParentPanel;
        /// <summary>
        /// Top child
        /// </summary>
        public PanelController TopPanel
        {
            get
            {
                if (panelStack.Count > 0)
                    return panelStack.Peek();
                return null;
            }
        }
        /// <summary>
        /// Index in stack
        /// </summary>
        public int Index
        {
            get
            {
                if (mParentPanel == null)
                    return 0;
                else
                {
                    int i = 0;
                    foreach (var p in mParentPanel.panelStack)
                    {
                        if (p == this)
                            return i;
                        i++;
                    }
                }
                return mParentPanel.panelStack.Count;
            }
        }
        /// <summary>
        /// Order base-on active sibling
        /// </summary>
        public int PanelOrder
        {
            get
            {
                if (mParentPanel == null)
                    return 1;
                return mParentPanel.panelStack.Count - Index;
            }
        }
        /// <summary>
        /// Total children panels
        /// </summary>
        public int StackCount { get { return panelStack.Count; } }

        protected virtual void Awake()
        {
            if (mParentPanel == null)
                mParentPanel = GetComponentInParent<PanelController>();
            if (mParentPanel == this)
                mParentPanel = null;
        }

        //=============================================================

        #region Create

        /// <summary>
        /// Create and init panel
        /// </summary>
        /// <typeparam name="T">Panels inherit PanelController</typeparam>
        /// <param name="pPanel">Can be prefab or buildin prefab</param>
        /// <returns></returns>
        protected T CreatePanel<T>(ref T pPanel) where T : PanelController
        {
            if (!pPanel.useOnce)
            {
                if (pPanel.gameObject.IsPrefab())
                {
                    pPanel = Instantiate(pPanel, transform);
                    pPanel.SetActive(false);
                    pPanel.Init();
                }
                return pPanel as T;
            }
            else
            {
                if (!pPanel.gameObject.IsPrefab())
                    Debug.LogWarning("One time use panel should be prefab!");

                var panel = Instantiate(pPanel, transform);
                panel.useOnce = true;
                panel.SetActive(false);
                panel.Init();

                if (!mCachedOnceUsePanels.ContainsKey(pPanel.GetInstanceID()))
                    mCachedOnceUsePanels.Add(pPanel.GetInstanceID(), panel);
                else
                    mCachedOnceUsePanels[pPanel.GetInstanceID()] = panel;

                return panel as T;
            }
        }

        /// <summary>
        /// Find child panel of this Panel
        /// </summary>
        /// <typeparam name="T">Panels inherit PanelController</typeparam>
        /// <param name="pOriginal">Can be prefab or buildin prefab</param>
        /// <returns></returns>
        protected T GetCachedPanel<T>(T pOriginal) where T : PanelController
        {
            if (pOriginal.useOnce)
            {
                if (mCachedOnceUsePanels.ContainsKey(pOriginal.GetInstanceID()))
                    return mCachedOnceUsePanels[pOriginal.GetInstanceID()] as T;
                else
                    return pOriginal;
            }
            else
            {
                //var panel = CreatePanel(ref pOriginal);
                return pOriginal;
            }
        }

        #endregion

        //=============================================================

        #region Single

        /// <summary>
        /// Check if panel is prefab or build-in prefab then create and init
        /// </summary>
        internal virtual T PushPanel<T>(ref T pPanel, bool keepCurrentInStack, bool onlyInactivePanel = true, bool sameTimePopAndPush = true) where T : PanelController
        {
            var panel = CreatePanel(ref pPanel);
            PushPanel(panel, keepCurrentInStack, onlyInactivePanel, sameTimePopAndPush);
            return panel;
        }

        /// <summary>
        /// Push new panel will hide the current top panel
        /// </summary>
        /// <param name="panel">New Top Panel</param>
        /// <param name="onlyDisablePanel">Do nothing if panel is currently active</param>
        /// <param name="sameTimePopAndPush">Allow pop current panel and push new </param>
        internal virtual void PushPanel(PanelController panel, bool keepCurrentInStack, bool onlyInactivePanel = true, bool sameTimePopAndPush = true)
        {
            if (panel == null)
            {
                Log("Panel is null");
                return;
            }

            if (onlyInactivePanel && panel.gameObject.activeSelf && panelStack.Contains(panel))
            {
                Log("Panel is already active " + panel.name);
                return;
            }

            if (this.TopPanel == panel)
                return;

            if (this.TopPanel != null && this.TopPanel.CanPop())
            {
                //If top panel is locked we must keep it
                PushPanelToTop(panel);
                return;
            }

            panel.mParentPanel = this;
            if (this.TopPanel != null)
            {
                PanelController currentTopPanel = this.TopPanel;
                if (currentTopPanel.IsShowing)
                {
                    var lastTopPanel = currentTopPanel;
                    currentTopPanel.Hide(() =>
                    {
                        if (!sameTimePopAndPush)
                        {
                            if (!keepCurrentInStack)
                                panelStack.Pop();
                            panelStack.Push(panel);
                            panel.Show();
                            OnAnyChildShow(panel);
                        }

                        OnAnyChildHide(lastTopPanel);
                    });

                    if (sameTimePopAndPush)
                    {
                        if (!keepCurrentInStack)
                            panelStack.Pop();
                        panelStack.Push(panel);
                        panel.Show();
                        OnAnyChildShow(panel);
                    }
                }
                else
                {
                    if (!keepCurrentInStack)
                        panelStack.Pop();
                    panelStack.Push(panel);
                    panel.Show();
                    OnAnyChildShow(panel);
                }
            }
            else
            {
                panelStack.Push(panel);
                panel.Show();
                OnAnyChildShow(panel);
            }
        }

        /// <summary>
        /// Pop the top panel off the stack and show the one beheath it
        /// </summary>
        internal virtual void PopPanel(bool actionSameTime = true)
        {
            if (this.TopPanel == null)
            {
                Log("Top Panel is null");
                return;
            }

            if (this.TopPanel != null && this.TopPanel.CanPop())
            {
                Log("Current top panel is locked");
                return;
            }

            var topStack = panelStack.Pop();

            if (topStack.IsShowing)
            {
                topStack.Hide(() =>
                {
                    if (!actionSameTime)
                    {
                        var newPanel = this.TopPanel;
                        if (newPanel != null && !newPanel.IsShowing)
                        {
                            newPanel.Show();
                            OnAnyChildShow(newPanel);
                        }
                    }

                    OnAnyChildHide(topStack);
                });

                if (actionSameTime)
                {
                    var newPanel = this.TopPanel;
                    if (newPanel != null && !newPanel.IsShowing)
                    {
                        newPanel.Show();
                        OnAnyChildShow(newPanel);
                    }
                }
            }
            else
            {
                var newPanel = this.TopPanel;
                if (newPanel != null && !newPanel.IsShowing)
                {
                    newPanel.Show();
                    OnAnyChildShow(newPanel);
                }
            }
        }

        /// <summary>
        /// Check if panel is prefab or build-in prefab then create and init
        /// </summary>
        internal virtual T PushPanelToTop<T>(ref T pPanel) where T : PanelController
        {
            var panel = CreatePanel(ref pPanel);
            PushPanelToTop(panel);
            return panel;
        }

        /// <summary>
        /// Push panel without hiding panel is under it
        /// </summary>
        internal virtual void PushPanelToTop(PanelController panel)
        {
            if (this.TopPanel == panel)
                return;

            panelStack.Push(panel);
            panel.mParentPanel = this;
            panel.Show();
            OnAnyChildShow(panel);
        }

        #endregion

        //=============================================================

        #region Multi

        /// <summary>
        /// Keep only one panel in stack
        /// </summary>
        internal virtual void PopAllThenPush(PanelController panel)
        {
            PopAllPanel();
            PushPanel(panel, false);
        }

        /// <summary>
        /// Pop all panels till there is only one panel left in the stack
        /// </summary>
        internal virtual void PopTillOneLeft()
        {
            bool changed = false;

            var panelsIsLocked = new List<PanelController>();
            PanelController lastTopPanel = null;
            while (panelStack.Count > 1)
            {
                changed = true;

                lastTopPanel = panelStack.Pop();
                if (lastTopPanel.CanPop())
                    //Locked panel should not be hide
                    panelsIsLocked.Add(lastTopPanel);
                else
                    lastTopPanel.Hide();
            }

            //Resign every locked panel, because we removed them temporarity above
            if (panelsIsLocked.Count > 0)
            {
                for (int i = panelsIsLocked.Count - 1; i >= 0; i--)
                    panelStack.Push(panelsIsLocked[i]);
            }

            if (!TopPanel.IsShowing)
            {
                TopPanel.Show();
                OnAnyChildShow(TopPanel);
            }

            if (changed)
                OnAnyChildHide(lastTopPanel);
        }

        /// <summary>
        /// Pop till we remove specific panel
        /// </summary>
        internal virtual void PopTillNoPanel(PanelController panel)
        {
            if (!panelStack.Contains(panel))
            {
                Log("Panel is not showing or is not under parent");
                return;
            }

            bool changed = false;
            var panelsIsLocked = new List<PanelController>();
            PanelController lastTopPanel = null;

            //Pop panels until we find the right one we're trying to pop
            do
            {
                changed = true;

                lastTopPanel = panelStack.Pop();
                if (lastTopPanel.CanPop())
                    //Locked panel should not be hide
                    panelsIsLocked.Add(lastTopPanel);
                else
                    lastTopPanel.Hide();

            } while (lastTopPanel.GetInstanceID() != panel.GetInstanceID() && panelStack.Count > 0);

            //Resign every locked panel, because we removed them temporarity above
            if (panelsIsLocked.Count > 0)
            {
                for (int i = panelsIsLocked.Count - 1; i >= 0; i--)
                    panelStack.Push(panelsIsLocked[i]);
            }

            var newPanel = this.TopPanel;
            if (newPanel != null && !newPanel.IsShowing)
            {
                newPanel.Show();
                OnAnyChildShow(newPanel);
            }

            if (changed)
                OnAnyChildHide(lastTopPanel);
        }

        /// <summary>
        /// Pop and hide all panels in stack, at the same time
        /// </summary>
        internal virtual void PopAllPanel()
        {
            bool changed = false;

            var panelsIsLocked = new List<PanelController>();
            PanelController lastTopPanel = null;
            while (panelStack.Count > 0)
            {
                changed = true;

                lastTopPanel = panelStack.Pop();
                if (lastTopPanel.CanPop())
                    //Locked panel should not be hide
                    panelsIsLocked.Add(lastTopPanel);
                else
                    lastTopPanel.Hide();
            }

            //Resign every locked panel, because we removed them temporarity above
            if (panelsIsLocked.Count > 0)
            {
                for (int i = panelsIsLocked.Count - 1; i >= 0; i--)
                    panelStack.Push(panelsIsLocked[i]);
            }

            if (changed)
                OnAnyChildHide(lastTopPanel);
        }

        /// <summary>
        /// Pop one by one, chilren then parent
        /// </summary>
        internal virtual void PopChildrenThenParent()
        {
            if (this.TopPanel == null)
                return;

            if (this.TopPanel.TopPanel != null)
                this.TopPanel.PopChildrenThenParent();
            else
                PopPanel();
        }

        #endregion

        //==============================================================

        protected virtual void OnAnyChildHide(PanelController pPanel)
        {
            //Parent notifies to grandparent of hidden panel
            if (mParentPanel != null)
                mParentPanel.OnAnyChildHide(pPanel);
        }
        protected virtual void OnAnyChildShow(PanelController pPanel)
        {
            if (mParentPanel != null)
                mParentPanel.OnAnyChildShow(pPanel);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        protected void Log(string pMessage)
        {
            Debug.Log(string.Format("<color=yellow><b>[{1}]:</b></color>{1}", gameObject.name, pMessage));
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        protected void LogError(string pMessage)
        {
            Debug.LogError(string.Format("<color=red><b>[{1}]:</b></color>{1}", gameObject.name, pMessage));
        }
    }
}