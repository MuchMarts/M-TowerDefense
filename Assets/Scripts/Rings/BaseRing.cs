using System.Collections.Generic;
using UnityEngine;

public abstract class BaseRing : MonoBehaviour
{

    public int id { get; protected set; }
    protected List<GameObject> neighbourRings = new List<GameObject>();

    public void AddNeigbhourRing(GameObject ring)
    {
        // Skips Duplicates
        if (neighbourRings.Contains(ring)) return;
        neighbourRings.Add(ring);
    }
    protected int AdjacenctCount()
    {
        int count = 0;
        for (int i = 0; i < neighbourRings.Count; i++)
        {
            // Compare self with neighbour rings, if the same type (same prefab) increment count
            if (gameObject.GetComponent<BaseRing>() == neighbourRings[i].GetComponent<BaseRing>())
            {
                count++;
            }
        }

        return count;
    }

    public void RemoveNeigbhourRing(GameObject ring)
    {
        neighbourRings.Remove(ring);
    }

    public abstract Dictionary<string, object> GetAbility();
}

