using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class Stats
{
    [TableColumnWidth(50)]
    public float speed;
    [TableColumnWidth(50)]
    public float attack;
}
