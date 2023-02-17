using Managers;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

[RequireComponent(typeof(AudioSource))]
public class PlayerData : MonoBehaviour
{
    [Inject] private Skins _skins;
    
    [field: SerializeField] public PlayerControls PlayerControls { get; private set; }
    [field: SerializeField] public PlayerNum PlayerNum { get; private set; } = PlayerNum.Player1;
    [field:SerializeField] public Rigidbody2D Rb { get; private set; }
    [SerializeField] private bool needToResetGravityScale = true;

    [Space]
    [SerializeField] private ParticleSystem deathFvx;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip deathSound;
    [Space]
    [SerializeField] private SpriteRenderer skinRenderer;

    public void FlipSpriteY(bool value) => skinRenderer.flipY = value;
    public void FlipSpriteX(bool value) => skinRenderer.flipX = value;

    public UnityEvent onResetPlayer;
    
    public void ResetRigidbody()
    {
        skinRenderer.flipY = false;
        Rb.velocity = Vector2.zero;
        if (needToResetGravityScale) Rb.gravityScale = 1;
    }
    
    public void PlayDeathEffects()
    {
        deathFvx.Play();
        audioSource.PlayOneShot(deathSound);
    }

    public void ResetPlayer()
    {
        onResetPlayer?.Invoke();
    }
    
    private void Start()
    {
        skinRenderer.sprite = _skins.GetPlayerSkin(PlayerNum);
    }
}
