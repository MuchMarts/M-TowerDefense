using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectileSO", menuName = "Game/Projectile")]
public class ProjectileSO : ScriptableObject
{    
    public string projectileName;
    public float baseDamage;
    public float baseSpeed;
    public int basePierce;
    
    public bool isHoming = false;
    public float homingRadius;
    
    public bool isTimed = false;
    public float timeToLive;

    public bool isSplash = false;
    public float splashRadius;
    public float splashDamage;

    public bool isTrueDamage = false;

    public BuffSO[] buffs;

    public TargetPriority priority = TargetPriority.Closest;
    public TargetType targetType = TargetType.Enemy;
    
    public GameObject prefab;
}
