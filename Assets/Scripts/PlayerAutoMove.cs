using Managers;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(PlayerData))]
public class PlayerAutoMove : MonoBehaviour
{
    [Inject] private LevelStateManager _levelStateManager;

    [Header("Ground check")] 
    [SerializeField] private bool isGroundedToJump = true;

    [Space] 
    [SerializeField, Tooltip("0 - without delay"), Min(0)] 
    private float jumpDelay;
    private float _nextJumpTime;
        
    [Space]
    [SerializeField] private Transform topGroundCheckPoint;
    [SerializeField] private Transform bottomGroundCheckPoint;
    [SerializeField] private Vector2 groundCheckSize;
    [SerializeField] private LayerMask groundMask = 1 << 0;
    
    [Header("Move settings")]
    [SerializeField] private PlayerData playerData;
    [SerializeField] private float jumpSpeed = 2f;
    [SerializeField] private float moveSpeed = 2f;
    private Rigidbody2D _rb;

    [SerializeField] private ParticleSystem jumpEffect;

    private void Start()
    {
        playerData.PlayerControls.SingleButtonPressed += Jump;
        _rb = playerData.Rb;

        _levelStateManager.OnGamePauseChange += PauseSwitch;
    }

    private void PauseSwitch(bool isGamePaused)
    {
        _rb.bodyType = isGamePaused ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
    }

    private void FixedUpdate()
    {
        if (_levelStateManager.IsGamePaused) return;
        _rb.velocity = new Vector2(moveSpeed, _rb.velocity.y);
    }

    private void Jump()
    {
        if (_levelStateManager.IsGamePaused) return;
        if (!CheckIsGrounded()) return;

        if (jumpDelay > 0)
        {
            if (Time.time < _nextJumpTime) return;
            _nextJumpTime = Time.time + jumpDelay;
        }
        
        _rb.velocity = new Vector2(_rb.velocity.x, jumpSpeed * _rb.gravityScale);
        jumpEffect.Play();
    }

    private bool CheckIsGrounded()
    {
        if (!isGroundedToJump) return true;
        switch (_rb.gravityScale)
        {
            case > 0 when Physics2D.OverlapBox(bottomGroundCheckPoint.position, groundCheckSize, 0, groundMask):
            case < 0 when Physics2D.OverlapBox(topGroundCheckPoint.position, groundCheckSize, 0, groundMask):
                return true;
            default:
                return false;
        }
    }
}
