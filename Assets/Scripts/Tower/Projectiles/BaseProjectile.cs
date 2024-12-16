using System;
using Unity.VisualScripting;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    private Vector3 target;

    public float baseSpeed = 70f;
    public int basePierce = 1;
    public float baseDamage = 1f;

    private float damage;
    private int pierce;
    private float speed;

    void Awake()
    {
        damage = baseDamage;
        pierce = basePierce;
        speed = baseSpeed;
    }

    public void Seek(Transform _target)
    {
        target = _target.position;
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Enemy"))
        {
            HitTarget(c);
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

        Vector3 direction = target - transform.position;
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
