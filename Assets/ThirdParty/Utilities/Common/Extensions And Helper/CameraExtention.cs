/**
 * Author NBear - Nguyen Ba Hung - nbhung71711@gmail.com 
 **/

using UnityEngine;

namespace Utilities.Common
{
    public static class CameraHelper
    {
        /// <summary>
        /// In case we have two camera, one for UI and one for non-UI objects
        /// Convert a canvas object form UI camera to a world position in main camera
        /// Canvas Render Mode is Screen Space - Camera
        /// </summary>
        public static Vector3 ConvertToWorldCameraPosition(Camera pWorldCamera, Camera pUiCamera, Transform rt)
        {
            Vector2 screenPoint = pUiCamera.WorldToScreenPoint(rt.position);
            float screenRatioX = screenPoint.x / Screen.width;
            float screenRatioY = screenPoint.y / Screen.height;

            Vector2 mainCamSize = pWorldCamera.Size();
            Vector2 position = pWorldCamera.transform.position;
            position.x -= mainCamSize.x / 2;
            position.y -= mainCamSize.y / 2;

            float dx = mainCamSize.x * screenRatioX;
            float dy = mainCamSize.y * screenRatioY;

            position.x += dx;
            position.y += dy;

            return position;
        }

        public static Vector3 MouseScreenToWorldPoint(Camera camera = null)
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (camera != null)
                pos = camera.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            return pos;
        }

        /// <summary>
        /// If Render Mode of canvas is World Space - Overlay
        /// </summary>
        public static Vector3 WorldToCanvasPoint(Vector3 worldPos, RectTransform canvas)
        {
            Vector2 viewportPosition = Camera.main.WorldToViewportPoint(worldPos);
            Vector2 worldPosition = new Vector2(
            ((viewportPosition.x * canvas.sizeDelta.x) - (canvas.sizeDelta.x * 0.5f)),
            ((viewportPosition.y * canvas.sizeDelta.y) - (canvas.sizeDelta.y * 0.5f)));

            return worldPosition;
        }
    }

    public static class CameraExtension
    {
        public static Vector2 GetSize(this Camera camera)
        {
            float height = 2f * camera.orthographicSize;
            float width = height * camera.aspect;
            return new Vector2(width, height);
        }

        public static Vector3 MouseScreenToWorldPoint(this Camera camera)
        {
            var pos = camera.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            return pos;
        }

        public static Bounds OrthographicBounds(this Camera camera)
        {
            float screenAspect = (float)Screen.width / (float)Screen.height;
            float cameraHeight = camera.orthographicSize * 2;
            Bounds bounds = new Bounds(camera.transform.position, new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
            return bounds;
        }

        public static Vector2 Size(this Camera camera)
        {
            float height = camera.orthographicSize * 2;
            float width = height * camera.aspect;
            return new Vector2(width, height);
        }

        /// <summary>
        /// Convert screen point to world point
        /// </summary>
        public static Vector3 ConvertToWorldPosition(this Camera camera, Vector3 screenPoint)
        {
            float screenRatioX = screenPoint.x / Screen.width;
            float screenRatioY = screenPoint.y / Screen.height;

            Vector2 camSize = camera.Size();
            Vector2 position = camera.transform.position;
            position.x -= camSize.x / 2;
            position.y -= camSize.y / 2;

            float dx = camSize.x * screenRatioX;
            float dy = camSize.y * screenRatioY;

            position.x += dx;
            position.y += dy;

            return position;
        }

        /// <summary>
        /// Convert world position to a coresponding position in camera
        /// </summary>
        public static Vector3 ConvertToWorldPosition(this Camera camera, Transform rt)
        {
            Vector2 screenPoint = camera.WorldToScreenPoint(rt.position);
            float screenRatioX = screenPoint.x / Screen.width;
            float screenRatioY = screenPoint.y / Screen.height;

            Vector2 camSize = camera.Size();
            Vector2 position = camera.transform.position;
            position.x -= camSize.x / 2;
            position.y -= camSize.y / 2;

            float dx = camSize.x * screenRatioX;
            float dy = camSize.y * screenRatioY;

            position.x += dx;
            position.y += dy;

            return position;
        }

        public static bool InsideCamera(this Camera pCamera, Vector3 pWorldPosition)
        {
            float screenAspect = (float)Screen.width / (float)Screen.height;
            float cameraHeight = pCamera.orthographicSize * 2;
            Bounds bounds = new Bounds(pCamera.transform.position, new Vector3(cameraHeight * screenAspect, cameraHeight, 0));

            if (pWorldPosition.x < bounds.min.x)
                return false;
            if (pWorldPosition.x > bounds.max.x)
                return false;
            if (pWorldPosition.y < bounds.min.y)
                return false;
            if (pWorldPosition.y > bounds.max.y)
                return false;
            return true;
        }
    }
}