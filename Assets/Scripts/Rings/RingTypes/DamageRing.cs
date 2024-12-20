using System.Collections.Generic;
using UnityEngine;

public class DamageRing : BaseRing
{
    private float fDamage = 10f;
    private RingEffectType ringEffectType = RingEffectType.fDamage;
    private float adjacencyBonus = 100f;

    private float Damage() 
    {
        float adjBonusDamage = AdjacenctCount() * fDamage * adjacencyBonus/100;
        Debug.Log("Damage: " + fDamage + " Adjacency: " + AdjacenctCount() + " Bonus: " + adjacencyBonus/100 + "AdjBonusDamage: " + adjBonusDamage);
        return fDamage + adjBonusDamage;
    }

    public override Dictionary<RingEffectType, object> GetEffect()
    { 
        return new Dictionary<RingEffectType, object>
        {
            { ringEffectType, Damage() },
        };
    }
}
