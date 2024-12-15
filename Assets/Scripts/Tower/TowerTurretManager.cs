using System.Collections.Generic;
using UnityEngine;

public class TowerTurretManager : MonoBehaviour
{
    [Header("Tower Manager Settings")]
    [SerializeField]
    private GameObject towerTurret, projectilePrefab;
    [SerializeField]
    private float range = 15f;
    [SerializeField]
    private float baseAttackSpeed = 1f;
    [SerializeField]
    private SphereCollider rangeCollider;
    private List<GameObject> inRangeTargets;
    private GameObject currentTarget;
    private float attackCountdown = 0f;


    void Start()
    {
        inRangeTargets = new List<GameObject>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        rangeCollider.radius = range;
    }

    void UpdateTarget()
    {
        GameObject nearestTarget = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject target in inRangeTargets)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            if (distanceToTarget < shortestDistance)
            {
                shortestDistance = distanceToTarget;
                nearestTarget = target;
            }
        }
        currentTarget = nearestTarget;
    }
    
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered Enter");
        if (other.CompareTag("Enemy"))
        {
            inRangeTargets.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Triggered Exit");
        if (other.CompareTag("Enemy"))
        {
            inRangeTargets.Remove(other.gameObject);
        }
    }

    void Update()
    {
        if (currentTarget == null)
        {
            return;
        }

        Vector3 direction = currentTarget.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = lookRotation.eulerAngles;
        towerTurret.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

    }

    void OnDrawGizmosSelected()
    {
        // Offset the range indicator to the center of the tower
        Vector3 position = transform.position + new Vector3(0.5f, 0.5f, 0.5f);
        Gizmos.DrawWireSphere(position, range);
    }    
}
