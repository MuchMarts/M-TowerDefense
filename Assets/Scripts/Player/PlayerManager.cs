using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    public List<GameObject> allTowers = new List<GameObject>();
    public List<GameObject> allRings = new List<GameObject>();

    private TowerManager towerManager;

    private int health = 100;
    public UnityEvent OnHealthZero;
    public UnityEvent OnHealthChange;

    private int points = 100;
    public UnityEvent OnPointsChange;
    private int score = 0;
    public UnityEvent OnScoreChange;

    void Awake()
    {
        Debug.Log("Player Awake");
        towerManager = TowerManager.Instance;
    }

    void Start()
    {
        Debug.Log("Player Started");
        Debug.Log("Towers: " + allTowers.Count);
        Debug.Log("Rings: " + allRings.Count);
        OnHealthChange.Invoke();
        OnPointsChange.Invoke();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        OnHealthChange.Invoke();
        if (health <= 0)
        {
            OnHealthZero.Invoke();
        }
    }

    public void AddPoints(int amount)
    {
        points += amount;
        OnPointsChange.Invoke();
    }

    public void RemovePoints(int amount)
    {
        points -= amount;
        OnPointsChange.Invoke();
    }

    public void AddScore(int amount)
    {
        score += amount;
        OnScoreChange.Invoke();
    }

    public int GetPoints()
    {
        return points;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetScore()
    {
        return score;
    }

    public void AddTower(GameObject tower)
    {
        towerManager.RegisterTower(tower);
    }

    public void RemoveTower(GameObject tower)
    {
        towerManager.UnregisterTower(tower);
    }

}
