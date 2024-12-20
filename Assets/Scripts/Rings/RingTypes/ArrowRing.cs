using System.Collections.Generic;
using UnityEngine;

public class ArrowRing : BaseRing
{
    public GameObject arrowPrefab;
    public float pfireRate = 0.5f;

    public override Dictionary<RingEffectType,object> GetEffect()
    { 
        return new Dictionary<RingEffectType, object>
        {
            {RingEffectType.Projectile, arrowPrefab},
            {RingEffectType.pFireRate, pfireRate}
        };
    }   
}
