using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Common
{
    public class CoroutineMediator : MonoBehaviour
    {
        private static CoroutineMediator mInstance;
        public static CoroutineMediator instance
        {
            get
            {
                if (mInstance == null)
                {
                    var temp = new GameObject("CoroutineMedator");
                    mInstance = temp.AddComponent<CoroutineMediator>();
                    DontDestroyOnLoad(temp);
                }
                return mInstance;
            }
        }

        private List<WaitUtil.CountdownEvent> mCountdownEventList = new List<WaitUtil.CountdownEvent>();
        private List<WaitUtil.ConditionEvent> mConditionEventList = new List<WaitUtil.ConditionEvent>();
        private List<WaitUtil.IUpdate> mUpdateList = new List<WaitUtil.IUpdate>();
        private Queue<Action> mListQueueActions = new Queue<Action>();

        private float mTimeBeforePause;

        private void Awake()
        {
            if (mInstance == null)
            {
                mInstance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (mInstance != this)
            {
                Destroy(gameObject);
            }
        }

        private void LateUpdate()
        {
            float pausedTime = 0;
            if (mTimeBeforePause > 0)
            {
                pausedTime = Time.unscaledTime - mTimeBeforePause;
                mTimeBeforePause = 0;
            }

            for (int i = mCountdownEventList.Count - 1; i >= 0; i--)
            {
                var d = mCountdownEventList[i];

                if (d.unscaledTime)
                    d.AddElapsedTime(Time.unscaledDeltaTime + pausedTime);
                else
                    d.AddElapsedTime(Time.deltaTime);
                if (d.breakCondition != null && d.breakCondition())
                {
                    if (!d.autoRestart)
                        mCountdownEventList.Remove(d);
                    else
                        d.Restart();
                }
                else if (d.IsTimeOut)
                {
                    d.doSomething(d.Elapsed - d.waitTime);
                    if (!d.autoRestart)
                        mCountdownEventList.Remove(d);
                    else
                        d.Restart();
                }
            }
            for (int i = mConditionEventList.Count - 1; i >= 0; i--)
            {
                var d = mConditionEventList[i];
                if (d.triggerCondition())
                {
                    d.doSomething();
                    mConditionEventList.Remove(d);
                }
            }
            for (int i = mUpdateList.Count - 1; i >= 0; i--)
            {
                var d = mUpdateList[i];
                d.Update(Time.unscaledDeltaTime);
            }
            if (mListQueueActions.Count > 0)
            {
                mListQueueActions.Peek().Raise();
                mListQueueActions.Dequeue();
            }
            enabled = mCountdownEventList.Count > 0 || mConditionEventList.Count > 0 || mUpdateList.Count > 0 || mListQueueActions.Count > 0;
        }

        public void Init()
        {
            if (mInstance == null)
            {
                var temp = new GameObject("Coroutine Mediator");
                mInstance = temp.AddComponent<CoroutineMediator>();
                DontDestroyOnLoad(temp);
            }
        }

        public WaitUtil.CountdownEvent WaitForSecond(WaitUtil.CountdownEvent pScheduleEvent)
        {
            if (pScheduleEvent.id == 0)
                mCountdownEventList.Add(pScheduleEvent);
            else
            {
                bool exist = false;
                for (int i = 0; i < mCountdownEventList.Count; i++)
                {
                    if (pScheduleEvent.id == mCountdownEventList[i].id)
                    {
                        exist = true;
                        mCountdownEventList[i] = pScheduleEvent;
                        break;
                    }
                }

                if (!exist)
                    mCountdownEventList.Add(pScheduleEvent);
            }

            enabled = true;

            return pScheduleEvent;
        }

        internal void Enqueue(Action pDoSomething)
        {
            mListQueueActions.Enqueue(pDoSomething);
        }

        public WaitUtil.ConditionEvent WaitForCondition(WaitUtil.ConditionEvent pScheduleEvent)
        {
            if (pScheduleEvent.id == 0)
                mConditionEventList.Add(pScheduleEvent);
            else
            {
                bool exist = false;
                for (int i = 0; i < mConditionEventList.Count; i++)
                {
                    if (pScheduleEvent.id == mConditionEventList[i].id)
                    {
                        exist = true;
                        mConditionEventList[i] = pScheduleEvent;
                        break;
                    }
                }

                if (!exist)
                    mConditionEventList.Add(pScheduleEvent);
            }

            enabled = true;

            return pScheduleEvent;
        }

        public WaitUtil.IUpdate AddUpdate(WaitUtil.IUpdate pScheduleEvent)
        {
            if (pScheduleEvent.id == 0)
                mUpdateList.Add(pScheduleEvent);
            else
            {
                bool exist = false;
                for (int i = 0; i < mUpdateList.Count; i++)
                {
                    if (pScheduleEvent.id == mUpdateList[i].id)
                    {
                        exist = true;
                        mUpdateList[i] = pScheduleEvent;
                        break;
                    }
                }

                if (!exist)
                    mUpdateList.Add(pScheduleEvent);
            }

            enabled = true;

            return pScheduleEvent;
        }

        public void Clear()
        {
            mCountdownEventList = new List<WaitUtil.CountdownEvent>();
            mConditionEventList = new List<WaitUtil.ConditionEvent>();
            mUpdateList = new List<WaitUtil.IUpdate>();
            mListQueueActions = new Queue<Action>();
            enabled = false;
        }

        internal void RemoveTimeAction(int id)
        {
            for (int i = 0; i < mCountdownEventList.Count; i++)
            {
                var d = mCountdownEventList[i];
                if (d.id == id)
                {
                    mCountdownEventList.Remove(d);
                    return;
                }
            }
        }

        internal void RemoveTriggerAction(int id)
        {
            for (int i = 0; i < mConditionEventList.Count; i++)
            {
                var d = mConditionEventList[i];
                if (d.id == id)
                {
                    mConditionEventList.Remove(d);
                    return;
                }
            }
        }

        internal void RemoveUpdate(int id)
        {
            for (int i = 0; i < mUpdateList.Count; i++)
            {
                var d = mUpdateList[i];
                if (d.id == id)
                {
                    mUpdateList.Remove(d);
                    return;
                }
            }
        }

        internal void RemoveTimeAction(WaitUtil.CountdownEvent pCounter)
        {
            mCountdownEventList.Remove(pCounter);
        }

        internal void RemoveTriggerAction(WaitUtil.ConditionEvent pCounter)
        {
            mConditionEventList.Remove(pCounter);
        }

        internal void RemoveUpdate(WaitUtil.IUpdate pUpdate)
        {
            mUpdateList.Remove(pUpdate);
        }

        private void OnApplicationPause(bool pause)
        {
            if (!pause)
            {
                if (enabled)
                    mTimeBeforePause = Time.unscaledTime;
            }
        }
    }
}