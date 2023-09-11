using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageTarget
{
    [BoxGroup("Setup Variables")] public GameObjectRuntimeSet runtimeSet;
    [BoxGroup("Setup Variables")] public GameEvent onDeadEvent;
    
    [BoxGroup("Health Variables")] public FloatReference maxHealth;
    [SerializeField]
    [BoxGroup("Health Variables")] private FloatReference currentHealth;

    private Animator animator;

    [BoxGroup("Unity Events")] 
    public UnityEvent onDamageTaken;
    [BoxGroup("Unity Events")] 
    public UnityEvent onDead;

    private void Awake()
    {
        currentHealth.Value = maxHealth;
        TryGetComponent(out animator);
    }

    private void Start()
    {
        if (runtimeSet)
        {
            Debug.Log($"Adding to {runtimeSet.name}");
            runtimeSet.Add(gameObject);
        }
    }
    
    public string GetId()
    {
        return gameObject.name;
    }

    public void TakeDamage(Damage _damage)
    {
        //Take damage if health is above 0
        if (_damage.damage > 0f && currentHealth > 0f)
        {
            currentHealth.Value -= _damage.damage;
            onDamageTaken?.Invoke();
            CheckDead();
        }
    }

    private void CheckDead()
    {
        if (IsDead())
        {
            if (animator != null)
            {
                // Debug.Log("Send Dead Event");
                animator.SetTrigger(AnimationHelper.DeadParameter);
            }

            if (onDeadEvent != null)
            {
                onDeadEvent.Raise();
            }
            onDead?.Invoke();
            runtimeSet.Remove(gameObject);
            // Destroy(gameObject);
        }
        else
        {
            if (animator != null)
            {
                // Debug.Log("Send Hurt Event");
                animator.SetTrigger(AnimationHelper.HurtParameter);
            }
        }
    }

    public bool IsDead()
    {
        return currentHealth <= 0f;
    }

    public float GetHeathPercentage()
    {
        return currentHealth / maxHealth;
    }
}
