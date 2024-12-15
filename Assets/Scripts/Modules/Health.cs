using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{

    public float health = 100;
    public UnityEvent onHealthZero;

    public void ChangeHealth(float amount)
    {
        health += amount;
        if (health <= 0)
        {
            onHealthZero.Invoke();
        }
    }
}
