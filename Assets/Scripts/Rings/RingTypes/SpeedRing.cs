using System.Collections.Generic;
using UnityEngine;

public class SpeedRing : BaseRing
{
    [SerializeField]
    private int damage = -1;
    [SerializeField]
    private int attackSpeed = 110;
    [SerializeField]
    private int adjacencyBonus = 100;

    private int AttackSpeed() 
    {
        return attackSpeed + (AdjacenctCount() * (attackSpeed-100) * adjacencyBonus/100);
    }

    public override Dictionary<string,object> GetAbility()
    { 
        Dictionary<string, object> ability = new Dictionary<string, object>
        {
            { "damage", damage },
            { "attackSpeed", AttackSpeed() }
        };
        return ability;
    }
}
