using System;
using System.Collections;
using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using Zenject;

namespace Managers
{
    public class LevelStateManager : MonoBehaviour
    {
        [Inject] private PlayersScore _playersScore;
        [Inject] private SceneLoader _sceneLoader;
        
        [Header("Level params")] 
        [SerializeField, Min(0)] private int scoreToWin = 1;
        private int _firstPlayerWins = 0;
        private int _secondPlayerWins = 0;
        public UnityAction RoundUpdate;

        [SerializeField] private TextMeshProUGUI announcerText;
        private readonly  Vector3 _minAnnouncerScale = new (0.5f, 0.5f, 1);
        private readonly Vector3 _maxAnnouncerScale = new (4, 4, 1);
        
        [Header("Sounds")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip tickBeforeStart;

        [Header("Localization")]
        [SerializeField] private LocalizedString goLocal;
        [SerializeField] private LocalizedString playerLocal;
        [SerializeField] private LocalizedString roundWinLocal;
        [SerializeField] private LocalizedString scoreLocal;
        [SerializeField] private LocalizedString winLocal;

        private bool _isGamePaused;
        public bool IsGamePaused {
            get => _isGamePaused;
            private set
            {
                OnGamePauseChange?.Invoke(value);
                _isGamePaused = value;
            }
        }

        public UnityAction<bool> OnGamePauseChange;
        
        private Sequence _sequence;

        private void Start()
        {
            IsGamePaused = true;
            StartTimer();
        }
        
        private void OnDestroy()
        {
            _sequence.Kill();
        }

        private void StartTimer()
        {
            StartCoroutine(nameof(StartTimerAnnouncer));
        }

        // before level start
        private IEnumerator StartTimerAnnouncer()
        {
            announcerText.transform.DOScale(Vector3.zero, 0);

            var delay = new WaitForSeconds(1);
            yield return delay;
            
            _sequence = DOTween.Sequence()
                .Append(announcerText.transform.DOScale(_maxAnnouncerScale, .1f))
                .Append(announcerText.transform.DOScale(_minAnnouncerScale, .9f))
                .SetLoops(3)
                ;

            for (var i = 3; i > 0; i--)
            {
                announcerText.text = i.ToString();
                audioSource.PlayOneShot(tickBeforeStart);
                yield return delay;
            }

            _sequence = DOTween.Sequence()
                .Append(announcerText.transform.DOScale(_maxAnnouncerScale, .1f))
                .Append(announcerText.transform.DOScale(Vector3.zero, .9f));

            announcerText.text = $"<color=yellow>{goLocal.GetLocalizedString()}!";
            audioSource.pitch = 1.5f;
            audioSource.PlayOneShot(tickBeforeStart);
            
            yield return delay;
            IsGamePaused = false;
            _sequence.Kill();
        }

        /// Score after player win
        private IEnumerator PlayerWinAnnouncer(PlayerNum playerNum)
        {
            audioSource.PlayOneShot(tickBeforeStart);

            var score = playerNum == PlayerNum.Player1 ? _firstPlayerWins : _secondPlayerWins;
            
            var sb = new StringBuilder();
            sb.Append($"<size=50%>{playerLocal.GetLocalizedString()} {(int)(playerNum + 1)}\n");
            sb.Append($"<size=75%>{roundWinLocal.GetLocalizedString()}\n");
            if (scoreToWin != 1)
            {
                sb.Append($"<size=25%>{scoreLocal.GetLocalizedString()} {score}/{scoreToWin}");
            }

            announcerText.text = sb.ToString();
            announcerText.transform.localScale = _maxAnnouncerScale * 4f;
            
            _sequence = DOTween.Sequence()
                .Append(announcerText.transform.DOScale(_maxAnnouncerScale, .5f))
                .SetEase(Ease.InOutExpo);
            
            yield return new WaitForSeconds(2f);
            
            _sequence.Kill();
            CheckWin();
        }
        
        private IEnumerator EndGameAnnouncer(PlayerNum playerNum)
        {
            audioSource.PlayOneShot(tickBeforeStart);

            var sb = new StringBuilder();
            sb.Append($"<size=50%>{playerLocal.GetLocalizedString()} {(int)(playerNum + 1)}\n");
            sb.Append($"<size=100%>{winLocal.GetLocalizedString()}\n");

            announcerText.text = sb.ToString();
            announcerText.transform.localScale = _maxAnnouncerScale * 4f;
            
            _sequence = DOTween.Sequence()
                .Append(announcerText.transform.DOScale(_maxAnnouncerScale, .5f))
                .SetEase(Ease.InOutExpo);
            
            yield return new WaitForSeconds(5f);
            
            ToMainMenu();
        }
        
        public void PlayerFinished(PlayerData playerData)
        {
            playerData.ResetRigidbody();
            if (IsGamePaused) return;
            IsGamePaused = true;
            switch (playerData.PlayerNum)
            {
                case PlayerNum.Player1:
                    _firstPlayerWins++;
                    break;
                case PlayerNum.Player2:
                    _secondPlayerWins++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            StartCoroutine(nameof(PlayerWinAnnouncer), playerData.PlayerNum);
        }

        private void CheckWin()
        {
            if (_firstPlayerWins >= scoreToWin)
            {
                _playersScore.AddScoreToPlayer(PlayerNum.Player1);
                EndGame(PlayerNum.Player1);
                return;
            }
            if (_secondPlayerWins >= scoreToWin)
            {
                _playersScore.AddScoreToPlayer(PlayerNum.Player2);
                EndGame(PlayerNum.Player2);
                return;
            }
            RoundUpdate?.Invoke();
            StartCoroutine(nameof(StartTimerAnnouncer));
        }

        private void EndGame(PlayerNum playerNum)
        {
            _playersScore.AddCoins(GlobalConstants.CoinsPerGame);
            StartCoroutine(nameof(EndGameAnnouncer), playerNum);
        }
    
        public void ToMainMenu()
        {
            _sceneLoader.LoadScene("MainMenu");
        }
    }
}
