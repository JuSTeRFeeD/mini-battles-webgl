using DG.Tweening;
using UnityEngine;

namespace Platformer
{
    public class GravityChanger : MonoBehaviour
    {
        [SerializeField] private Transform sprite;

        private Sequence _sequence;
        
        private void Start()
        {
            _sequence = DOTween.Sequence()
                .Append(sprite.DORotate(new Vector3(180, 0), 1.5f))
                .Append(sprite.DORotate(new Vector3(0, 0), 1.5f))
                .SetDelay(0.25f)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1)
                .Play();
        }

        private void OnDestroy()
        {
            _sequence.Kill();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.CompareTag("Player")) return;
            var player = col.GetComponent<PlayerData>();
            player.Rb.gravityScale = -player.Rb.gravityScale;
            player.FlipSpriteY(player.Rb.gravityScale < 0);
        }
    }
}
