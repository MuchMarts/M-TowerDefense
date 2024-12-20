using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 target;
    private Transform targetTransform;
    private Vector3 _target;

    public float baseSpeed;
    public int basePierce;
    public float baseDamage;
    private float damage;
    private int pierce;
    private float speed;

    public bool isHoming;
    public float homingRadius;
    public bool isTimed;
    public float timeToLive;
    private float timeAlive = 0f;

    public bool isSplash;
    public float splashRadius;

    public bool isTrueDamage = false;

    public List<Buff> buffs;

    private TargetPriority priority = TargetPriority.Closest;
    private TargetType targetType = TargetType.Enemy;

    void Awake()
    {
        damage = baseDamage;
        pierce = basePierce;
        speed = baseSpeed;
    }

    public void Seek(Transform _target)
    {
        target = _target.position;
        targetTransform = _target;

        if (isHoming)
        {
            InvokeRepeating("UpdateTarget", 0f, 0.5f);
        }
    }
    
    private void UpdateTarget()
    {
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
                enemy.GetComponent<Enemy>().TakeDamage(damage, gameObject, isTrueDamage, buffs);
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
            c.GetComponent<Enemy>().TakeDamage(damage, gameObject, isTrueDamage, buffs);
        }
        pierce -= c.GetComponent<Enemy>().pierce_armour;
    }

    void Update()
    {
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
        Destroy(gameObject);
    }

    public void SetModifier(ProjectileModifier projectileModifier)
    {
        if (projectileModifier == null) return;
        damage = projectileModifier.damage;
        pierce = projectileModifier.pierce;
        speed = projectileModifier.speed;
        isHoming = projectileModifier.isHoming;
        homingRadius = projectileModifier.homingRadius;
    }
}

public class ProjectileModifier
{
    public float damage;
    public int pierce;
    public float speed;
    public bool isHoming;
    public float homingRadius;
    
    private float baseDamage;
    private int basePierce;
    private float baseSpeed;
    private bool baseIsHoming;
    private float baseHomingRadius;

    public ProjectileModifier(float _damage, int _pierce, float _speed, bool _isHoming = false, float _homingRadius = 0f)
    {
        damage = _damage;
        pierce = _pierce;
        speed = _speed;
        isHoming = _isHoming;
        homingRadius = _homingRadius;

        baseDamage = _damage;
        basePierce = _pierce;
        baseSpeed = _speed;
        baseIsHoming = _isHoming;
        baseHomingRadius = _homingRadius;
    }

    public void UpdateProjectile(Projectile proj)
    {
        damage += proj.baseDamage - baseDamage;
        pierce += proj.basePierce - basePierce;
        speed += proj.baseSpeed - baseSpeed;
        isHoming = proj.isHoming;
        homingRadius = proj.homingRadius;

        baseDamage = proj.baseDamage;
        basePierce = proj.basePierce;
        baseSpeed = proj.baseSpeed;
        baseIsHoming = proj.isHoming;
        baseHomingRadius = proj.homingRadius;
    }
}
