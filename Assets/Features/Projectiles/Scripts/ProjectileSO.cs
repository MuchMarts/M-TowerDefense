using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectileSO", menuName = "Game/Projectile")]
public class ProjectileSO : ScriptableObject
{    
    public string projectileName;
    public float damage = 1;
    public float speed = 1;
    public int pierce = 1;
    
    public bool isHoming = false;
    public float homingRadius = 0;
    
    public bool isTimed = false;
    public float timeToLive = 0;

    public bool isSplash = false;
    public float splashRadius = 0;

    public bool isTrueDamage = false;

    public List<BuffSO> buffs = new();

    // Remove this as unnecessary
    public TargetPriority priority = TargetPriority.Closest;
    public TargetType targetType = TargetType.Enemy;
    
    public GameObject prefab;
}
