using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Events;


public class BossFlow : MonoBehaviour
{
    [Serializable]
    public class BossState
    {
        public float healthPercentage;
        public UnityEvent events;
    }

    [BoxGroup("Setup Variables")] public BossRetreat bossRetreat;
    [BoxGroup("Setup Variables")] public FloatReference bossMaxHealth;
    [BoxGroup("Setup Variables")] public FloatReference bossCurrentHealth;
    [BoxGroup("Setup Variables")] public List<BossState> bossStates;

    public void OnHealthChanged()
    {
        if (bossMaxHealth == 0f)
            return;
        float healthPercent = bossCurrentHealth / bossMaxHealth;
        for (int i = 0; i < bossStates.Count; ++i)
        {
            //Only take the first state. Cannot activate two at once
            if (bossStates[i].healthPercentage >= healthPercent)
            {
                bossRetreat.SetEvents(bossStates[i].events);
                bossRetreat.ForceActivate();
            }
        }
    }
}
