using DG.Tweening;
using Managers;
using UnityEngine;
using Zenject;

public class SavePoint : MonoBehaviour
{
    [Inject] private RespawnManager _respawnManager;

    [SerializeField] private Transform sprite;
    [SerializeField] private Transform respawnPoint;

    private Sequence _sequence;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        _respawnManager.UpdateRespawnPoint(col.GetComponent<PlayerData>(), respawnPoint);
        
        if (sprite == null) return;
        _sequence?.Kill();
        _sequence = DOTween.Sequence();
        _sequence
            .Append(sprite.transform.DOScale(Vector3.one * 1.5f, 0.25f))
            .Append(sprite.transform.DOScale(Vector3.one, 0.25f))
            .Play();
    }
}
