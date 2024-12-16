using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    private List<GameObject> allEnemies = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    public void RegisterEnemy(GameObject enemy)
    {
        allEnemies.Add(enemy);
    }

    public void UnregisterEnemy(GameObject enemy)
    {
        allEnemies.Remove(enemy);
    }

    public List<GameObject> GetEnemiesInRange(Vector3 position, float range)
    {
        List<GameObject> enemiesInRange = new List<GameObject>();
        foreach (var enemy in allEnemies)
        {
            if (enemy != null && Vector3.Distance(position, enemy.transform.position) <= range)
            {
                enemiesInRange.Add(enemy);
            }
        }
        return enemiesInRange;
    }
}
