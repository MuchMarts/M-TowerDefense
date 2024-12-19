using UnityEngine;
using UnityEngine.Events;
public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float health;

    public int pierce_armour = 0; // Determines by how much projectiles pierce gets reduced
    public int value = 1; // How much money the player gets for killing

    // Additional stats
    [SerializeField]
    private float baseRegen = 0;
    private float regenTimer = 1; 
    private bool regenActive = false;
    private float regen = 0; 
    private TowerManager towerManager = TowerManager.Instance;

    void Awake()
    {
        regen = baseRegen;
    }

    void Start()
    {
        EnemyManager.Instance.RegisterEnemy(gameObject);
    }

    void Update()
    {
        if (regenActive)
        {
            regenTimer -= Time.deltaTime;
            if (regenTimer <= 0)
            {
                health += regen;
                regenTimer = 1;
            }
        }

    }

    public void Regen(float value)
    {
        regen = value + baseRegen;
        regenActive = true;
    }

    public void BuffSpeed(float value)
    {
        if (GetComponent<EnemyMovement>() != null) return;
        GetComponent<EnemyMovement>().buffSpeed(value);
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

    void DestroyObject()
    {
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        EnemyManager.Instance.UnregisterEnemy(gameObject);
    }
}
