using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{

    public float health = 100;
    public UnityEvent onHealthZero;

    private TowerManager towerManager = TowerManager.Instance;

    public void ChangeHealth(float amount, GameObject source)
    {
        health += amount;
        if (health <= 0)
        {
            if (source.GetComponent<Projectile>() != null)
            {
                towerManager.AddKill(gameObject.GetComponentInParent<Enemy>().value);
            }

            onHealthZero.Invoke();
        }
    }
}
