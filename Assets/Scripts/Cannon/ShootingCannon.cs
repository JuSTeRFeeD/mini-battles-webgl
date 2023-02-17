using Managers;
using UnityEngine;
using Zenject;

namespace Cannon
{
    public class ShootingCannon : MonoBehaviour
    {
        [Inject] private RespawnManager _respawnManager;
        [Inject] private LevelStateManager _levelStateManager;
        
        [SerializeField] private float shootSpeed = 5f;
        
        [Space] 
        [SerializeField] private bool isRotating;
        [SerializeField] private float rotateDelay = 0.25f;
        [SerializeField] private float rotateAngle = 45f;
        private float _nextRotateTime;
        private float _curRotationZ;
        
        [Space]
        [SerializeField] private Animator animator;
        [SerializeField] private ParticleSystem shootVfx;
        private PlayerData _playerInCannon;

        [Space] 
        [SerializeField] private AudioSource audioSource;
        
        private static readonly int ShootAnim = Animator.StringToHash("shoot");
        private static readonly int ResizeAnim = Animator.StringToHash("resize");

        private void Start()
        {
            _nextRotateTime = Time.time + rotateDelay;
            _curRotationZ = transform.localRotation.z;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.CompareTag("Player")) return;
            if (_playerInCannon != null)
            {
                _respawnManager.ToRespawnPlayer(col.GetComponent<PlayerData>());
                return;
            }

            animator.SetTrigger(ResizeAnim);
            
            _playerInCannon = col.GetComponent<PlayerData>();
            _playerInCannon.Rb.velocity = Vector2.zero;
            _playerInCannon.transform.position = transform.position;
            _playerInCannon.PlayerControls.SingleButtonPressed += Shoot;
            RotatePlayer();
        }

        private void Shoot()
        {
            if (_levelStateManager.IsGamePaused) return;
            
            audioSource.Play();
            shootVfx.Play();
            animator.SetTrigger(ShootAnim);
            _playerInCannon.PlayerControls.SingleButtonPressed -= Shoot;
            _playerInCannon.Rb.velocity = transform.up * shootSpeed;
            _playerInCannon = null;
        }

        private void FixedUpdate()
        {
            if (_levelStateManager.IsGamePaused) return;
            
            if (!isRotating || Time.time < _nextRotateTime) return;
            _nextRotateTime = Time.time + rotateDelay;
            _curRotationZ += rotateAngle;
            transform.localRotation = Quaternion.Euler(0, 0, _curRotationZ);
            RotatePlayer();
        }

        private void RotatePlayer()
        {
            if (!_playerInCannon) return;
            _playerInCannon.transform.rotation = transform.rotation;
        }
    }
}
