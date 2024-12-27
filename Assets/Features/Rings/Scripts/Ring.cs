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
            if (gameObject.GetComponent<Ring>().GetType() == neighbourRings[i].GetComponent<Ring>().GetType())
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
            
            if (effectSO.isAdjacencyBonus)
            {
                // Example 1: 10 + 10 * 2 * 0.1 = 12
                // Example 2: 1.2 + 1.2 * 2 * 0.1 = 1.44
                switch(effectSO.valueType)
                {
                    case RingEffectSO.ValueType.Int:
                        ringEffect.value = (float)effectSO.intValue + (float)effectSO.intValue * AdjacenctCount() * ringData.adjacencyBonus;
                        break;
                    case RingEffectSO.ValueType.Float:
                        ringEffect.value = effectSO.floatValue + effectSO.floatValue * AdjacenctCount() * ringData.adjacencyBonus;
                        break;
                    default:
                        throw new Exception("Unhandled value type");
                }
            }
            processedEffects.Add(ringEffect);
        }

        return processedEffects;
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
    isHoming,
    HomingRange,
    RingReadManipulation,
}

