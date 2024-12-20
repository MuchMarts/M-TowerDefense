using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance;
    private List<GameObject> allTowers = new List<GameObject>();

    [SerializeField]
    private GameObject player;
    private PlayerManager playerManager;

    void Awake()
    {
        Instance = this;
        playerManager = player.GetComponent<PlayerManager>();
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
