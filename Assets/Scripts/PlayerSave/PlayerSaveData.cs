namespace PlayerSave
{
    [System.Serializable]
    public class PlayerSaveData
    {
        public int selectedLocale;
        public bool musicEnabled;
        public bool sfxEnabled;
        
        public int coins;
        
        public bool[] openedSkins;
        public int firstPlayerSkin;
        public int secondPlayerSkin;
    }
}
