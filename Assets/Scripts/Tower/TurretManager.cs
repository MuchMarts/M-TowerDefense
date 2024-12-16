using System;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    [Header("Turret Settings")]
    [SerializeField]
    private GameObject projectileObject;
    [SerializeField]
    private float range = 15f;
    [SerializeField]
    private float baseAttackSpeed = 1f;
    public float turnSpeed = 10f;
    private SphereCollider rangeCollider;
    private List<GameObject> inRangeTargets;
    private GameObject currentTarget;
    private float attackCountdown = 0f;
    private enum targetPriority { First, Last, Closest };
    [SerializeField]
    private targetPriority priority = targetPriority.Closest;
    // Idea: Add a target type to the turret, so it can target enemies or path points
    // Imagine a tower that lays mines or spikes on the path
    private enum targetType { Enemy, Path};
    [SerializeField]
    private targetType target = targetType.Enemy;



    void Start()
    {
        inRangeTargets = new List<GameObject>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        rangeCollider = GetComponent<SphereCollider>();
        rangeCollider.radius = range;
    }

    private GameObject FirstTarget()
    {
        int lowestWaypointIndex = int.MaxValue;
        float shortestDistanceToPreviousWaypoint = Mathf.Infinity;
        GameObject firstestTarget = null;
        for (int i = 0; i < inRangeTargets.Count; i++)
        {
            int waypointIndex = inRangeTargets[i].GetComponent<EnemyMovement>().GetWaypointIndex();
            
            // Break out, if the target is at the first waypoint
            if (waypointIndex == 0)
            {
                Debug.Log("Targets next waypoint is 0");
                return inRangeTargets[i];
            }
            
            if (waypointIndex < lowestWaypointIndex)
            {
                lowestWaypointIndex = waypointIndex;
                firstestTarget = inRangeTargets[i];
            }
            else if (waypointIndex == lowestWaypointIndex)
            {
                float distanceToPreviousWaypoint = Vector3.Distance(inRangeTargets[i].transform.position, Waypoints.points[waypointIndex-1].position);
                if (distanceToPreviousWaypoint < shortestDistanceToPreviousWaypoint)
                {
                    shortestDistanceToPreviousWaypoint = distanceToPreviousWaypoint;
                    firstestTarget = inRangeTargets[i];
                }
            } 
        }

        return firstestTarget;
    }

    private GameObject LastTarget()
    {
        int highestWaypoinyIndex = -1;
        float shortestDistanceToNextWaypoint = Mathf.Infinity;
        GameObject lastestTarget = null;
        for (int i = 0; i < inRangeTargets.Count; i++)
        {
            int waypointIndex = inRangeTargets[i].GetComponent<EnemyMovement>().GetWaypointIndex();
            
            if (waypointIndex > highestWaypoinyIndex)
            {
                highestWaypoinyIndex = waypointIndex;
                lastestTarget = inRangeTargets[i];
            }
            else if (waypointIndex == highestWaypoinyIndex)
            {
                float distanceToNextWaypoint = Vector3.Distance(inRangeTargets[i].transform.position, Waypoints.points[waypointIndex].position);
                if (distanceToNextWaypoint < shortestDistanceToNextWaypoint)
                {
                    shortestDistanceToNextWaypoint = distanceToNextWaypoint;
                    lastestTarget = inRangeTargets[i];
                }
            } 
        }

        return lastestTarget;

    }

    private GameObject ClosestTarget()
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
        return nearestTarget;
    }
    private GameObject TargetSelection()
    {
        switch (priority)
        {
            case targetPriority.First:
                return FirstTarget();
            case targetPriority.Last:
                return LastTarget();
            case targetPriority.Closest:
                return ClosestTarget();
            default:
                return null;
        }

    } 

    void UpdateTarget()
    {
        currentTarget = TargetSelection();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            inRangeTargets.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
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
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        gameObject.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if (attackCountdown <= 0f)
        {
            Shoot();
            attackCountdown = 1f / baseAttackSpeed;
        }

        attackCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        Debug.Log("Pew");
    }

    void OnDrawGizmosSelected()
    {
        // Offset the range indicator to the center of the tower
        Vector3 position = transform.position;
        Gizmos.DrawWireSphere(position, range);
    }    
}
