using Managers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Archers
{
    public class ArcherBowController : MonoBehaviour
    {
        [Inject] private LevelStateManager _levelStateManager;
        
        [SerializeField] private PlayerControls controls;
        [Space]
        [SerializeField] private Transform target;
        [SerializeField] private Transform hands;
        [SerializeField] private Transform shootPos;
        [SerializeField] private PhysicsProjectile projectile;
        
        [Space]
        [SerializeField] private float angleDiff = 90;
        [SerializeField] private float rotationSpeed = 5;
        [Space]
        [SerializeField] private Image speedFillBar;
        [SerializeField, Range(1f, 25f)] private float changeSpeed = 1;
        [SerializeField] private float minSpeed = 2;
        [SerializeField] private float maxSpeed = 15;
        private float _shootSpeed;
        private bool _isMovingToMaxSpeed;

        private bool _isMovingToUpper;
        private float _upperAngle;
        private float _lowerAngle;
        private float _angleBetween;
        private float _angle;

        private bool _isLockedPos;

        private void Start()
        {
            controls.SingleButtonPressed += HandleInput;
            
            GetAngleBetween();
            hands.localRotation = Quaternion.Euler(0, 0, _angleBetween);
        }

        private void Update()
        {
            if (_levelStateManager.IsGamePaused) return;
            if (!_isLockedPos) UpdateCurAngle();
            else UpdateShootSpeed();
        }
        
        private void FixedUpdate()
        {
            if (_levelStateManager.IsGamePaused) return;
            GetAngleBetween();
            speedFillBar.fillAmount = (_shootSpeed - minSpeed) / (maxSpeed - minSpeed);
        }

        private void HandleInput()
        {
            if (_levelStateManager.IsGamePaused) return;
            if (!_isLockedPos)
            {
                _isLockedPos = true;
            }
            else
            {
                Shoot();
                _shootSpeed = minSpeed;
                _isLockedPos = false;
            }
        }

        private void Shoot()
        {
            var arrow = Instantiate(projectile, shootPos.position, Quaternion.Euler(0, 0, _angle));
            arrow.SetParams(hands.right, _shootSpeed);
        }

        private void GetAngleBetween()
        {
            var diff = target.position - transform.position;
            _angleBetween = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            _upperAngle = _angleBetween + angleDiff;
            _lowerAngle = _angleBetween - angleDiff;
        }
        
        private void UpdateCurAngle()
        {
            hands.localRotation = Quaternion.Euler(0, 0, _angle);
            if (_isMovingToUpper)
            {
                _angle += Time.deltaTime * rotationSpeed;
                if (_angle < _upperAngle) return;
                _isMovingToUpper = !_isMovingToUpper;
                _angle = _upperAngle;
                return;
            }
            _angle -= Time.deltaTime * rotationSpeed;
            if (_angle > _lowerAngle) return;
            _isMovingToUpper = !_isMovingToUpper;
            _angle = _lowerAngle;
        }

        private void UpdateShootSpeed()
        {
            if (_isMovingToMaxSpeed)
            {
                _shootSpeed += Time.deltaTime * changeSpeed;
                if (_shootSpeed < maxSpeed) return;
                _shootSpeed = maxSpeed;
                _isMovingToMaxSpeed = false;
                return;
            }
            
            _shootSpeed -= Time.deltaTime * changeSpeed;
            if (_shootSpeed > minSpeed) return;
            _shootSpeed = minSpeed;
            _isMovingToMaxSpeed = true;
        } 
    }
}
