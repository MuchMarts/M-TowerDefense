using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int pierce_armour = 0; // Determines by how much projectiles pierce gets reduced

    public int value = 1;

    void OnDestroy()
    {
        EnemyManager.Instance.UnregisterEnemy(gameObject);
    }
}
