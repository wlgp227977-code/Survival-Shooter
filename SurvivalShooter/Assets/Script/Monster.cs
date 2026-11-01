
using UnityEngine;
using UnityEngine.AI;

public class Monster : LivingEntity
{
    public enum Status
    {
        Idle,
        Trace,
        Attack,
        Die,
    }

    public Transform target;
    public LayerMask targetLayer;

    public float traceDistance = 100f;
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
    private bool isSinking;
    private Status currentStatus;

    public static readonly int IdHasTarget =
        Animator.StringToHash("HasTarget");

    public static readonly int IdDie =
        Animator.StringToHash("Die");

    public Status CurrentStatus
    {
        get => currentStatus;
        set
        {
            currentStatus = value;
            Debug.Log(currentStatus);

            switch (currentStatus)
            {
                case Status.Idle:
                    monsterAnimator.SetBool(IdHasTarget, false);
                    agent.isStopped = true;
                    break;

                case Status.Trace:
                    monsterAnimator.SetBool(IdHasTarget, true);
                    agent.isStopped = false;
                    break;

                case Status.Attack:
                    monsterAnimator.SetBool(IdHasTarget, false);
                    agent.isStopped = true;
                    break;

                case Status.Die:
                    agent.isStopped = true;
                    agent.enabled = false;

                    monsterAnimator.SetTrigger(IdDie);
                    monsterCollider.enabled = false;
                    monsterAudioSource.PlayOneShot(deathClip);

                    deactiveTime = Time.time + 5f;
                    break;
            }
        }
    }

    public void Setup(MonsterData data)
    {
        monsterdata = data;

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

    protected override void OnEnable()
    {
        base.OnEnable();

        agent.enabled = true;

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }

        monsterCollider.enabled = true;

        target = null;
        lastAttackTime = 0f;

        monsterAnimator.SetBool(IdHasTarget, false);
        monsterAnimator.ResetTrigger(IdDie);

        CurrentStatus = Status.Idle;
        isSinking = false;
    }

    private void OnDisable()
    {
        if (agent != null && agent.enabled && agent.isOnNavMesh)
        {
            agent.ResetPath();
        }
    }

    private void Update()
    {
        switch (CurrentStatus)
        {
            case Status.Idle:
                UpdateIdle();
                break;

            case Status.Trace:
                UpdateTrace();
                break;

            case Status.Attack:
                UpdateAttack();
                break;

            case Status.Die:
                UpdateDie();
                break;
        }
    }

    private float deactiveTime;
    public void StartSinking()
    {
        isSinking = true;
    }
    private void UpdateDie()
    {
        if (isSinking)
        {
            transform.position += Vector3.down * 0.3f * Time.deltaTime;
        }

        if (Time.time >= deactiveTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void UpdateAttack()
    {
        Debug.Log($"{name}이 공격 상태!");
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
        if (target == null)
        {
            CurrentStatus = Status.Idle;
            return;
        }

        if (Vector3.Distance(transform.position, target.position) < attackDistance)
        {
            CurrentStatus = Status.Attack;
            return;
        }

        if (Vector3.Distance(transform.position, target.position) > traceDistance)
        {
            target = null;
            CurrentStatus = Status.Idle;
            return;
        }

        agent.SetDestination(target.position);
    }

    private void UpdateIdle()
    {
        if (target != null && Vector3.Distance(transform.position, target.position) < traceDistance)
        {
            CurrentStatus = Status.Trace;
            return;
        }

        target = FindTarget(traceDistance);
    }

    private Transform FindTarget(float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, targetLayer);

        Transform closest = null;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < colliders.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, colliders[i].transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = colliders[i].transform;
            }
        }

        return closest;
    }

    public override void Die()
    {
        if (IsDead)
        {
            return;
        }

        base.Die();

        CurrentStatus = Status.Die;
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        Debug.Log($"몬스터 피격! 데미지: {damage}, 호출 위치 확인");
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
