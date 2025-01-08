using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject uiContainer;
    public static UIManager Instance;
    
    public GameObject towerShopContainer;
    public GameObject ringShopContainer;
    public GameObject buttonPrefab;
    public GameObject victoryScreen;
    public GameObject defeatScreen;

    private bool isEndScreenActive = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        BuildTowerShop();
        BuildRingShop();
    }

    public void ShowVictoryScreen()
    {
        if (isEndScreenActive)
        {
            return;
        }

        isEndScreenActive = true;
        victoryScreen.SetActive(true);
        victoryScreen.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Score: " + PlayerManager.Instance.GetScore();
    }

    public void ShowDefeatScreen()
    {
        if (isEndScreenActive)
        {
            return;
        }
        
        isEndScreenActive = true;
        defeatScreen.SetActive(true);
    }

    private void BuildTowerShop()
    {
        foreach (TowerSO tower in TowerManager.Instance.allTowerSO)
        {
            GameObject button = Instantiate(buttonPrefab, towerShopContainer.transform);
            // Get button game object and price game object from button container
            GameObject button_container = button.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
            GameObject price_container = button.transform.GetChild(1).gameObject;

            button_container.GetComponent<TextMeshProUGUI>().text = tower.towerName;
            price_container.GetComponent<TextMeshProUGUI>().text = tower.cost.ToString();

            ShopManager.Instance.AddShopItem(tower, button);
        }
    }

    public void BuildRingShop()
    {
        foreach (RingSO ring in RingManager.Instance.allRingSO)
        {
            GameObject button = Instantiate(buttonPrefab, ringShopContainer.transform);
            // Get button game object and price game object from button container
            GameObject button_container = button.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
            GameObject price_container = button.transform.GetChild(1).gameObject;

            button_container.GetComponent<TextMeshProUGUI>().text = ring.ringName;
            price_container.GetComponent<TextMeshProUGUI>().text = ring.cost.ToString();

            ShopManager.Instance.AddShopItem(ring, button);
        }
    }

}
