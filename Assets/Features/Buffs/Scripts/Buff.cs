using UnityEngine;

public enum BuffType { Regen, Poison, Bleed, Slow, Stun, Speed}

public class Buff
{
    public BuffType type;
    public float duration;
    public float tickRate; // Per second
    public float value;
    public GameObject source;
}