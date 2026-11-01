using UnityEngine;
using UnityEngine.Events;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth = 100f;
    public float Health { get; private set; }
    public bool IsDead { get; private set; }

    public UnityEvent OnDead;

    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        Debug.Log($"{name}이 {damage} 데미지를 받음");
        if (IsDead)
        {
            return;
        }

        Health -= damage;
        if (Health <= 0f)
        {
            Health = 0f;
            Die();
        }
    }
    protected virtual void OnEnable()
    {
        IsDead = false;
        Health = startingHealth;      
          
    }


    public virtual void Die()
    {
        IsDead = true;
        OnDead.Invoke();
    }    

}
