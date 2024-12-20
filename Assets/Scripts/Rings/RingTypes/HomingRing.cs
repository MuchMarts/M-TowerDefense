using System.Collections.Generic;
using UnityEngine;

public class HomingRing : BaseRing
{
    public bool isHoming = true;
    public float homingRadius = 1f;
    public float adjacencyBonus = 20f;

    private float HomingRange()
    {
        return homingRadius + homingRadius * AdjacenctCount() * adjacencyBonus / 100;
    }
    public override Dictionary<RingEffectType,object> GetEffect()
    { 
        return new Dictionary<RingEffectType, object>
        {
            {RingEffectType.isHoming, isHoming},
            {RingEffectType.HomingRange, HomingRange()}
        };
    }   
}
