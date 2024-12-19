using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int pierce_armour = 0; // Determines by how much projectiles pierce gets reduced

    void OnDestroy()
    {
        EnemyManager.Instance.UnregisterEnemy(gameObject);
    }
}
