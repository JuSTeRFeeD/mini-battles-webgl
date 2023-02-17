using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class RespawnManager : MonoBehaviour
    {
        private readonly Dictionary<PlayerData, Transform> _respawnPoints = new();
    
        public void UpdateRespawnPoint(PlayerData playerData, Transform newPoint)
        {
            if (_respawnPoints.ContainsKey(playerData))
            {
                _respawnPoints[playerData] = newPoint;
            }
            else
            {
                _respawnPoints.Add(playerData, newPoint);
            }
        }

        public void ToRespawnPlayer(PlayerData playerData)
        {
            playerData.PlayDeathEffects();
            playerData.ResetRigidbody();
            playerData.transform.position = _respawnPoints[playerData].position;
        }
    }
}
