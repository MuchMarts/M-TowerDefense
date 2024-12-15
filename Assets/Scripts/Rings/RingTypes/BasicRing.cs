using System.Collections.Generic;
using UnityEngine;

public class BasicRing : BaseRing
{
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private int attackSpeed = 100;
    [SerializeField]
    private int adjacencyBonus = 50;

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
