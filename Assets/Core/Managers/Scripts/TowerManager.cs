using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance;
    private List<GameObject> allTowers = new();
    public List<TowerSO> allTowerSO = new();
    
    [SerializeField]
    private GameObject player;
    private PlayerManager playerManager;

    void Awake()
    {
        Instance = this;
        playerManager = player.GetComponent<PlayerManager>();
    }

    internal GameObject GetTowerPrefab(int towerID)
    {
        return allTowerSO[towerID].towerPrefab;
    }

    public void RegisterTower(GameObject tower)
    {
        allTowers.Add(tower);
    }

    public void UnregisterTower(GameObject tower)
    {
        allTowers.Remove(tower);
    }

    public void AddKill(int amount)
    {
        AddPoints(amount);
        AddScore(1);
    }

    private void AddPoints(int amount)
    {
        playerManager.AddPoints(amount);
    }

    private void AddScore(int amount)
    {
        playerManager.AddScore(amount);
    }

}
