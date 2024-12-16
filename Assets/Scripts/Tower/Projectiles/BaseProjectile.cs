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

    public void SetRingEffects(RingEffects rEffect)
    {

        if (rEffect != null)
        {
            if (rEffect.pierce > 0)
            {
                pierce += rEffect.pierce;
            }
            if (rEffect.damage > 0)
            {
                Debug.Log("Adding rEffect Damage");
                Debug.Log("Damage: " + damage + " Ring Damage: " + rEffect.damage);
                damage += rEffect.damage;
                Debug.Log("Damage: " + damage);
            }
        }
    }
}
