/**
 * Author NBear - nbhung71711@gmail.com - 2017
 **/

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Utilities.Common;

namespace Utilities.Components
{
    public class SceneLoader
    {
        public static AsyncOperation LoadScene(string pScene, LoadSceneMode pLoadMode, bool pAutoActive, UnityAction<float> pOnProgress, UnityAction pOnCompleted, float pMinTime = 0.5f)
        {
            if (IsSceneLoaded(pScene))
            {
                pOnProgress.Raise(1);
                pOnCompleted.Raise();
                return null;
            }

            var sceneOperator = SceneManager.LoadSceneAsync(pScene, pLoadMode);
            sceneOperator.allowSceneActivation = false;
            CoroutineUtil.StartCoroutine(IEProcessOperation(sceneOperator, pAutoActive, pOnProgress, pOnCompleted, pMinTime));
            return sceneOperator;
        }

        private static IEnumerator IEProcessOperation(AsyncOperation sceneOperator, bool pAutoActive, UnityAction<float> pOnProgress, UnityAction pOnCompleted, float pMinTime = 0.5f)
        {
            pOnProgress.Raise(0f);

            float startTime = Time.unscaledTime;

            float progress = 0;
            while (true)
            {
                progress = Mathf.Clamp01(sceneOperator.progress / 0.9f);
                pOnProgress.Raise(progress);
                yield return null;

                if (sceneOperator.isDone || progress >= 1)
                    break;
            }

            float loadTime = Time.unscaledTime - startTime;
            if (loadTime < pMinTime)
                yield return new WaitForSeconds(pMinTime - loadTime);

            if (pAutoActive)
                sceneOperator.allowSceneActivation = true;

            pOnCompleted.Raise();
        }

        public static AsyncOperation UnloadScene(string pScene, UnityAction<float> pOnProgress, UnityAction pOnComplted)
        {
            if (!IsSceneLoaded(pScene))
            {
                pOnProgress(1f);
                return null;
            }

            var sceneOperator = SceneManager.UnloadSceneAsync(pScene);
            CoroutineUtil.StartCoroutine(IEProcessOperation(sceneOperator, false, pOnProgress, pOnComplted));
            return sceneOperator;
        }

        private static bool IsSceneLoaded(string pSceneName)
        {
            for (int i = 0; i < SceneManager.sceneCount; ++i)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name == pSceneName)
                    return true;
            }

            return false;
        }

        public static AsyncOperation ReloadScene(string pScene, LoadSceneMode pLoadMode, bool pAutoActive, UnityAction<float> pOnProgress, UnityAction pOnCompleted)
        {
            var sceneOperator = SceneManager.LoadSceneAsync(pScene, pLoadMode);
            sceneOperator.allowSceneActivation = false;
            CoroutineUtil.StartCoroutine(IEProcessOperation(sceneOperator, pAutoActive, pOnProgress, pOnCompleted));
            return sceneOperator;
        }
    }
}