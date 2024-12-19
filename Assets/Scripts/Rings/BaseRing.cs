using System.Collections.Generic;
using UnityEngine;

public abstract class BaseRing : MonoBehaviour
{

    public int id { get; protected set; }
    protected List<GameObject> neighbourRings = new List<GameObject>();

    float max_spin_speed = 0.5f;
    float min_spin_speed = 0.1f;
    float spin_speed;
    float spin_direction;
    
    public void Start()
    {
        spin_direction = Random.Range(1, 3) == 1 ? 1 : -1;
        spin_speed = Random.Range(min_spin_speed, max_spin_speed);
    }

    void Update()
    {
        gameObject.transform.GetChild(0).Rotate(0, 0, spin_speed * spin_direction);
    }

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
            if (gameObject.GetComponent<BaseRing>().GetType() == neighbourRings[i].GetComponent<BaseRing>().GetType())
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

    public abstract Dictionary<RingEffectType, object> GetEffect();
}

public enum RingEffectType
    {
        // f = flat, p = percentage
        fDamage,
        pDamage,
        Slow,
        Pierce,
        fRange,
        pRange,
        fFireRate,
        pFireRate,
        Projectile,
        ProjectileSpeed,
        isHoming,
        HomingRange,
        RingReadManipulation,
    }