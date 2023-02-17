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
        [SerializeField] private Vector3 timerScale = new (0.5f, 0.5f, 1);
        private Vector3 _initAnnouncerScale;
        
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

        private void Start()
        {
            IsGamePaused = true;
            _initAnnouncerScale = announcerText.transform.localScale;
            StartTimer();
        }

        private void StartTimer()
        {
            StartCoroutine(nameof(StartTimerAnnouncer));
        }

        // before level start
        private IEnumerator StartTimerAnnouncer()
        {
            audioSource.pitch = 1f;
            var delay = new WaitForSeconds(1);
            yield return delay;
        
            announcerText.transform.localScale = _initAnnouncerScale;
            announcerText.transform.DOScale(timerScale, 1f).SetEase(Ease.InOutExpo);
            announcerText.text = "3";
            audioSource.PlayOneShot(tickBeforeStart);
            yield return delay;

            announcerText.transform.localScale = _initAnnouncerScale;
            announcerText.transform.DOScale(timerScale, 1f).SetEase(Ease.InOutExpo);;
            announcerText.text = "2";
            audioSource.PlayOneShot(tickBeforeStart);
            yield return delay;
        
            announcerText.transform.localScale = _initAnnouncerScale;
            announcerText.transform.DOScale(timerScale, 1f).SetEase(Ease.InOutExpo);;
            announcerText.text = "1";
            audioSource.PlayOneShot(tickBeforeStart);
            yield return delay;
        
            announcerText.transform.localScale = _initAnnouncerScale;
            announcerText.transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InOutExpo);;
            announcerText.text = $"<color=yellow>{goLocal.GetLocalizedString()}!";
            audioSource.pitch = 1.5f;
            audioSource.PlayOneShot(tickBeforeStart);
            IsGamePaused = false;
        }

        // score after player win
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
            announcerText.transform.localScale = _initAnnouncerScale * 4f;
            announcerText.transform.DOScale(_initAnnouncerScale, .5f).SetEase(Ease.InOutExpo);;
            yield return new WaitForSeconds(2f);
            CheckWin();
        }
        
        private IEnumerator EndGameAnnouncer(PlayerNum playerNum)
        {
            audioSource.PlayOneShot(tickBeforeStart);

            var sb = new StringBuilder();
            sb.Append($"<size=50%>{playerLocal.GetLocalizedString()} {(int)(playerNum + 1)}\n");
            sb.Append($"<size=100%>{winLocal.GetLocalizedString()}\n");

            announcerText.text = sb.ToString();
            announcerText.transform.localScale = _initAnnouncerScale * 4f;
            announcerText.transform.DOScale(_initAnnouncerScale, .5f).SetEase(Ease.InOutExpo);;
            yield return new WaitForSeconds(5f);
            TempToMainMenu();
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
    
        public void TempToMainMenu()
        {
            _sceneLoader.LoadScene("MainMenu");
        }
    }
}
