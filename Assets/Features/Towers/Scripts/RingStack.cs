using UnityEngine;
using System.Collections.Generic;

class RingStack {

    private List<Ring> ringStack = new();

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

    public bool Add(Ring ring)
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

    private void AddRing(Ring ring)
    {
        ringStack.Add(ring);
        index++;
        UpdateNeighbours();
    }

    public Ring Remove()
    {
        if (index < 0) return null;
        
        Ring ring = ringStack[index];
        
        if (ring == null) {
            Debug.LogError("Ring is null");
            return null;
        }

        ringStack.RemoveAt(index);
        index--;

        RemoveNeighbours(ring);
        return ring;
    }

    public Ring Peek()
    {
        if (index < 0) return null;
        return ringStack[index];
    }

    public Ring this[int i]
    {
        get { return ringStack[i]; }
    }

    private void UpdateNeighbours()
    {
        if (Count <= 1) return;

        // Updates neighbours of a ring starting from the bottom
        // Duplicates skipped in BaseRing.AddNeigbhourRing
        for (int i = 0; i < index; i++)
        {
            Ring currentRing = ringStack[i];
            Ring nextRing = ringStack[i+1];

            currentRing.AddNeigbhourRing(nextRing.gameObject);
            nextRing.AddNeigbhourRing(currentRing.gameObject);
        }
    }

    private void RemoveNeighbours(Ring ring)
    {
        if (Count < 1) return;
    
        Ring currentRing = ring;
        Ring previousRing = ringStack[index];

        currentRing.RemoveNeigbhourRing(previousRing.gameObject);
        previousRing.RemoveNeigbhourRing(currentRing.gameObject);
    }

} 