using System;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

namespace Yandex
{
    public class YandexSDK : MonoBehaviour
    {
        private void Start() => transform.parent = null;

        #region Variables
        // TEST
        public TextMeshProUGUI deviceType;
        // TEST
        public YandexDeviceType CurrentDeviceType { get; private set; } =
            #if DEBUG
                YandexDeviceType.Mobile;
            #else 
                YandexDeviceType.Desktop;
            #endif
        
#endregion
        
#region __Internal
    
        [DllImport("__Internal")]
        private static extern void ShowFullScreenAd();
        
        [DllImport("__Internal")]
        private static extern void GetPlatformDevice();
        
#endregion

#region Unity Methods

        public void ShowAd() => ShowFullScreenAd();
            
        public void GetPlatformDeviceType() => GetPlatformDevice();
        
#endregion

#region JS methods

        public void SetTargetDeviceType(string typeDevice)
        {
            CurrentDeviceType = typeDevice switch
            {
                "desktop" => YandexDeviceType.Desktop, 
                "mobile" => YandexDeviceType.Mobile,
                _ => YandexDeviceType.Mobile
            };
                    
            // todo del
            deviceType.text = "Device: " + CurrentDeviceType;
        }
#endregion
    }
}