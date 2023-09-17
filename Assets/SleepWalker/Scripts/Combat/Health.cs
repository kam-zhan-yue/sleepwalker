using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageTarget
{
    [BoxGroup("Invulnerable")] public bool startInvulerable;

    [BoxGroup("Setup Variables")] public bool removeFromSetOnDead = true;
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
    [BoxGroup("Unity Events")] 
    public UnityEvent onDamageAfterDead;
    [BoxGroup("Unity Events")] 
    public UnityEvent onInvulnerableHit;
    
    [BoxGroup("Invulnerable")] 
    [NonSerialized, ShowInInspector, ReadOnly]
    private bool invulnerable = false;

    private bool targetable = true;

    private void Awake()
    {
        currentHealth.Value = maxHealth;
        TryGetComponent(out animator);
    }

    private void Start()
    {
        if (runtimeSet)
        {
            runtimeSet.Add(gameObject);
        }

        invulnerable = startInvulerable;
    }
    
    public string GetId()
    {
        return gameObject.name;
    }

    public bool IsTargetable()
    {
        return targetable;
    }
    
    public void TakeDamage(Damage _damage)
    {
        if (!targetable)
            return;
        if (invulnerable)
        {
            if (IsDead())
            {
                onDamageAfterDead?.Invoke();
            }
            else
            {
                onInvulnerableHit?.Invoke();
            }
            return;
        }
        //Take damage if health is above 0
        if (_damage.damage > 0f)
        {
            if (currentHealth > 0f)
            {
                currentHealth.Value -= _damage.damage;
                onDamageTaken?.Invoke();
                CheckDead();
            }
            //only for special boss state!
            else
            {
                onDamageAfterDead?.Invoke();
            }
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
            if(removeFromSetOnDead && runtimeSet != null)
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

    public void ToggleInvulnerability(bool _toggle)
    {
        invulnerable = _toggle;
    }

    public void ToggleTargetable(bool _toggle)
    {
        targetable = _toggle;
    }

    public bool IsDead()
    {
        return currentHealth <= 0f;
    }

    public float GetHeathPercentage()
    {
        return currentHealth / maxHealth;
    }

    private void OnDisable()
    {
        if(runtimeSet != null)
            runtimeSet.Remove(gameObject);
    }
}
