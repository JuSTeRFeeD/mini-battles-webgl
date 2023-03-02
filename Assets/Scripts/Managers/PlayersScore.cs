using UnityEngine.Events;
using System;
using PlayerSave;

namespace Managers
{
    public class PlayersScore
    {
        public int Coins { get; private set; }
        public int FirstPlayerScore { get; private set; }
        public int SecondPlayerScore { get; private set; }

        public UnityAction CoinsUpdate;

        public PlayersScore()
        {
            SaveAndLoad.OnDataUpdate += () => Coins = SaveAndLoad.Data.coins;
        }

        public void AddScoreToPlayer(PlayerNum playerNum)
        {
            switch (playerNum)
            {
                case PlayerNum.Player1:
                    FirstPlayerScore++;
                    break;
                case PlayerNum.Player2:
                    SecondPlayerScore++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(playerNum), playerNum, null);
            }
        }

        public void AddCoins(int amount)
        {
            Coins += amount;
            CoinsUpdate?.Invoke();
            SaveCoins();
        }

        public bool SpendCoins(int amount)
        {
            if (amount > Coins) return false;
            Coins -= amount;
            CoinsUpdate?.Invoke();
            SaveCoins();
            return true;
        }

        private void SaveCoins()
        {
            SaveAndLoad.Data.coins = Coins;
            SaveAndLoad.Save();
        }
    }
}
