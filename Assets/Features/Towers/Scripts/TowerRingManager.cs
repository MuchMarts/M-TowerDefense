using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TowerRingManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ringAnchor;
    
    private int ringStackSize;
    public UnityEvent RingStackChanged;
    private RingStack ringStack;
    private TowerSO towerData;

    void Start()
    {
        ringStack = new RingStack(towerData.ringStackSize);
    }

    public void SetTowerData(TowerSO towerData)
    {
        this.towerData = towerData;
        ringStackSize = towerData.ringStackSize;
    }

    // Public methods
    public bool AddRing(GameObject ring, TowerRingManager tower = null)
    {
        if (ring == null && ring.GetComponent<Ring>() == null){
            Debug.LogError("Ring is null, TowerRingManager.cs, Tower: " + gameObject.transform.name);
            return false;
        };

        bool success = ringStack.Add(ring.GetComponent<Ring>());
        
        if (success) 
        {
            RingStackChanged.Invoke();
            if (tower != null) tower.RemoveTopRing();
            TransforRingLocation(ring);
        }
        
        return success;
    }

    public Ring RemoveTopRing()
    {
        Ring ring = ringStack.Remove();
        RingStackChanged.Invoke();
        return ring;
    }

    public Ring PeekTopRing()
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
        // TODO: Make this dynamic. for now, it's hardcoded
        float offset = (0.8f-0.1f) / ringStackSize;    
        float new_ring_pos = ringStack.Count * offset;
        ring.transform.localPosition = new Vector3(0, new_ring_pos, 0);
    }

    public List<RingEffect> GetRingEffects()
    {
        if (ringStack.Count == 0)
        {
            Debug.LogWarning("Ring stack is empty, TowerRingManager.cs, Tower: " + gameObject.transform.name);
            return null;
        }

        List<RingEffect> allRingEffects = new();

        for (int i = 0; i < ringStack.Count; i++)
        {
            Ring ring = ringStack[i];
            
            if (ring == null) 
            {
                Debug.LogError("Ring is null, TowerRingManager.cs, Tower: " + gameObject.transform.name);
                return null;
            }

            ring.GetEffects().ForEach(effect =>
            {
                if (effect.type == RingEffectType.RingReadManipulation)
                {
                    Debug.Log("RingReadManipulation not implemented!");
                } 
                else
                {
                    allRingEffects.Add(effect);
                }
            });
        }

        return allRingEffects;
    }
}
