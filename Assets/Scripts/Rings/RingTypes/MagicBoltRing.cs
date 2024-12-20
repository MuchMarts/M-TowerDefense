using System.Collections.Generic;
using UnityEngine;

public class MagicBoltRing : BaseRing
{
    public GameObject magicBoltPrefab;

    public override Dictionary<RingEffectType,object> GetEffect()
    { 
        return new Dictionary<RingEffectType, object>
        {
            {RingEffectType.Projectile, magicBoltPrefab},
        };
    }   
}
