using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    private EnemySO enemyData;
    
    private float health;

    private TowerManager towerManager = TowerManager.Instance;
    private BuffManager buffManager = BuffManager.Instance;
    private EnemyMovement enemyMovement;
    void Start() // This might cause bugs with object pooling
    {
        enemyMovement = GetComponent<EnemyMovement>();
        health = enemyData.baseHealth;
    }

    public void SetEnemyData(EnemySO data)
    {
        enemyData = data;
    } 

    // trueDamage ignores armour, source is the object that caused the damage
    public void TakeDamage(float damage, bool trueDamage = false, List<BuffSO> buffs = null)
    {
        float dmg = damage;

        if (!trueDamage)
        {
            dmg = damage - enemyData.baseDamageArmour;
            if (dmg < 0f) dmg = 0f;
        }
        health -= dmg;
        
        if (health <= 0)
        {
            towerManager.AddKill(enemyData.baseKillValue);
            Die();
        }

        // If not dead, apply buffs
        if (buffs != null)
        {
            foreach (BuffSO buff in buffs)
            {
                buffManager.ApplyBuff(gameObject.GetComponent<Enemy>(), buff);
            }
        }

    }
    
    public void Die(){
        EnemyManager.Instance.UnregisterEnemy(enemyData, gameObject);
    }

    public void ResetEnemyData()
    {
        health = enemyData.baseHealth;
        
        if (enemyMovement != null) {
            enemyMovement.ResetWaypoint();
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

    public int GetPierceArmour()
    {
        return enemyData.basePierceArmour;
    }

    public float GetSpeed()
    {
        return enemyData.baseSpeed;
    }

    public int GetKillValue()
    {
        return enemyData.baseKillValue;
    }

}

