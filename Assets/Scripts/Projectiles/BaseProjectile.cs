using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
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
            DestroyProjectile();
        }

    }

    void HitTarget(Collider c)
    {
        if (pierce <= 0)
        {
            DestroyProjectile();
        } else {
            Debug.Log("Pierce: " + pierce + " Damage: " + damage);
            c.GetComponent<Damageable>().TakeDamage(damage);

        }
        pierce--;
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
        Debug.Log("Setting modifier: " + projectileModifier.damage + " " + projectileModifier.pierce + " " + projectileModifier.speed);
        Debug.Log("Current: " + damage + " " + pierce + " " + speed);
        
        damage = projectileModifier.damage;
        pierce = projectileModifier.pierce;
        speed = projectileModifier.speed;
    }
}

public class ProjectileModifier
{
    public float damage;
    public int pierce;
    public float speed;
    
    private float baseDamage;
    private int basePierce;
    private float baseSpeed;


    public ProjectileModifier(float _damage, int _pierce, float _speed)
    {
        damage = _damage;
        pierce = _pierce;
        speed = _speed;

        baseDamage = _damage;
        basePierce = _pierce;
        baseSpeed = _speed;
    }

    public void UpdateProjectile(BaseProjectile proj)
    {
        damage += proj.baseDamage - baseDamage;
        pierce += proj.basePierce - basePierce;
        speed += proj.baseSpeed - baseSpeed;

        baseDamage = proj.baseDamage;
        basePierce = proj.basePierce;
        baseSpeed = proj.baseSpeed;
    }
}
