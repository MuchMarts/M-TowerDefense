using System.Collections.Generic;
using UnityEngine;

public class BasicRing : BaseRing
{
    private float fdamage = 1f;
    private float adjacencyBonus = 50f;

    private float Damage() 
    {
        float adjBonusDamage = (AdjacenctCount() * fdamage * adjacencyBonus/100);
        return fdamage + adjBonusDamage;
    }

    public override Dictionary<RingEffectType, object> GetEffect()
    { 
        Dictionary<RingEffectType, object> ability = new Dictionary<RingEffectType, object>
        {
            { RingEffectType.fDamage, Damage() },
        };
        return ability;
    }
}
