using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class HealthDecision : Decision
{
    [BoxGroup("Setup Variables")] public Health health;
    [BoxGroup("Setup Variables")] public float healthThreshold;
    
    protected override bool CanActivate()
    {
        float currentHealthPercentage = health.GetHeathPercentage();
        return currentHealthPercentage <= healthThreshold;
    }
}
