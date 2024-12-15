using System.Collections.Generic;
using UnityEngine;

public class DamageRing : BaseRing
{
    [SerializeField]
    private int damage = 10;
    [SerializeField]
    private int attackSpeed = 90;
    [SerializeField]
    private int adjacencyBonus = 100;

    private int Damage() 
    {
        return damage + (AdjacenctCount() * damage * adjacencyBonus/100);
    }

    public override Dictionary<string,object> GetAbility()
    { 
        Dictionary<string, object> ability = new Dictionary<string, object>
        {
            { "damage", Damage() },
            { "attackSpeed", attackSpeed }
        };
        return ability;
    }
}
