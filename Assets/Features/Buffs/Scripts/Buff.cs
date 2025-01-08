using UnityEngine;

public enum BuffType { Regen, Poison, Bleed, Slow, Stun, Speed}

public class Buff
{
    public BuffSO buffSO;
    public BuffType type;
    public float duration;
    public float tickRate; // Per second
    private float _value;
    public float Value { 
        get => _value * stackCount; 
        set => _value = value; 
        }
    public bool canStack;
    public int stackCount = 1;
    public GameObject source;

    public Buff(BuffSO _buffSO)
    {
        type = _buffSO.type;
        duration = _buffSO.duration;
        tickRate = _buffSO.tickRate;
        Value = _buffSO.value;
        canStack = _buffSO.canStack;
        buffSO = _buffSO;
    }

    
}