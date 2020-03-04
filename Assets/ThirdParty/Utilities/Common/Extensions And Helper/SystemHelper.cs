using System;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Utilities.Common
{
    public class SystemHelper
    {
        private static string mDeviceUniqueID;
        private static string mProductEncode;

        public static string DeviceUniqueID
        {
            get
            {
                if (mDeviceUniqueID == null)
                {
                    string systemID = "";
#if UNITY_EDITOR
                    systemID = "EDITOR_" + SystemInfo.deviceUniqueIdentifier;
#elif UNITY_IPHONE
                    systemID = "IOS_" + SystemInfo.deviceUniqueIdentifier;
#elif UNITY_ANDROID
                    systemID = "ANDROID_" + SystemInfo.deviceUniqueIdentifier;
#else
                    systemID = "OTHER_" + SystemInfo.deviceUniqueIdentifier;
#endif
                    mDeviceUniqueID = systemID.Replace("-", "").ToLower();
                }
                return mDeviceUniqueID;
            }
        }

#if UNITY_EDITOR
        public static string ProductName
        {
            get
            {
                if (mProductEncode == null)
                {
                    string text = PlayerSettings.productName;
                    Encoding utf8 = Encoding.UTF8;
                    Byte[] encodedBytes = utf8.GetBytes(text);
                    Byte[] convertedBytes = Encoding.Convert(Encoding.UTF8, Encoding.ASCII, encodedBytes);
                    Encoding ascii = Encoding.ASCII;
                    mProductEncode = ascii.GetString(convertedBytes);
                }
                return mProductEncode;
            }
        }
#endif
    }
}
