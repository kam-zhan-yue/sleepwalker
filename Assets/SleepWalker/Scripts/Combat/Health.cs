using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageTarget
{
    [BoxGroup("Setup Variables")]
    public GameEvent onDeadEvent;
    
    [BoxGroup("Health Variables")]
    public FloatReference maxHealth;
    [BoxGroup("Health Variables")]
    [SerializeField]
    private FloatReference currentHealth;

    private Animator animator;

    [BoxGroup("Events")] 
    public UnityEvent onDamageTaken;
    public UnityEvent onDead;

    private void Awake()
    {
        currentHealth.Value = maxHealth;
        TryGetComponent(out animator);
    }

    public string GetId()
    {
        return gameObject.name;
    }

    public void TakeDamage(Damage _damage)
    {
        if (_damage.damage > 0f)
        {
            currentHealth.Value -= _damage.damage;
            onDamageTaken?.Invoke();
            
            if (currentHealth <= 0f)
            {
                if (animator != null)
                {
                    Debug.Log("Send Dead Event");
                    animator.SetTrigger(AnimationHelper.DeadParameter);
                }

                if (onDeadEvent != null)
                {
                    onDeadEvent.Raise();
                }
                onDead?.Invoke();
                // Destroy(gameObject);
            }
            else
            {
                if (animator != null)
                {
                    Debug.Log("Send Hurt Event");
                    animator.SetTrigger(AnimationHelper.HurtParameter);
                }
            }
        }
    }

    public float GetHeathPercentage()
    {
        return currentHealth / maxHealth;
    }
}
