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
    public Grid grid;
    public LayerMask towerLayerMask;
    public TowerTurretManager selectedTower;
    void Awake()
    {
        Instance = this;
        playerManager = player.GetComponent<PlayerManager>();
    }

    void Start()
    {
        PlacementSystem.Instance.inputManager.OnClicked += GetSelectTower;
        PlacementSystem.Instance.inputManager.OnExit += UnselectTower;
    }

    internal GameObject GetTowerPrefab(int towerID)
    {
        return allTowerSO[towerID].prefab;
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

    private GameObject GetTowerAtPosition(Vector3Int gridPos)
    {
        Collider[] intersecting = Physics.OverlapSphere(grid.GetCellCenterWorld(gridPos), 0.01f, towerLayerMask);
        if (intersecting.Length > 0) return intersecting[0].gameObject;
        return null;
    }
    void GetSelectTower()
    {
        Vector3 mapPosition =  PlacementSystem.Instance.inputManager.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mapPosition);
    
        GameObject _selectedTower = GetTowerAtPosition(gridPos);

        if (_selectedTower == null) return;

        selectedTower = _selectedTower.GetComponentInChildren<TowerTurretManager>();

        if (selectedTower == null) {
            Debug.LogError("Cant select tower: No tower behaviour found");
            return;
        }
        Debug.Log("Tower Selected");
    }

    void UnselectTower()
    {
        if (selectedTower == null) return;
        selectedTower = null;
    }
}
