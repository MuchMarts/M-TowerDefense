using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Finish : MonoBehaviour
{
    public UnityEvent onReachedFinish;

    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Enemy"))
        {
            onReachedFinish.Invoke();
            Destroy(c.gameObject);
        }
    }
}