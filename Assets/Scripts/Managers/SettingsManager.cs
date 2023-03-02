using System.Collections;
using PlayerSave;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using Utils;

namespace Managers
{
    public class SettingsManager : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup mixer;
        [SerializeField] private float musicVolumeBond = -15f; 
        [SerializeField] private float sfxVolumeBond = -10f;
        
        private const float DisabledVolumeBond = -80f;
        private const float VolumeTransitionTime = .25f;

        private PlayerSaveData _oldPlayerSaveData;
        
        private void Awake()
        {
            _oldPlayerSaveData = SaveAndLoad.Data;
            SaveAndLoad.OnDataUpdate += SetupBySaveData;
        }

        private void Start()
        {
            mixer.audioMixer.SetFloat("SFX", DisabledVolumeBond);
            mixer.audioMixer.SetFloat("Music",DisabledVolumeBond);
        }

        public static bool IsActiveMusic() => SaveAndLoad.Data.musicEnabled;
        public static bool IsActiveSfx() => SaveAndLoad.Data.sfxEnabled;
        
        private void SetupBySaveData()
        {
            _oldPlayerSaveData = SaveAndLoad.Data;
            SetLocale(SaveAndLoad.Data.selectedLocale);
            SetMusicActive(IsActiveMusic());
            SetSfxActive(IsActiveSfx());
        }
        
        public void SetMusicActive(bool value)
        {
            SaveAndLoad.Data.musicEnabled = value;
            mixer.audioMixer.DoSetFloat("Music", value ? musicVolumeBond : DisabledVolumeBond, VolumeTransitionTime);
        }
        
        public void SetSfxActive(bool value)
        {
            SaveAndLoad.Data.sfxEnabled = value;
            mixer.audioMixer.DoSetFloat("SFX", value ? sfxVolumeBond : DisabledVolumeBond, VolumeTransitionTime);
        }
        
        public void SetLocale(int localeId)
        {
            if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[localeId]) 
                return;
            SaveAndLoad.Data.selectedLocale = localeId;
            StartCoroutine(nameof(ChangeLocale), localeId);
        }
        
        private IEnumerator ChangeLocale(int localeId)
        {
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];
        }

        public void Save()
        {
            var newData = SaveAndLoad.Data;
            if (_oldPlayerSaveData.sfxEnabled != newData.sfxEnabled ||
                _oldPlayerSaveData.musicEnabled != newData.musicEnabled ||
                _oldPlayerSaveData.selectedLocale != newData.selectedLocale
               )
            {
                SaveAndLoad.Save();
            }
        }
    }
}