/**
 * Author NBear - nbhung71711 @gmail.com - 2017
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utilities.Common
{
    public static class TransformExtension
    {
        // Since transforms return their position as a property,
        // you can't set the x/y/z values directly, so you have to
        // store a temporary Vector3
        // Or you can use these methods instead
        public static void SetX(this Transform transform, float x)
        {
            var pos = transform.position;
            pos.x = x;
            transform.position = pos;
        }

        public static void SetY(this Transform transform, float y)
        {
            var pos = transform.position;
            pos.y = y;
            transform.position = pos;
        }

        public static void SetZ(this Transform transform, float z)
        {
            var pos = transform.position;
            pos.z = z;
            transform.position = pos;
        }

        public static void SetScaleX(this Transform transform, float x)
        {
            var scale = transform.localScale;
            scale.x = x;
            transform.localScale = scale;
        }

        public static void SetScaleY(this Transform transform, float y)
        {
            var scale = transform.localScale;
            scale.y = y;
            transform.localScale = scale;
        }

        public static void SetScaleZ(this Transform transform, float z)
        {
            var scale = transform.localScale;
            scale.z = z;
            transform.localScale = scale;
        }

        public static void FlipX(this Transform transform)
        {
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        public static void FlipX(this Transform transform, int direction)
        {
            if (direction > 0)
                direction = 1;
            else
                direction = -1;

            var scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * direction;
            transform.localScale = scale;
        }

        public static void FlipY(this Transform transform)
        {
            var scale = transform.localScale;
            scale.y *= -1;
            transform.localScale = scale;
        }

        public static void FlipY(this Transform transform, int direction)
        {
            if (direction > 0)
                direction = 1;
            else
                direction = -1;

            var scale = transform.localScale;
            scale.y = Mathf.Abs(scale.x) * direction;
            transform.localScale = scale;
        }

        public static void Reset(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;
        }

        public static List<Transform> GetChildren(this Transform transform)
        {
            var children = new List<Transform>();

            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                children.Add(child);
            }

            return children;
        }

        /// <summary>
        /// Sort children by conditions
        /// E.g: transform.Sort(t => t.name);
        /// </summary>
        public static void Sort(this Transform transform, Func<Transform, IComparable> sortFunction)
        {
            var children = transform.GetChildren();
            var sortedChildren = children.OrderBy(sortFunction).ToList();

            for (int i = 0; i < sortedChildren.Count(); i++)
            {
                sortedChildren[i].SetSiblingIndex(i);
            }
        }

        public static IEnumerable<Transform> GetAllChildren(this Transform transform)
        {
            var openList = new Queue<Transform>();

            openList.Enqueue(transform);

            while (openList.Any())
            {
                var currentChild = openList.Dequeue();

                yield return currentChild;

                var children = transform.GetChildren();

                foreach (var child in children)
                {
                    openList.Enqueue(child);
                }
            }
        }

        #region RectTransfrom

        public static void SetX(this RectTransform transform, float x)
        {
            var pos = transform.anchoredPosition;
            pos.x = x;
            transform.anchoredPosition = pos;
        }

        public static void SetY(this RectTransform transform, float y)
        {
            var pos = transform.anchoredPosition;
            pos.y = y;
            transform.anchoredPosition = pos;
        }

        public static Vector2 TopLeft(this RectTransform rect)
        {
            var pivot = rect.pivot;
            var x = rect.anchoredPosition.x - rect.rect.width * pivot.x;
            var y = rect.anchoredPosition.y + rect.rect.height * (1 - pivot.y);
            return new Vector2(x, y);
        }

        public static Vector2 TopRight(this RectTransform rect)
        {
            var pivot = rect.pivot;
            var x = rect.anchoredPosition.x + rect.rect.width * (1 - pivot.x);
            var y = rect.anchoredPosition.y + rect.rect.height * (1 - pivot.y);
            return new Vector2(x, y);
        }

        public static Vector2 BotLeft(this RectTransform rect)
        {
            var pivot = rect.pivot;
            var x = rect.anchoredPosition.x - rect.rect.width * pivot.x;
            var y = rect.anchoredPosition.y - rect.rect.height * pivot.y;
            return new Vector2(x, y);
        }

        public static Vector2 BotRight(this RectTransform rect)
        {
            var pivot = rect.pivot;
            var x = rect.anchoredPosition.x + rect.rect.width * (1 - pivot.x);
            var y = rect.anchoredPosition.y - rect.rect.height * pivot.y;
            return new Vector2(x, y);
        }

        public static Vector2 Center(this RectTransform rect)
        {
            var pivot = rect.pivot;
            var x = rect.anchoredPosition.x - rect.rect.width * pivot.x + rect.rect.width / 2f;
            var y = rect.anchoredPosition.y - rect.rect.height * pivot.y + rect.rect.height / 2f;
            return new Vector2(x, y);
        }

        public static Bounds Bounds(this RectTransform rect)
        {
            var size = new Vector2(rect.rect.width, rect.rect.height);
            return new Bounds(rect.Center(), size);
        }

        public static Vector3 WorldToCanvasPoint(this RectTransform mainCanvas, Vector3 worldPos)
        {
            Vector2 viewportPosition = Camera.main.WorldToViewportPoint(worldPos);
            Vector2 worldPosition = new Vector2(
            ((viewportPosition.x * mainCanvas.sizeDelta.x) - (mainCanvas.sizeDelta.x * 0.5f)),
            ((viewportPosition.y * mainCanvas.sizeDelta.y) - (mainCanvas.sizeDelta.y * 0.5f)));

            return worldPosition;
        }

        public static List<Vector2> GetScreenCoordinateOfUIRect(this RectTransform rt)
        {
            Vector3[] corners = new Vector3[4];
            rt.GetWorldCorners(corners);

            var output = new List<Vector2>();
            foreach (Vector3 item in corners)
            {
                var pos = RectTransformUtility.WorldToScreenPoint(null, item);
                output.Add(pos);
            }
            return output;
        }

        #endregion
    }
}