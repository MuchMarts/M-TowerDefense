using System;
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

    public RingEffects GetRingEffects()
    {
        if (ringStack.Count == 0) return null;
        RingEffects ringEffects = new RingEffects();
        for (int i = 0; i < ringStack.Count; i++)
        {
            BaseRing ring = ringStack.Peek().GetComponent<BaseRing>();
            
            if (ring == null) 
            {
                Debug.LogError("Ring is null, TowerRingManager.cs, Object: " + gameObject.name);
                return null;
            }


            foreach (KeyValuePair<BaseRing.RingEffectType, object> effect in ring.GetEffect())
            {
                switch (effect.Key)
                {
                    case BaseRing.RingEffectType.fDamage:
                        ringEffects.damage += (float)effect.Value;
                        break;
                    case BaseRing.RingEffectType.pDamage:
                        ringEffects.damage *= (float)effect.Value;
                        break;
                    default:
                        Debug.LogError("Effect not implemented: " + effect.Key);
                        break;
                }
            }

        }
        return ringEffects;
    }
}

// RingEffects Holds all effects from rings calculated, that are then used in TurretManager
// To apply the effects to the turret
public class RingEffects 
{
    public float damage;
    public float slow;
    public int pierce;
    public float range;
    public float fireRate;
    public float projectileSpeed;
    public GameObject projectile;

    public RingEffects()
    {
        damage = 1;
        slow = 0;
        pierce = 0;
        range = 1f;
        fireRate = 1f;
        projectileSpeed = 0;
        projectile = null;
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