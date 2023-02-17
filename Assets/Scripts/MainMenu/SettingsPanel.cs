using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace MainMenu
{
    public class SettingsPanel : MonoBehaviour
    {
        [SerializeField] private Color32 activeColor;
        [SerializeField] private Color32 disabledColor;

        [Space] 
        [SerializeField] private Image musicBgImg;
        [SerializeField] private Image sfxBgImg;
        [SerializeField] private float musicVolumeBond = -20f; 
        [SerializeField] private float sfxVolumeBond = -10f; 
        [Space]
        [SerializeField] private AudioMixerGroup mixer;

        private const float DisabledVolumeBond = -80f;
        private bool _isActiveMusic = true;
        private bool _isActiveSfx = true;
        
        public void SwitchMusicVolume()
        {
            _isActiveMusic = !_isActiveMusic;
            musicBgImg.color = _isActiveMusic ? activeColor : disabledColor;
            mixer.audioMixer.SetFloat("Music", _isActiveMusic ? musicVolumeBond : DisabledVolumeBond);
        }
        public void SwitchSfxVolume()
        {
            _isActiveSfx = !_isActiveSfx;
            sfxBgImg.color = _isActiveSfx ? activeColor : disabledColor;
            mixer.audioMixer.SetFloat("SFX", _isActiveSfx ? sfxVolumeBond : DisabledVolumeBond);
        }

        public void SetLocale(int localeId)
        {
            if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[localeId]) 
                return;
            StartCoroutine(nameof(ChangeLocale), localeId);
        }

        private IEnumerator ChangeLocale(int localeId)
        {
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];
        }
    }
}
