using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerSO", menuName = "Game/Tower")]
public class TowerSO : ScriptableObject
{
    public string towerName;
    public int cost;
    public float baseRange;
    public float baseFireRate;
    public float baseTurnSpeed;
    public int ringStackSize;
    public TargetType targetingType;
    public TargetPriority targetingPriority;
    public GameObject prefab;
    public ProjectileSO baseProjectile;
}
