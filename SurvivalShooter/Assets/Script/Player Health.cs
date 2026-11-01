using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public static readonly int IdDie = Animator.StringToHash("Die");

    public AudioClip hitClip;
    public AudioClip deathClip;
    private AudioSource playerAudioSource;
    private Animator playerAnimator;
    private PlayerMovement playerMovement;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerAudioSource = GetComponent<AudioSource>();
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerInput = GetComponent<PlayerInput>();
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsDead)
        {
            return;
        } 

        playerAudioSource.PlayOneShot(hitClip);
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    public override void Die()
    {
        base.Die();
        playerAnimator.SetTrigger(IdDie);
        playerAudioSource.PlayOneShot(deathClip);

        playerMovement.enabled = false;
        playerInput.enabled = false;

    }

}
