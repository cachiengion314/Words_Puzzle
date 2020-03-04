/**
 * Author NBear - nbhung71711@gmail.com - 2018
 **/

using System;

namespace Utilities.Common
{
    public class WaitUtil
    {
        [System.Serializable]
        public class CountdownEvent
        {
            public int id;
            public Action<float> doSomething;
            public ConditionalDelegate breakCondition;
            public float waitTime;
            public bool unscaledTime;
            public bool autoRestart;

            private float mElapsed;
            private float mElapsedOffset;

            public float Elapsed { get { return mElapsed; } }
            public float ElapsedOffset { get { return mElapsedOffset; } }
            public bool IsTimeOut { get { return mElapsed >= waitTime; } }
            public float Remain { get { return (waitTime - mElapsed > 0) ? (waitTime - mElapsed) : 0; } }

            public CountdownEvent() { }

            public void Set(CountdownEvent other)
            {
                id = other.id;
                doSomething = other.doSomething;
                breakCondition = other.breakCondition;
                waitTime = other.waitTime;
                unscaledTime = other.unscaledTime;
                mElapsed = other.mElapsed;
                mElapsedOffset = other.mElapsedOffset;
            }

            public void AddElapsedTime(float pValue)
            {
                mElapsed += pValue;
            }

            public void Restart()
            {
                mElapsed = 0;
                mElapsedOffset = 0;
            }

            public void SetElapsedOffset(float pValue)
            {
                mElapsedOffset = pValue;
            }
        }

        //======================

        [System.Serializable]
        public class ConditionEvent
        {
            public int id;
            public ConditionalDelegate triggerCondition;
            public Action doSomething;

            public ConditionEvent() { }

            public void Set(ConditionEvent other)
            {
                id = other.id;
                triggerCondition = other.triggerCondition;
                doSomething = other.doSomething;
            }
        }

        //======================

        public interface IUpdate
        {
            int id { get; set; }

            void Update(float pDetalTime);
        }

        //======================

        private static CoroutineMediator mMediator { get { return CoroutineMediator.instance; } }

        /// <summary>
        /// This Wait uses Update to calcualate time
        /// </summary>
        public static CountdownEvent Start(CountdownEvent pScheduleEvent)
        {
            return mMediator.WaitForSecond(pScheduleEvent);
        }

        public static ConditionEvent Start(ConditionalDelegate pTriggerCondition, Action pDoSomething)
        {
            return mMediator.WaitForCondition(new ConditionEvent() { doSomething = pDoSomething, triggerCondition = pTriggerCondition });
        }

        public static CountdownEvent Start(float pTime, Action<float> pDoSomething)
        {
            return mMediator.WaitForSecond(new CountdownEvent() { waitTime = pTime, doSomething = pDoSomething });
        }

        /// <summary>
        /// This Wait uses Update to calcualate time
        /// </summary>
        public static ConditionEvent Start(ConditionEvent pScheduleEvent)
        {
            return mMediator.WaitForCondition(pScheduleEvent);
        }

        public static void Enqueue(Action pDoSomething)
        {
            mMediator.Enqueue(pDoSomething);
        }

        public static ConditionEvent Start(Action pDoSomething, ConditionalDelegate pTriggerCondition)
        {
            return mMediator.WaitForCondition(new ConditionEvent() { doSomething = pDoSomething, triggerCondition = pTriggerCondition });
        }

        public static IUpdate AddUpdate(IUpdate pUpdate)
        {
            return mMediator.AddUpdate(pUpdate);
        }

        public static void RemoveTimeAction(int pId)
        {
            mMediator.RemoveTimeAction(pId);
        }

        public static void RemoveTriggerAction(int pId)
        {
            mMediator.RemoveTriggerAction(pId);
        }

        public static void RemoveTimeAction(CountdownEvent pEvent)
        {
            mMediator.RemoveTimeAction(pEvent);
        }

        public static void RemoveTriggerAction(ConditionEvent pEvent)
        {
            mMediator.RemoveTriggerAction(pEvent);
        }

        public static void RemoveUpdate(int pId)
        {
            mMediator.RemoveUpdate(pId);
        }

        public static void RemoveUpdate(IUpdate pUpdate)
        {
            mMediator.RemoveUpdate(pUpdate);
        }
    }
}
