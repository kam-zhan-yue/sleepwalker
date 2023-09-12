using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class WaveController : MonoBehaviour
{

    public void Spawn(Wave.WaveObject _waveObject)
    {
        switch (_waveObject.enemyType)
        {
            case Wave.EnemyType.Light:
                
                break;
            case Wave.EnemyType.Heavy:
                break;
        }
    }
}
