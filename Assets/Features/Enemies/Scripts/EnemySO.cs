using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemySO", menuName = "Game/Enemy")]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public float baseHealth;
    public float baseSpeed;
    public int basePierceArmour;
    public int baseDamageArmour;
    public int baseKillValue;
    public GameObject prefab;
}
