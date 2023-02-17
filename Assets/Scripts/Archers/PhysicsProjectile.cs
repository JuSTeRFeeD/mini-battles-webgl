using UnityEngine;

namespace Archers
{
    public class PhysicsProjectile : MonoBehaviour
    {
        [SerializeField] private CircleCollider2D solidCollider;
        [SerializeField] private CircleCollider2D triggerCollider;
        [SerializeField] private Rigidbody2D rb;

        private bool _isStopped;
        private int _damage = 1;
        
        public void SetParams(Vector2 dir, float speed, int damage = 1)
        {
            rb.velocity = dir * speed;
            _damage = damage;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Projectile")) return;
            MakeStatic(col.transform);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (_isStopped || col.CompareTag("Projectile") || !col.TryGetComponent<Health>(out var health)) return;
            health.TakeDamage(_damage);
            MakeStatic(health.transform);
        }
        
        private void MakeStatic(Transform parent)
        {
            _isStopped = true;
            solidCollider.enabled = false;
            triggerCollider.enabled = false;
            rb.bodyType = RigidbodyType2D.Static;
            transform.SetParent(parent);
        }

        private void Update()
        {
            if (_isStopped) return;
            var diff = rb.velocity - (Vector2)transform.position.normalized;
            var angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
