using Managers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MainMenu
{
    public class SettingsPanel : MonoBehaviour
    {
        [Inject] private SettingsManager _settingsManager;
        
        [SerializeField] private Color32 activeColor;
        [SerializeField] private Color32 disabledColor;

        [Space] 
        [SerializeField] private Image musicBgImg;
        [SerializeField] private Image sfxBgImg;

        private void OnEnable()
        {
            musicBgImg.color = SettingsManager.IsActiveMusic() ? activeColor : disabledColor;
            sfxBgImg.color = SettingsManager.IsActiveSfx() ? activeColor : disabledColor;
        }
        
        public void SwitchMusicVolume()
        {
            _settingsManager.SetMusicActive(!SettingsManager.IsActiveMusic());
            musicBgImg.color = SettingsManager.IsActiveMusic() ? activeColor : disabledColor;
        }
        public void SwitchSfxVolume()
        {
            _settingsManager.SetSfxActive(!SettingsManager.IsActiveSfx());
            sfxBgImg.color = SettingsManager.IsActiveSfx() ? activeColor : disabledColor;
        }

        public void SetLocale(int localeId)
        {
            _settingsManager.SetLocale(localeId);
        }

        public void SaveSettings()
        {
            _settingsManager.Save();
        }
    }
}
