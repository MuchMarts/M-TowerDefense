using System.Collections.Generic;
using UnityEngine;

public class RangeRing : BaseRing
{
    public float pRange = 1.1f;
    private RingEffectType ringEffectType = RingEffectType.pRange;

    private float adjacencyBonus = 100f;

    private float Range() 
    {
        float adjRange = AdjacenctCount() * pRange * adjacencyBonus/100;
        return pRange + adjRange;
    }

    public override Dictionary<RingEffectType,object> GetEffect()
    { 
        return new Dictionary<RingEffectType, object>
        {
            { ringEffectType, Range() }
        };
    }   
}
