using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager Instance;
    public List<ProjectileSO> AllProjectileSO;

    void Awake()
    {
        Instance = this;
    }

    public GameObject SpawnProjectile(ProjectileSO projectileSO, Vector3 position, Quaternion rotation)
    {
        return GameObjectPoolManager.Instance.GetObject(projectileSO, projectileSO.prefab, position, rotation);
    }

    public void DespawnProjectile(ProjectileSO projectileSO, GameObject projectile)
    {
        GameObjectPoolManager.Instance.ReleaseObject(projectileSO, projectile);
    }
}
