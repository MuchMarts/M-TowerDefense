using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float health;

    public int pierce_armour = 0; // Determines by how much projectiles pierce gets reduced
    public float damage_armour = 0; // Determines by how much damage gets reduced
    public int value = 1; // How much money the player gets for killing

    private TowerManager towerManager = TowerManager.Instance;
    private EnemyMovement enemyMovement;
    void Start()
    {
        EnemyManager.Instance.RegisterEnemy(gameObject);
        enemyMovement = GetComponent<EnemyMovement>();
    }

    // trueDamage ignores armour, source is the object that caused the damage
    public void TakeDamage(float damage, GameObject source, bool trueDamage = false)
    {
        float dmg = damage;

        if (!trueDamage)
        {
            dmg = damage - damage_armour;
            if (dmg < 0f) dmg = 0f;
        }

        health -= dmg;
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
