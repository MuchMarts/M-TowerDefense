using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Destructible : MonoBehaviour
{
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
