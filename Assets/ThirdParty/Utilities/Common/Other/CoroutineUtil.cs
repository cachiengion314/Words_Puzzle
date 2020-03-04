/**
 * Author NBear - nbhung71711@gmail.com - 2018
 **/

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Utilities.Common
{
    public class CoroutineUtil
    {
        private static CoroutineMediator mMediator { get { return CoroutineMediator.instance; } }

        public static void Init()
        {
            mMediator.Init();
        }

        //====

        public static Coroutine StartCoroutine(IEnumerator pCoroutine)
        {
            return mMediator.StartCoroutine(pCoroutine);
        }

        public static void StopCoroutine(Coroutine pCoroutine)
        {
            if (pCoroutine != null)
                mMediator.StopCoroutine(pCoroutine);
        }

        //====

        public static IEnumerator WaitForRealSeconds(float time)
        {
            float start = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < start + time)
            {
                yield return null;
            }
        }

        //========

        /* OUT OF DATE METHODS
        private static List<KeyValuePair<int, Coroutine>> mListCoroutine = new List<KeyValuePair<int, Coroutine>>();

        public static void WaitUntilSecond(int pId, Action pAction, float pDelayStart, bool pIgnoreTimeScale = false)
        {
            CheckListCoroutine(pId);

            var cc = mMediator.StartCoroutine(iEWaitUntilSecond(pAction, pDelayStart, pIgnoreTimeScale));
            mListCoroutine.Add(new KeyValuePair<int, Coroutine>(pId, cc));
        }

        private static IEnumerator iEWaitUntilSecond(Action pAction, float pDelayStart, bool pIgnoreTimeScale = false)
        {
            if (!pIgnoreTimeScale)
                yield return new WaitForSeconds(pDelayStart);
            else
                yield return mMediator.StartCoroutine(WaitForRealSeconds(pDelayStart));

            pAction();
        }

        //=======

        public static void WaitUntilCondition(int pId, Action pAction, ConditionalDelegate pTriggerCondition)
        {
            CheckListCoroutine(pId);

            if (pTriggerCondition())
            {
                pAction();
                return;
            }

            var cc = mMediator.StartCoroutine(iEWaitUntilCondition(pAction, pTriggerCondition));
            mListCoroutine.Add(new KeyValuePair<int, Coroutine>(pId, cc));
        }

        private static IEnumerator iEWaitUntilCondition(Action pAction, ConditionalDelegate pTriggerCondition)
        {
            yield return new WaitUntil(() => pTriggerCondition());

            pAction();
        }

        //=======

        public static void Update(int pId, Action pAction, float pTime)
        {
            CheckListCoroutine(pId);

            var cc = mMediator.StartCoroutine(iEUpdate(pAction, pTime));
            mListCoroutine.Add(new KeyValuePair<int, Coroutine>(pId, cc));
        }

        private static IEnumerator iEUpdate(Action pAction, float pTime)
        {
            WaitForSeconds wait = new WaitForSeconds(pTime);

            while (true)
            {
                pAction();

                yield return wait;
            }
        }

        //=======

        private static void CheckListCoroutine(int pId)
        {
            foreach (var c in mListCoroutine)
            {
                if (c.Key == pId)
                {
                    Debug.Log("Stop coroutine " + c.Key);
                    StopCoroutine(c.Value);
                    mListCoroutine.Remove(c);
                    break;
                }
            }
        }
        */
    }

    public class CustomUpdate : IDisposable
    {
        public Action onFirstUpdate;
        public Action onLateUpdate;
        public ConditionalDelegate breakCondition;
        public Action onBreak;
        public float interval;
        public bool needDetail = true;
        public float timeOffset;

        private Coroutine mCoroutine;
        public float progress { get; private set; }
        public bool isWorking { get; private set; }

        public void Run()
        {
            if (needDetail)
                mCoroutine = CoroutineUtil.StartCoroutine(IE_RunDetail());
            else
                mCoroutine = CoroutineUtil.StartCoroutine(IE_RunSimple());
        }

        private IEnumerator IE_RunDetail()
        {
            while (true)
            {
                isWorking = true;

                if (onFirstUpdate != null)
                    onFirstUpdate();

                progress = 0;

                float elapsedTime = timeOffset;
                while (interval > 0)
                {
                    if (breakCondition != null && breakCondition())
                    {
                        if (onBreak != null) onBreak();
                        Kill();
                    }

                    yield return null;

                    elapsedTime += Time.deltaTime;
                    progress = elapsedTime / interval;

                    if (elapsedTime > interval)
                        break;
                }
                if (interval == 0)
                    yield return null;

                progress = 1f;

                if (onLateUpdate != null)
                    onLateUpdate();
            }
        }

        private IEnumerator IE_RunSimple()
        {
            while (true)
            {
                isWorking = true;

                if (onFirstUpdate != null)
                    onFirstUpdate();

                progress = 0;

                if (breakCondition != null && breakCondition())
                {
                    if (onBreak != null) onBreak();
                    Kill();
                }

                if (interval == 0)
                    yield return null;
                else
                    yield return new WaitForSeconds(interval);

                progress = 1f;

                if (breakCondition != null && breakCondition())
                {
                    if (onBreak != null) onBreak();
                    Kill();
                }

                if (onLateUpdate != null)
                    onLateUpdate();
            }
        }

        public void Kill()
        {
            if (mCoroutine != null)
                CoroutineUtil.StopCoroutine(mCoroutine);

            isWorking = false;
        }

        public void Dispose()
        {
            Kill();
        }
    }

    public class CustomWait : IDisposable
    {
        public Action onStart;
        public Action onDone;
        public ConditionalDelegate breakCondition;
        public Action onBreak;
        public float time;
        public float timeOffset;
        public bool needDetail = true;

        private Coroutine mCoroutine;
        public float progress { get; private set; }
        public bool isWorking { get; private set; }

        public void Run()
        {
            if (needDetail)
                mCoroutine = CoroutineUtil.StartCoroutine(IE_RunDetail());
            else
                mCoroutine = CoroutineUtil.StartCoroutine(IE_RunSimple());
        }

        private IEnumerator IE_RunDetail()
        {
            isWorking = true;

            if (onStart != null)
                onStart();

            progress = 0;

            float elapsedTime = timeOffset;
            while (true)
            {
                elapsedTime += Time.deltaTime;
                progress = elapsedTime / time;

                if (elapsedTime >= time)
                    break;

                if (breakCondition != null && breakCondition())
                    Kill();

                yield return null;
            }

            progress = 1f;

            isWorking = false;

            if (onDone != null)
                onDone();
        }

        private IEnumerator IE_RunSimple()
        {
            isWorking = true;

            if (onStart != null)
                onStart();

            progress = 0;

            if (breakCondition != null && breakCondition())
            {
                if (onBreak != null) onBreak();
                Kill();
            }

            if (time == 0)
                yield return null;
            else
                yield return new WaitForSeconds(time);

            if (breakCondition != null && breakCondition())
            {
                if (onBreak != null) onBreak();
                Kill();
            }

            progress = 1f;

            isWorking = false;

            if (onDone != null)
                onDone();
        }

        public void Kill()
        {
            if (mCoroutine != null)
                CoroutineUtil.StopCoroutine(mCoroutine);

            isWorking = false;
        }

        public void Dispose()
        {
            Kill();
        }
    }
}