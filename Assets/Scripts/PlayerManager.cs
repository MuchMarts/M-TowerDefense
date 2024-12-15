using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<GameObject> allTowers = new List<GameObject>();
    public List<GameObject> allRings = new List<GameObject>();

    void Start()
    {
        Debug.Log("Player Started");
        Debug.Log("Towers: " + allTowers.Count);
        Debug.Log("Rings: " + allRings.Count);
    }
}
