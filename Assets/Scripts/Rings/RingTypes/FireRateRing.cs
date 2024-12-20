using System.Collections.Generic;
using UnityEngine;

public class FireRateRing : BaseRing
{
    private float pFireRate = 150f;
    private float adjacencyBonus = 100f;

    private float FireRate() 
    {
        float adjFireRate = AdjacenctCount() * (pFireRate/100) * adjacencyBonus/100;
        return (pFireRate + adjFireRate) / 100;
    }

    public override Dictionary<RingEffectType,object> GetEffect()
    { 
        return new  Dictionary<RingEffectType, object>
        {
            { RingEffectType.pFireRate, FireRate() }
        };
    }
}
