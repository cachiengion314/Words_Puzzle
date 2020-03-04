using System;
using UnityEngine.Events;

namespace Utilities.Common
{
    public static class EventExtension
    {
        public static void Raise(this Action pAction)
        {
            if (pAction != null) pAction();
        }

        public static void Raise(this UnityAction pAction)
        {
            if (pAction != null) pAction();
        }

        public static void Raise<T>(this Action<T> pAction, T pParam)
        {
            if (pAction != null) pAction(pParam);
        }

        public static void Raise<T>(this UnityAction<T> pAction, T pParam)
        {
            if (pAction != null) pAction(pParam);
        }

        public static void Raise<T, M>(this Action<T, M> pAction, T pParam1, M pParam2)
        {
            if (pAction != null) pAction(pParam1, pParam2);
        }

        public static void Raise<T, M>(this UnityAction<T, M> pAction, T pParam1, M pParam2)
        {
            if (pAction != null) pAction(pParam1, pParam2);
        }
    }
}