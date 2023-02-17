using Managers;
using UnityEngine;
using Zenject;

public class DeathZone : MonoBehaviour
{
    [Inject] private RespawnManager _respawnManager;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        _respawnManager.ToRespawnPlayer(col.GetComponent<PlayerData>());
    }
}
