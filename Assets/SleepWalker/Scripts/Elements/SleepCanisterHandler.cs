using System.Collections;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

public class SleepCanisterHandler : MonoBehaviour
{
    [BoxGroup("Setup Variables")] public List<CanisterSwitch> canisters = new();
    [BoxGroup("Setup Variables")] public float reactivateCountdown = 5f;
    
    private CoroutineHandle countdownRoutine;
    private int activatedCanisters = 0;

    public void OnCanisterActivated()
    {
        activatedCanisters++;
        Debug.Log($"{activatedCanisters} / {canisters.Count}");
        if (activatedCanisters >= canisters.Count)
        {
            countdownRoutine = Timing.RunCoroutine(Countdown());
        }
    }

    private IEnumerator<float> Countdown()
    {
        yield return Timing.WaitForSeconds(reactivateCountdown);
        Reactivate();
    }

    public void Reactivate()
    {
        Timing.KillCoroutines(countdownRoutine);
        for (int i = 0; i < canisters.Count; ++i)
        {
            canisters[i].Reactivate();
        }

        activatedCanisters = 0;
    }
}
