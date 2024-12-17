using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    public List<GameObject> allTowers = new List<GameObject>();
    public List<GameObject> allRings = new List<GameObject>();

    private int Health = 100;
    void Start()
    {
        Debug.Log("Player Started");
        Debug.Log("Towers: " + allTowers.Count);
        Debug.Log("Rings: " + allRings.Count);
    }
    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Debug.Log("Player has died");
        }
        Debug.Log("Player Health: " + Health);
    }

}
