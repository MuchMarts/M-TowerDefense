using System.Collections.Generic;
using UnityEngine;

public class SpeedRing : BaseRing
{
    private float pAttackSpeed = 110f;
    private float adjacencyBonus = 100f;

    private float AttackSpeed() 
    {
        float adjAttackSpeed = AdjacenctCount() * (pAttackSpeed/100) * adjacencyBonus/100;
        return (pAttackSpeed + adjAttackSpeed) / 100;
    }

    public override Dictionary<RingEffectType,object> GetEffect()
    { 
        Dictionary<RingEffectType, object> ability = new Dictionary<RingEffectType, object>
        {
            { RingEffectType.pFireRate, AttackSpeed() }
        };
        return ability;
    }
}
