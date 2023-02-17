using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using Zenject;

namespace MainMenu
{
    public class MenuPlayersScore : MonoBehaviour
    {
        [Inject] private PlayersScore _playersScore;

        [SerializeField] private TextMeshProUGUI coinsAmount;
        [SerializeField] private TextMeshProUGUI firstPlayerScore;
        [SerializeField] private TextMeshProUGUI secondPlayerScore;

        private Sequence _sequence;
        private Sequence _sequence1;
        
        private void Start()
        {
            firstPlayerScore.text = _playersScore.FirstPlayerScore.ToString();
            secondPlayerScore.text = _playersScore.SecondPlayerScore.ToString();

            _sequence = DOTween.Sequence()
                    .Append(firstPlayerScore.transform.DOShakePosition(1f, Vector3.up * 10))
                    .SetDelay(4f)
                    .SetLoops(-1);
            _sequence1 = DOTween.Sequence()
                    .Append(secondPlayerScore.transform.DOShakePosition(1f, Vector3.up * 10))
                    .SetDelay(4f)
                    .SetLoops(-1);
            
            UpdateCoins();
            _playersScore.CoinsUpdate += UpdateCoins;
        }

        private void UpdateCoins()
        {
            coinsAmount.text = _playersScore.Coins.ToString();
        }

        private void OnDestroy()
        {
            _sequence.Kill();
            _sequence1.Kill();
        }
    }
}
