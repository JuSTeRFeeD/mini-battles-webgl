using System;

namespace SaveData
{
    [Serializable]
    public class SaveData
    {
        public string selectedLocale;
        public bool musicEnabled;
        public bool sfxEnabled;
        public int coins;
        public bool[] openedLevels;
        public bool[] openedSkins;
    }
}
