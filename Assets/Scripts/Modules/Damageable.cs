using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    Health health;

    private void Start()
    {
        health = GetComponent<Health>();
    }

    public void TakeDamage(float damage, GameObject source)
    {
        if (health == null)
        {
            Debug.Log("Object does not have health component");
            return;
        }
        health.ChangeHealth(-damage, source);
    }

}
