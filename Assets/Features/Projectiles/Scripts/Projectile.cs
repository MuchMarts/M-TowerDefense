using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public ProjectileSO projectileData;

    private Vector3 target;
    private Transform targetTransform;
    private Vector3 _target;

    private float damage;
    private int pierce;
    private float speed;

    private bool isHoming;
    private float homingRadius;
    private bool isTimed;
    private float timeToLive;
    private float timeAlive = 0f;

    private bool isSplash;
    private float splashRadius;

    private bool isTrueDamage = false;

    private List<BuffSO> buffs;

    private TargetPriority priority = TargetPriority.Closest;
    private TargetType targetType = TargetType.Enemy;

    void Start()
    {
        damage = projectileData.damage;
        pierce = projectileData.pierce;
        speed = projectileData.speed;
    }

    // TODO: Target should be above the ground, not on the ground. 
    // Add a height offset to the target position, and make sure homing works correctly. 
    public void Seek(Transform _target)
    {
        target = _target.position;
        targetTransform = _target;

        if (isHoming)
        {
            InvokeRepeating("UpdateTarget", 0f, 0.5f);
        }
    }
    
    private bool isLookingForTargets = false;
    private void UpdateTarget()
    {
        isLookingForTargets = true;
        if (!isHoming) return;
        if (targetTransform != null)
        {
            return;
        }

        List<GameObject> inRangeTargets = EnemyManager.Instance.GetEnemiesInRange(transform.position, homingRadius);
        GameObject newTarget = Targeting.getTarget(priority, targetType, transform, inRangeTargets);

        if (newTarget == null) 
        {
            return;
        }
        target = newTarget.transform.position;
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Enemy"))
        {
            HitTarget(c);
        }

        if (c.CompareTag("Ground"))
        {
            HitTarget(c, false);
        }

    }

    void HitTarget(Collider c, bool isEnemy = true)
    {   
        if (isSplash)
        {
            List<GameObject> inRangeTargets = EnemyManager.Instance.GetEnemiesInRange(gameObject.transform.position, splashRadius);
            foreach (GameObject enemy in inRangeTargets)
            {
                if (enemy == null) continue;
                enemy.GetComponent<Enemy>().TakeDamage(damage, isTrueDamage, buffs);
                // Could be changed so the splash is triggered by hitting an enemy thus allowing for multiple splashes
                DestroyProjectile();
            }
            return;
        }

        if (!isEnemy)
        {
            DestroyProjectile();
            return;
        }

        if (pierce < 0)
        {
            DestroyProjectile();
        } else {
            c.GetComponent<Enemy>().TakeDamage(damage, isTrueDamage, buffs);
        }

        pierce -= c.GetComponent<Enemy>().GetPierceArmour();
    }

    void Update()
    {
        // Turn on homing if it's not
        if (isHoming && !isLookingForTargets)
        {
            UpdateTarget();
        }

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        if (isHoming && targetTransform != null) {
            _target = targetTransform.position;
            target = _target; //Keep last target
        } else {
            _target = target;
        }

        if (isTimed)
        {
            timeAlive += Time.deltaTime;
            if (timeAlive >= timeToLive)
            {
                DestroyProjectile();
                return;
            }
        }

        Vector3 direction = _target - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            DestroyProjectile();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        Quaternion Rotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, Rotation, Time.deltaTime * 10f).eulerAngles;
        gameObject.transform.rotation = Quaternion.Euler(rotation);
    }

    private void DestroyProjectile()
    {
        GameObjectPoolManager.Instance.ReleaseObject(projectileData, gameObject);
    }

    public void SetModifier(ProjectileModifier projectileModifier)
    {
        if (projectileModifier == null) return;
        damage = projectileModifier.damage;
        pierce = projectileModifier.pierce;
        speed = projectileModifier.speed;
        isHoming = projectileModifier.isHoming;
        homingRadius = projectileModifier.homingRadius;
        isTimed = projectileModifier.isTimed;
        timeToLive = projectileModifier.timeToLive;
        isSplash = projectileModifier.isSplash;
        splashRadius = projectileModifier.splashRadius;
        isTrueDamage = projectileModifier.isTrueDamage;
        buffs = projectileModifier.buffs;
    }
}


