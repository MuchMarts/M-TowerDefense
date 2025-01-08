using UnityEngine;

public class RingSystem : MonoBehaviour
{
    // References
    [SerializeField]
    private GameObject mouseIndicator;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private LayerMask towerLayerMask;

    // Variables for Ring Placement
    private TowerRingManager ringOriginTowerManager;    
    private GameObject currentRing = null;
    private RingSO selectedNewRingSO = null;
    private bool ringPlaced = false;
    public static RingSystem Instance;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StopPlacement();
        inputManager.OnClicked += SelectTopRing;
    }

    private Vector3Int GridPosition()
    {
        Vector3 position = inputManager.GetSelectedMapPosition();
        return grid.WorldToCell(position);
    }

    private GameObject GetTowerAtPosition(Vector3Int gridPos)
    {
        Collider[] intersecting = Physics.OverlapSphere(grid.GetCellCenterWorld(gridPos), 0.01f, towerLayerMask);
        if (intersecting.Length > 0) return intersecting[0].gameObject;
        return null;
    }

    // When clicking on a tower, select the top ring if it has one
    public void SelectTopRing()
    {
        if (inputManager.IsPlacingTower) return;
        if (inputManager.IsPointerOverUI()) return;
        if (currentRing != null) return;

        Vector3Int gridPos = GridPosition();
        GameObject tower = GetTowerAtPosition(gridPos);

        if (tower == null)
        {
            Debug.Log("No Tower Selected");
            return;
        }

        TowerRingManager towerBehaviour = tower.GetComponentInParent<TowerRingManager>();

        if (towerBehaviour == null)
        {
            Debug.LogError("No tower behaviour found");
            return;
        }

        if (towerBehaviour.PeekTopRing() == null)
        {
            Debug.Log("No ring to select");
            return;
        }

        StartPlacementWithExistingRing(towerBehaviour.PeekTopRing(), towerBehaviour);
    }

    // Start placement with a new ring
    public void StartPlacementWithNewRing(RingSO _ring)
    {
        StopPlacement();
        
        selectedNewRingSO = _ring;

        GameObject ring = Instantiate(selectedNewRingSO.prefab);
        ring.SetActive(false);
        currentRing = ring;

        inputManager.OnClicked += AddRing;
        inputManager.OnExit += StopPlacement;
        mouseIndicator.SetActive(true);
    }

    // Move existing ring from another tower, called by SelectTopRing()
    private void StartPlacementWithExistingRing(Ring ring, TowerRingManager tower)
    {
        StopPlacement();
        currentRing = ring.gameObject;
        ringOriginTowerManager = tower;

        inputManager.OnClicked += AddRing;
        inputManager.OnExit += StopPlacement;
        mouseIndicator.SetActive(true);
    }
    
    // Handles adding a ring to a tower, for both new and existing rings
    private void AddRing()
    {
        // Guard clauses
        if (inputManager.IsPointerOverUI()) return;

        if (currentRing == null)
        {
            Debug.LogError("No Ring selected");
            StopPlacement();
            return;
        }

        Vector3Int gridPos = GridPosition();
        GameObject tower = GetTowerAtPosition(gridPos);

        if (tower == null)
        {
            Debug.Log("No Tower Selected");
            return;
        }

        TowerRingManager towerBehaviour = tower.GetComponentInParent<TowerRingManager>();
        
        if (towerBehaviour == null)
        {
            Debug.LogError("No tower behaviour found");
            return;
        }

        if (towerBehaviour.IsTowerFull())
        {
            Debug.Log("Tower is full");
            return;
        }

        if (ringOriginTowerManager != null)
        {
            towerBehaviour.AddRing(currentRing, ringOriginTowerManager);
            Debug.Log("Ring: " + currentRing.name + " added to tower: " + towerBehaviour.name + " at position: " + gridPos + " from tower: " + ringOriginTowerManager.name);
        }
        else 
        {
            ringPlaced = true;
            towerBehaviour.AddRing(currentRing);
            Debug.Log("Ring: " + currentRing.name + " added to tower: " + towerBehaviour.name + " at position: " + gridPos);
        }

        StopPlacement();
    }

    private void StopPlacement()
    {
        if (!ringPlaced && selectedNewRingSO != null && ringOriginTowerManager == null)
        {
            PlayerManager.Instance.AddPoints(selectedNewRingSO.cost);
        }

        ringPlaced = false;
        inputManager.OnClicked -= AddRing;
        inputManager.OnExit -= StopPlacement;
        selectedNewRingSO = null;
        mouseIndicator.SetActive(false);
        
        // Reset the current ring and tower
        currentRing = null;
        ringOriginTowerManager = null;
    }
}
