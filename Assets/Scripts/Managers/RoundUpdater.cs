using System;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class RoundUpdater : MonoBehaviour
    {
        [Inject] private LevelStateManager _levelStateManager;
        
        [SerializeField] private PlayerData firstPlayer;
        [SerializeField] private PlayerData secondPlayer;

        [SerializeField] private StartPoint[] startPoints;

        private int _curStartIndex = 0;

        private void Start()
        {
            _levelStateManager.RoundUpdate += UpdatePlayers;
            
#if DEBUG
            if (startPoints.Length == 0) Debug.LogError("Must be 1 start points at least!");
#endif
        }

        public void UpdatePlayers()
        {
            _curStartIndex++;
            _curStartIndex %= startPoints.Length;

            firstPlayer.transform.position = startPoints[_curStartIndex].startPointFirstPlayer.position;
            firstPlayer.ResetPlayer();
            firstPlayer.ResetRigidbody();
            
            secondPlayer.transform.position = startPoints[_curStartIndex].startPointSecondPlayer.position;
            secondPlayer.ResetPlayer();
            secondPlayer.ResetRigidbody();
        }
    }

    [Serializable]
    public class StartPoint
    {
        public Transform startPointFirstPlayer;
        public Transform startPointSecondPlayer;
    }
}