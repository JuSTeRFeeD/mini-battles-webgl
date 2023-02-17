using System;
using UnityEngine.Events;

namespace Managers
{
    public class PlayersScore
    {
        public int Coins { get; private set; }
        public int FirstPlayerScore { get; private set; }
        public int SecondPlayerScore { get; private set; }

        public UnityAction CoinsUpdate;

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
        }

        public bool SpendCoins(int amount)
        {
            if (amount > Coins) return false;
            Coins -= amount;
            CoinsUpdate?.Invoke();
            return true;
        }
    }
}
