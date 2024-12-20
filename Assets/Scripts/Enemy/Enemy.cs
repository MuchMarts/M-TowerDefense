using UnityEngine;
using UnityEngine.Events;
public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float health;

    public int pierce_armour = 0; // Determines by how much projectiles pierce gets reduced
    public int value = 1; // How much money the player gets for killing

    private TowerManager towerManager = TowerManager.Instance;
    private EnemyMovement enemyMovement;
    void Start()
    {
        EnemyManager.Instance.RegisterEnemy(gameObject);
        enemyMovement = GetComponent<EnemyMovement>();
    }

    public void TakeDamage(float damage, GameObject source)
    {
        health -= damage;
        if (health <= 0)
        {
            if (source.GetComponent<Projectile>() != null)
            {
                towerManager.AddKill(gameObject.GetComponentInParent<Enemy>().value);
            }
            DestroyObject();
        }
    }

    public void Heal(float value)
    {
        health += value;
    }

    public void ChangeSpeedModifier(float modifier)
    {
        enemyMovement.speedModifier = modifier;
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        EnemyManager.Instance.UnregisterEnemy(gameObject);
    }
}
