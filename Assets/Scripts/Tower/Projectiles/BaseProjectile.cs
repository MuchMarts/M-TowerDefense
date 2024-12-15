using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    [SerializeField]
    GameObject projectilePrefab;
    int damage;
    float speed;
    int piercing;
    float timeToLive;

    public void behaviour()
    {
        return;
    }

}
