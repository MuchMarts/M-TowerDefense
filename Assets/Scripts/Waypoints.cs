using UnityEngine;

public class Waypoints : MonoBehaviour
{
    // Array of waypoints that the enemy will follow
    public static Transform[] points;

    void Awake()
    {
        Debug.Log("Waypoints Awake");
        points = new Transform[transform.childCount];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = transform.GetChild(i);
        }
    }
}
