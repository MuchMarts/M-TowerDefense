using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetPriority { Closest };
public enum TargetType { Enemy };

public class Targeting : MonoBehaviour
{
    public static GameObject getTarget(TargetPriority priority, TargetType type, Transform tower, List<GameObject> inRangeTargets)
    {
        switch (priority)
        {
            case TargetPriority.Closest:
                return GetClosestTarget(tower, inRangeTargets);
            default:
                return null;
        }
    }

    private static GameObject GetClosestTarget(Transform tower, List<GameObject> inRangeTargets)
    {
        GameObject nearestTarget = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject target in inRangeTargets)
        {
            float distanceToTarget = Vector3.Distance(tower.position, target.transform.position);
            if (distanceToTarget < shortestDistance)
            {
                shortestDistance = distanceToTarget;
                nearestTarget = target;
            }
        }
        return nearestTarget;
    }
}
