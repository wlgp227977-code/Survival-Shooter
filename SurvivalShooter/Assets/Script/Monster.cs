using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Monster : LivingEntity
{
    public enum Status
    {
        Trace,
        Attack,
        Die,
    }

    public Transform target;
    public LayerMask targetLayer;

    public float traceDistance = 10f;
    public float attackDistance = 1f;

    public ParticleSystem hitEffect;
    public Collider monsterCollider;
    public Renderer monsterRenderer;
    public AudioClip hitClip;
    public AudioClip deathClip;
    private NavMeshAgent agent;
    private Animator monsterAnimator;
    private AudioSource monsterAudioSource;
    public MonsterData monsterdata;

    public float damage = 10f;
    private float lastAttackTime;
    public float attackInterval = 1f;
    private Status currentStatus;
    public static readonly int IdHasTarget = Animator.StringToHash("HasTarget");
    public static readonly int IdDie = Animator.StringToHash("Die");

    public Status CurrentStatus
    {
        get => currentStatus;
        set
        {
            currentStatus = value;
            Debug.Log(currentStatus);

            switch (currentStatus)
            {
                case Status.Trace:
                    break;
                case Status.Attack:
                    break;
                case Status.Die:
                    break;
            }
        }
    }

    public void Setup(MonsterData data)
    {
        startingHealth = data.maxHP;
        damage = data.damage;
        agent.speed = data.speed;
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        monsterAnimator = GetComponent<Animator>();
        monsterAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        switch (CurrentStatus)
        {
            case Status.Trace:
                break;
            case Status.Attack:
                break;
            case Status.Die:
                break;
        }
    }

    private float deactiveTime;
    private void UpdateDie()
    {

    }
    private void UpdateAttack()
    {
        if (target == null || Vector3.Distance(transform.position, target.position) > attackDistance)
        {
            CurrentStatus = Status.Trace;
            return;
        }
        if (Time.time >= lastAttackTime + attackInterval)
        {
            lastAttackTime = Time.time;
            IDamageable damageable = target.GetComponent<IDamageable>();
            var hitNormal = (transform.position - target.position).normalized;
            damageable.OnDamage(damage, target.position, hitNormal);
        }
    }
    private void UpdateTrace()
    {
        if (target != null && Vector3.Distance(transform.position, target.position) < attackDistance)
        {
            CurrentStatus = Status.Attack;
            return;
        }
        CurrentStatus = Status.Trace;
        agent.SetDestination(target.position);
    }

    private Transform FindTarget(float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, targetLayer);

        Transform closet = null;
        float closetDistance = float.MaxValue;

        for (int i = 0; i < colliders.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, colliders[i].transform.position);
            if (distance < closetDistance)
            {
                closetDistance = distance;
                closet = colliders[i].transform;
            }
        }
        return closet;
    }

    public override void Die()
    {
        base.Die();
        CurrentStatus = Status.Die;
    }
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsDead)
        {
            return;
        }
        base.OnDamage(damage, hitPoint, hitNormal);

        monsterAudioSource.PlayOneShot(hitClip);
        hitEffect.transform.position = hitPoint;
        hitEffect.transform.forward = hitNormal;
        hitEffect.Play();
    }

}
