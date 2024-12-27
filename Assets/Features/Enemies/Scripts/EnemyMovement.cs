using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private float speed;
    public float speedModifier = 1f;
    // Handle next waypoint in map
    private Transform target;
    private int waypointIndex = 0;
    private Enemy enemy;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        if (enemy == null)
        {
            Debug.LogError("EnemyMovement script attached to object without Enemy script");
        }
    }

    void Start()
    {
        // Set the first waypoint as the target
        target = Waypoints.points[0];
        speed = enemy.GetSpeed();
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

    public int GetWaypointIndex()
    {
        return waypointIndex;
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

    internal void ResetWaypoint()
    {
        waypointIndex = 0;
        target = Waypoints.points[0];
    }
}
