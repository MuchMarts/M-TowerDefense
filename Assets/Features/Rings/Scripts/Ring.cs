using System;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    private List<GameObject> neighbourRings = new();
    public RingSO ringData;

    // Animation config
    float max_spin_speed = 0.5f;
    float min_spin_speed = 0.1f;
    float spin_speed;
    float spin_direction;
    
    public void Start()
    {
        spin_direction = UnityEngine.Random.Range(1, 3) == 1 ? 1 : -1;
        spin_speed = UnityEngine.Random.Range(min_spin_speed, max_spin_speed);
    }

    void Update()
    {
        gameObject.transform.GetChild(0).Rotate(0, 0, spin_speed * spin_direction);
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void AddNeigbhourRing(GameObject ring)
    {
        // Skips Duplicates
        if (neighbourRings.Contains(ring)) return;
        neighbourRings.Add(ring);
    }
    protected int AdjacenctCount()
    {
        int count = 0;
        for (int i = 0; i < neighbourRings.Count; i++)
        {
            // Compare self with neighbour rings, if the same type (same prefab) increment count
            if (gameObject.GetComponent<Ring>().ringData == neighbourRings[i].GetComponent<Ring>().ringData)
            {
                count++;
            }
        }

        return count;
    }

    public void RemoveNeigbhourRing(GameObject ring)
    {
        neighbourRings.Remove(ring);
    }

    public List<RingEffect> GetEffects() {
        List<RingEffect> processedEffects = new();

        foreach (RingEffectSO effectSO in ringData.effects)
        {
            RingEffect ringEffect = new ();
            ringEffect.SetValues(effectSO);
            
            // Example 1: 10 + 10 * 2 * 0.1 = 12
            // Example 2: 1.2 + 1.2 * 2 * 0.1 = 1.44
            ringEffect.value = CalculateTotalEffect(ringEffect, effectSO);
        
            processedEffects.Add(ringEffect);
        }

        return processedEffects;
    }
    object CalculateTotalEffect(RingEffect effect, RingEffectSO effectSO)
    {
        if (!effect.isAdjacencyBonus) {
            return effect.value;
        }
        
        float adjancecyBonus = AdjacenctCount() * ringData.adjacencyBonus;

        switch (effectSO.valueType)
        {
            case RingEffectSO.ValueType.Int:
                return effectSO.intValue + adjancecyBonus;
            case RingEffectSO.ValueType.Float:
                return effectSO.floatValue + adjancecyBonus;
            default:
                return effect.value;
        }

    }
}

public enum RingEffectType
{
    // f = flat, p = percentage
    fDamage,
    pDamage,
    Slow,
    Pierce,
    fRange,
    pRange,
    fFireRate,
    pFireRate,
    Projectile,
    ProjectileSpeed,
    Buff,
    isHoming,
    HomingRange,
    isTimed,
    TimeToLive,
    isSplash,
    SplashRadius,
    isTrueDamage,
    RingReadManipulation,
}

