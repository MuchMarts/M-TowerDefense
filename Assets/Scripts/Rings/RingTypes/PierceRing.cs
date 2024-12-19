using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PierceRing : BaseRing
{
    private float pierce = 2;
    private RingEffectType ringEffectType = RingEffectType.Pierce;
    private float adjacencyBonus = 50f;

    private int Pierce() 
    {
        float adjPierce = AdjacenctCount() * pierce * adjacencyBonus/100;
        return (int)(pierce + adjPierce);
    }

    public override Dictionary<RingEffectType,object> GetEffect()
    { 
        Dictionary<RingEffectType,object> effect = new Dictionary<RingEffectType, object>
        {
            { ringEffectType, Pierce() }
        };
        return effect;
    }   
}
