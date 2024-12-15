using System.Collections.Generic;
using UnityEngine;

public class TowerRingManager : MonoBehaviour
{
    [SerializeField]
    private int ringStackSize = 6;
    [SerializeField]
    private GameObject ringAnchor;
    
    private RingStack ringStack;

    public void Start()
    {
        ringStack = new RingStack(ringStackSize);
        gameObject.name += " " + gameObject.GetInstanceID();
    }

    // Public methods
    public bool AddRing(GameObject ring)
    {
        bool success = ringStack.Add(ring);
        if (success) TransforRingLocation(ring);
        return success;
    }

    public bool AddRingFromTower(TowerRingManager tower, GameObject ring)
    {
        bool success = ringStack.Add(ring);
        if (success) {
            tower.RemoveTopRing();
            TransforRingLocation(ring);
        }
        return success;
    }

    public GameObject RemoveTopRing()
    {
        return ringStack.Remove();
    }

    public GameObject PeekTopRing()
    {
        return ringStack.Peek();
    }

    public bool IsTowerFull()
    {
        return ringStack.IsFull();
    }

    private void TransforRingLocation(GameObject ring)
    {
        ring.transform.SetParent(ringAnchor.transform);        
        float offset = ringStack.Count * 0.2f;
        ring.transform.localPosition = new Vector3(0, offset, 0);
    }
}

// Data Structure for RingStack
// Works like a stack but with a fixed size and with the ability to read the whole stack
class RingStack {

    private List<GameObject> ringStack = new List<GameObject>();
    private int index = -1;
    private int ringStackSize;

    public RingStack(int ringStackSize)
    {
        this.ringStackSize = ringStackSize;
    }

    public bool IsFull()
    {
        return (index + 1) >= ringStackSize;
    }

    public int Count
    {
        get { return index + 1; }
    }

    public bool Add(GameObject ring)
    {
        if (IsFull()) return false;
        if (ring == null) {
            Debug.LogError("Ring is null");
            return false;
        }
        ring.SetActive(true);
        AddRing(ring);
        return true;
    }

    private void AddRing(GameObject ring)
    {
        ringStack.Add(ring);
        index++;
        UpdateNeighbours();
    }

    public GameObject Remove()
    {
        if (index < 0) return null;
        GameObject ring = ringStack[index];
        ringStack.RemoveAt(index);
        RemoveNeighbours(ring);
        index--;
        return ring;
    }

    public GameObject Peek()
    {
        if (index < 0) return null;
        return ringStack[index];
    }

    private void UpdateNeighbours()
    {
        if (index < 1) return;

        // Updates neighbours of a ring starting from the bottom
        // Duplicates skipped in BaseRing.AddNeigbhourRing
        for (int i = 0; i < index; i++)
        {
            BaseRing currentRing = ringStack[i].GetComponent<BaseRing>();
            BaseRing nextRing = ringStack[i+1].GetComponent<BaseRing>();

            currentRing.AddNeigbhourRing(nextRing.gameObject);
            nextRing.AddNeigbhourRing(currentRing.gameObject);
        }
    }

    private void RemoveNeighbours(GameObject ring)
    {
        if (index < 1) return;

        BaseRing currentRing = ring.GetComponent<BaseRing>();
        BaseRing previousRing = ringStack[index-1].GetComponent<BaseRing>();

        currentRing.RemoveNeigbhourRing(previousRing.gameObject);
        previousRing.RemoveNeigbhourRing(currentRing.gameObject);
    }

} 