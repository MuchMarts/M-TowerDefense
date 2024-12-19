using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Wave", fileName = "Wave", order = 1)]
public class Wave : ScriptableObject
{
    [field: SerializeField]
    public List<WaveSpawns> WaveSpawns { get; private set; }
    [field: SerializeField]
    public float TimeBetweenSpawns { get; private set; }
}

public class WaveSpawns {
    public GameObject Enemy { get; set; }
    public int Count { get; set; }
}