using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TowerRingManager : MonoBehaviour
{
    [SerializeField]
    private int ringStackSize = 6;
    [SerializeField]
    private GameObject ringAnchor;
    
    public UnityEvent RingStackChanged;

    private RingStack ringStack;

    public void Start()
    {
        ringStack = new RingStack(ringStackSize);
        gameObject.name += "-" + gameObject.GetInstanceID();
    }

    // Public methods
    public bool AddRing(GameObject ring, TowerRingManager tower = null)
    {
        bool success = ringStack.Add(ring);
        if (success) 
        {
            RingStackChanged.Invoke();
            if (tower != null) tower.RemoveTopRing();
            TransforRingLocation(ring);
        }
        
        return success;
    }

    public GameObject RemoveTopRing()
    {
        RingStackChanged.Invoke();
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

    public List<RingEffect> GetRingEffects()
    {
        if (ringStack.Count == 0)
        {
            Debug.LogWarning("Ring stack is empty, TowerRingManager.cs, Tower: " + gameObject.transform.name);
            return null;
        }

        List<RingEffect> ringEffects = new List<RingEffect>();

        for (int i = 0; i < ringStack.Count; i++)
        {
            GameObject ring = ringStack[i];
            
            if (ring == null) 
            {
                Debug.LogError("Ring in stack is null, TowerRingManager.cs, Tower: " + gameObject.transform.name);
                return null;
            }

            BaseRing baseRing = ring.GetComponent<BaseRing>();
            // Skip if ring does not have a BaseRing component, should not happen
            if (baseRing == null) {
                Debug.LogError("BaseRing is null, TowerRingManager.cs, Tower: " + gameObject.transform.name);
                continue;
            }

            foreach (KeyValuePair<RingEffectType, object> effect in baseRing.GetEffect())
            {
                RingEffect ringEffect = new RingEffect();
                ringEffect.ringEffectType = effect.Key;
                ringEffect.effectValue = effect.Value;
                ringEffects.Add(ringEffect);
            }
        }

        return ringEffects;
    }
}

public class RingEffect
{
    public RingEffectType ringEffectType;
    public object effectValue;
    public bool isUsed = false;
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

    public GameObject this[int i]
    {
        get { return ringStack[i]; }
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