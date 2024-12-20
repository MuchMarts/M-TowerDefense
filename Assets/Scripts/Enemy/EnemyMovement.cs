using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 10f;
    public float speedModifier = 1f;
    // Handle next waypoint in map
    private Transform target;
    private int waypointIndex = 0;

    public int GetWaypointIndex()
    {
        return waypointIndex;
    }

    void Start()
    {
        // Set the first waypoint as the target
        target = Waypoints.points[0];
    }
    
    private void GetNextWaypoint()
    {
        if (waypointIndex >= Waypoints.points.Length - 1)
        {
            return;
        }

        waypointIndex++;
        target = Waypoints.points[waypointIndex];
    }

    void Update()
    {
        Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized * speed * speedModifier * Time.deltaTime, Space.World);
        Vector3 lookDirection = target.position - transform.position;
        if (lookDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10f);
        }

        if (0.2f >= Vector3.Distance(transform.position, target.position))
        {
            GetNextWaypoint();
        }
    }
}
