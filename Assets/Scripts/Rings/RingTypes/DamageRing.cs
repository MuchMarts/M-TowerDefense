using System.Collections.Generic;
using UnityEngine;

public class DamageRing : BaseRing
{
    private float fdamage = 10f;
    private float adjacencyBonus = 100f;

    private float Damage() 
    {
        float adjBonusDamage = (AdjacenctCount() * fdamage * adjacencyBonus/100);
        Debug.Log("Damage: " + fdamage + " Adjacency: " + AdjacenctCount() + " Bonus: " + adjacencyBonus/100 + "AdjBonusDamage: " + adjBonusDamage);
        return fdamage + adjBonusDamage;
    }

    public override Dictionary<RingEffectType, object> GetEffect()
    { 
        Dictionary<RingEffectType, object> ability = new Dictionary<RingEffectType, object>
        {
            { RingEffectType.pDamage, Damage() },
        };
        return ability;
    }
}
