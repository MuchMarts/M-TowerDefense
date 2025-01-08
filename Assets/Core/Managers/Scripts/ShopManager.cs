using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    
    public Dictionary<ScriptableObject, GameObject> shopItemButtons = new();
    public GameObject sellTowerButton;
    public Color enabledColor = new();
    public Color disabledColor = new();

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (TowerManager.Instance.selectedTower != null)
        {
            sellTowerButton.SetActive(true);
            sellTowerButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => SellTower(TowerManager.Instance.selectedTower));
            sellFlag = true;
        }
        else
        {
            sellTowerButton.SetActive(false);
            sellTowerButton.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
        }
    }
    private bool sellFlag = true;
    public void SellTower(TowerTurretManager tower)
    {
        if (sellFlag){
            PlayerManager.Instance.AddPoints(tower.towerData.cost);
            Destroy(tower.gameObject.transform.parent.gameObject);
            sellFlag = false;
        }
    }
    public void OnPlayerPointsChange()
    {
        foreach (var item in shopItemButtons)
        {
            if (item.Key is TowerSO)
            {
                TowerSO tower = (TowerSO)item.Key;
                if (PlayerManager.Instance.GetPoints() >= tower.cost)
                {
                    EnableShopItem(item.Key);
                }
                else
                {
                    DisableShopItem(item.Key);
                }
            }
            else if (item.Key is RingSO)
            {
                RingSO ring = (RingSO)item.Key;
                if (PlayerManager.Instance.GetPoints() >= ring.cost)
                {
                    EnableShopItem(item.Key);
                }
                else
                {
                    DisableShopItem(item.Key);
                }
            }
        }
    }

    public void AddShopItem(ScriptableObject item, GameObject button)
    {
        shopItemButtons.Add(item, button);
        AddShopItemEventHandlers(item, button);
    }

    public void EnableShopItem(ScriptableObject item)
    {
        shopItemButtons[item].transform.GetChild(0).GetComponent<UnityEngine.UI.Button>().interactable = true;
        shopItemButtons[item].transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = enabledColor;
    }

    public void DisableShopItem(ScriptableObject item)
    {
        shopItemButtons[item].transform.GetChild(0).GetComponent<UnityEngine.UI.Button>().interactable = false;
        shopItemButtons[item].transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = disabledColor;
    }
    
    // For each item in the shop, add a event handler for the button click, to buy the item
    public void AddShopItemEventHandlers(ScriptableObject item, GameObject button)
    {
        button.transform.GetChild(0).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => BuyItem(item));
    }

    public void BuyItem(ScriptableObject item)
    {
        if (item is TowerSO _tower) {
            Debug.Log("Buying tower: " + _tower.towerName);
        }

        if (item is RingSO _ring) {
            Debug.Log("Buying ring: " + _ring.ringName);
        }



        if (item is TowerSO tower)
        {
            if (PlayerManager.Instance.GetPoints() >= tower.cost)
            {
                PlayerManager.Instance.AddPoints(-tower.cost);
                PlacementSystem.Instance.StartPlacement(tower);
            }
        }
        else if (item is RingSO ring)
        {
            if (PlayerManager.Instance.GetPoints() >= ring.cost)
            {
                PlayerManager.Instance.AddPoints(-ring.cost);
                RingSystem.Instance.StartPlacementWithNewRing(ring);
            }
        }
    }

}