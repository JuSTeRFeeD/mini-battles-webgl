using UnityEngine;
using UnityEngine.Events;

namespace Archers
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int maxHealth;
        public int MaxHealth => maxHealth;
        [Space]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip hitSound;
        public int CurHealth { get; private set; }

        public UnityAction HealthUpdate;
        public UnityEvent onDeath;
        public UnityEvent<Health> onTakeDamage;
        
        private void Start()
        {
            ResetHealth();
        }

        public void ResetHealth()
        {
            CurHealth = maxHealth;
            HealthUpdate?.Invoke();
        }
        
        public void TakeDamage(int value)
        {
            if (CurHealth <= 0) return;
            
            CurHealth -= value;
            
            audioSource.PlayOneShot(hitSound);
            HealthUpdate?.Invoke();
            onTakeDamage?.Invoke(this);
            
            if (CurHealth <= 0)
            {
                onDeath?.Invoke();
            }
        }
    }
}
