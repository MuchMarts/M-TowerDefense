using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyGroupSO", menuName = "Game/Wave/EnemyGroup")]
public class EnemyGroupSO : ScriptableObject
{
    public EnemySO enemySO;
    public int count;
    public float rate;
    public float delay;
}
