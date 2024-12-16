using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    private Vector3 target;

    public float speed = 70f;
    public int pierce = 1;
    public float damage = 1f;
    public void Seek(Transform _target)
    {
        target = _target.position;
    }

    void OnTriggerEnter(Collider c)
    {
        Debug.Log("Projectile hit something: " + c.name);
        if (c.CompareTag("Enemy"))
        {
            HitTarget(c);
        }

    }

    void HitTarget(Collider c)
    {
        pierce--;
        Debug.Log("Projectile hit target: " + c.name);
        c.GetComponent<Damageable>().TakeDamage(damage);
        if (pierce <= 0)
        {
            DestroyProjectile();
        }
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
            Debug.Log("Projectile missed target");
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
}
