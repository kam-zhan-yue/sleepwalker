using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class Brain : MonoBehaviour
{
    [NonSerialized, ShowInInspector, ReadOnly]
    public Transform target;
}
