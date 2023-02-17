using Managers;
using UnityEngine;
using Zenject;

public class FinishChecker : MonoBehaviour
{
    [Inject] private LevelStateManager _levelStateManager;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_levelStateManager.IsGamePaused || !col.CompareTag("Player")) return;
        _levelStateManager.PlayerFinished(col.GetComponent<PlayerData>());
    }
}
