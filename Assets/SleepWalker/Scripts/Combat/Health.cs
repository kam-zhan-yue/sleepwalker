using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageTarget
{
    [BoxGroup("Health Variables")]
    public FloatReference maxHealth;
    [BoxGroup("Health Variables")]
    [SerializeField]
    private FloatReference currentHealth;


    [BoxGroup("Events")] 
    public UnityEvent onDamageTaken;
    public UnityEvent onDead;

    private void Awake()
    {
        currentHealth.Value = maxHealth;
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
            CheckDead();
        }
    }

    private void CheckDead()
    {
        if (currentHealth <= 0f)
        {
            onDead?.Invoke();
            Destroy(gameObject);
        }
    }

    public float GetHeathPercentage()
    {
        return currentHealth / maxHealth;
    }
}
