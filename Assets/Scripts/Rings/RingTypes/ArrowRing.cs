using System.Collections.Generic;
using UnityEngine;

public class ArrowRing : BaseRing
{
    public GameObject arrowPrefab;

    public override Dictionary<RingEffectType,object> GetEffect()
    { 
        return new Dictionary<RingEffectType, object>
        {
            {RingEffectType.Projectile, arrowPrefab},
        };
    }   
}
