using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseIndicator, cellIndicator;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;
    private int selectedTowerID = -1;
    private PlayerManager playerManager;
    private void Start()
    {
        StopPlacement();
        playerManager = GetComponent<PlayerManager>();
    }


    public void StartPlacement(int towerID)
    {
        StopPlacement();
        if (towerID < 0 || towerID >= playerManager.allTowers.Count)
        {
            Debug.LogError("Invalid tower ID: " + towerID);
            return;
        }

        selectedTowerID = towerID;

        GameObject selectedTower = playerManager.allTowers[selectedTowerID];
        if (selectedTower == null)
        {
            Debug.LogError("Tower not found");
            return;
        }

        inputManager.IsPlacingTower = true;

        mouseIndicator.SetActive(true);
        cellIndicator.SetActive(true);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;

    }

    private void PlaceStructure() 
    {
        if (inputManager.IsPointerOverUI()) return;
        
        Vector3 mousePos = inputManager.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);
        Vector3 gridCenter = grid.GetCellCenterWorld(gridPos);
        if (!inputManager.IsOverTower(gridCenter) || !inputManager.IsOverPath(gridCenter))
        {
            Debug.Log("Invalid placement");
            return;
        }

        GameObject tower = Instantiate(playerManager.allTowers[selectedTowerID]);
        tower.transform.position = grid.CellToWorld(gridPos);
        playerManager.AddTower(tower);
    }

    public void StopPlacement()
    {
        selectedTowerID = -1;
        inputManager.IsPlacingTower = false;
        mouseIndicator.SetActive(false);
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
    }

    private void Update()
    {
        Vector3 mousePos = inputManager.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);
        mouseIndicator.transform.position = mousePos;
        cellIndicator.transform.position = grid.CellToWorld(gridPos);
    }

}
