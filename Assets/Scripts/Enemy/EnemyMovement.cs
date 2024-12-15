using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 10f;

    // Handle next waypoint in map
    private Transform target;
    private int waypointIndex = 0;


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
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.2f)
        {
            GetNextWaypoint();
        }
    }
}
