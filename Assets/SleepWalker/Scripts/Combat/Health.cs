using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageTarget
{
    [BoxGroup("Health Variables")]
    public float maxHealth;
    private float currentHealth;


    [BoxGroup("Events")] 
    public UnityEvent onDead;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public string GetId()
    {
        return gameObject.name;
    }

    public void TakeDamage(Damage _damage)
    {
        currentHealth -= _damage.damage;
        CheckDead();
    }

    private void CheckDead()
    {
        if (currentHealth <= 0f)
        {
            onDead?.Invoke();
            Destroy(this);
        }
    }

    public float GetHeathPercentage()
    {
        return currentHealth / maxHealth;
    }
}
