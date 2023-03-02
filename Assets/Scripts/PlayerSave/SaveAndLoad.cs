using System;
using Newtonsoft.Json;
using YandexProvider.Player;

namespace PlayerSave
{
    public static class SaveAndLoad
    {
        public static PlayerSaveData Data;
        public static Action OnDataUpdate;

        static SaveAndLoad()
        {
            Data = new PlayerSaveData
            {
                musicEnabled = true,
                sfxEnabled = true,
                firstPlayerSkin = 0,
                secondPlayerSkin = 1,
                selectedLocale = 0,
                openedSkins = new [] { true, true, true }
            };
        } 
        
        public static void Save()
        {
            Player.SetPlayerData(JsonConvert.SerializeObject(Data));
        }

        public static void Load(Action success, Action<string> onError = default)
        {
            Player.GetPlayerData((result) =>
            {
                success?.Invoke();
                if (!string.IsNullOrEmpty(result))
                {
                    Data = JsonConvert.DeserializeObject<PlayerSaveData>(result);
                }
                OnDataUpdate?.Invoke();
                OnDataUpdate = null;
            }, onError);
        }
    }
}