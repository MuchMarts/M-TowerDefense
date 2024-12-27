using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerSO towerData;
    private TowerRingManager ringManager;
    private TowerTurretManager turretManager;

    void Awake()
    {
        ringManager = gameObject.GetComponentInChildren<TowerRingManager>();
        turretManager = gameObject.GetComponentInChildren<TowerTurretManager>();
        gameObject.name = towerData.name + "-" + gameObject.GetInstanceID();
    }

    void Start()
    {
        ringManager.SetTowerData(towerData);
        turretManager.SetTowerData(towerData);
    }

}
