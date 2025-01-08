using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseIndicator, cellIndicator;

    public InputManager inputManager;
    [SerializeField]
    private Grid grid;
    private TowerSO selectedTowerSO = null;
    private bool towerPlaced = false;
    public static PlacementSystem Instance;
    
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StopPlacement();
    }


    public void StartPlacement(TowerSO tower)
    {
        StopPlacement();

        selectedTowerSO = tower;

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

        towerPlaced = true;
        GameObject tower = Instantiate(selectedTowerSO.prefab);
        tower.transform.position = grid.CellToWorld(gridPos);
        PlayerManager.Instance.AddTower(tower);
    }

    public void StopPlacement()
    {
        if (!towerPlaced && selectedTowerSO != null)
        {
            PlayerManager.Instance.AddPoints(selectedTowerSO.cost);
        }

        towerPlaced = false;
        selectedTowerSO = null;
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
