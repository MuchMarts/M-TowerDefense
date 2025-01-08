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
            c.GetComponent<Enemy>().Die();
        }
    }
}
