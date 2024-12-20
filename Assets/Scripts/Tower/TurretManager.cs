using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurretManager : MonoBehaviour
{
    [Header("Turret Settings")]
    public GameObject baseProjectile;
    private GameObject currentProjectile;
    private ProjectileModifier projectileModifier;
    public Transform firePoint;
    public float turnSpeed = 10f;


    [Header("Turret Range")]
    public float baseRange = 10f;
    private float range;


    [Header("Turret Attack Speed")]
    public float baseFireRate = 1f;
    private float _fireRate;
    private float FireRate {
        get { if (_fireRate >= 10f) return 10f; return _fireRate;}
        set { _fireRate = value; }
    }
    private float attackCountdown = 0f;
    private float timeSinceLastShot = 0f;


    // Turret targeting
    [Header("Turret Targeting")]
    public TargetPriority target = TargetPriority.Closest;
    public TargetType priority = TargetType.Enemy;
    protected GameObject currentTarget = null;

    // Referenced components
    private TowerRingManager ringManager;

    protected Vector3 originalPosition;


    void Awake()
    {
        originalPosition = transform.localPosition;

        if (EnemyManager.Instance == null)
        {
            Debug.LogError("Enemy Manager is null");
        }

        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        ringManager = gameObject.GetComponentInParent<TowerRingManager>();
        
        if (ringManager == null)
        {
            Debug.LogError("Ring Manager is null, Tower: " + gameObject.transform.parent.name);
        }

        range = baseRange;
        FireRate = baseFireRate;
        currentProjectile = baseProjectile;
    }

    // On RingeStackChange calculate the new projectile modifier
    public void OnRingStackChanged()
    {
        Debug.Log("Ring stack changed");
        List<RingEffect> ringEffects = ringManager.GetRingEffects();
        if (ringEffects == null) {
            Debug.LogWarning("Ring effects are null");
            projectileModifier = null;
            return;
        }
        // Projectile Modifier init values are based on turret base projectile
        Projectile projectile = baseProjectile.GetComponent<Projectile>();
        ProjectileModifier projMod = new(projectile.baseDamage, projectile.basePierce, projectile.baseSpeed);

        for(int index = 0; index < ringEffects.Count; index++)
        {
           RingEffect effect = ringEffects[index];

              switch (effect.ringEffectType)
              {
                case RingEffectType.fRange:
                    range += (float)effect.effectValue;
                    break;
                case RingEffectType.pRange:
                    range *= (float)effect.effectValue;
                    break;
                case RingEffectType.fFireRate:
                    FireRate += (float)effect.effectValue;
                    break;
                case RingEffectType.pFireRate:
                    FireRate *= (float)effect.effectValue;
                    break;
                case RingEffectType.fDamage:
                    projMod.damage += (float)effect.effectValue;
                    break;
                case RingEffectType.pDamage:
                    projMod.damage *= (float)effect.effectValue;
                    break;
                case RingEffectType.Pierce:
                    projMod.pierce += (int)effect.effectValue;
                    break;
                case RingEffectType.Projectile:
                    currentProjectile = (GameObject)effect.effectValue;
                    projMod.UpdateProjectile(currentProjectile.GetComponent<Projectile>());
                    break;
                case RingEffectType.isHoming:
                    projMod.isHoming = (bool)effect.effectValue;
                    break;
                case RingEffectType.HomingRange:
                    projMod.homingRadius = (float)effect.effectValue;
                    break;
                default:
                    Debug.LogWarning("Ring effect not implemented: " + effect.ringEffectType);
                    break;
              }
        }

        Debug.Log("Tower: " + gameObject.name + " has new attributes: " + range + " " + FireRate + " " + currentProjectile.name);

        if (projMod != null) 
        {
            Debug.Log("Projectile modifier created: " + projMod.damage + " " + projMod.pierce + " " + projMod.speed);
        }
        projectileModifier = projMod;
    }

    void UpdateTarget()
    {
        List<GameObject> inRangeTargets = EnemyManager.Instance.GetEnemiesInRange(transform.position, range);
        GameObject newTarget = Targeting.getTarget(target, priority, transform, inRangeTargets);

        if (newTarget == null) 
        {
            currentTarget = null;
            return;
        }
        
        currentTarget = newTarget;
    }
    private float timeToResetRotation = 3f;
    private float countdownResetRotation = 0f;
    protected void ResetXZRotation()
    {
        if (countdownResetRotation > 0f)
        {
            countdownResetRotation -= Time.deltaTime;
            return;
        }

        if (gameObject.transform.rotation.eulerAngles.x != 0 || gameObject.transform.rotation.eulerAngles.z != 0)
        {
            Quaternion targetRotation = Quaternion.Euler(0, gameObject.transform.rotation.eulerAngles.y, 0);
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }
    }

    protected virtual Vector3 RotateTurret()
    {
        if (currentTarget == null)
        {
            ResetXZRotation();
            return Vector3.zero;
        }

        Vector3 direction = currentTarget.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;

        float clampedX = Mathf.Clamp(rotation.x, -20f, 20f);
        float clampedZ = Mathf.Clamp(rotation.z, -20f, 20f);

        gameObject.transform.rotation = Quaternion.Euler(clampedX, rotation.y, clampedZ);
        return direction;
    }

    void Update()
    {
        Vector3 direction = RotateTurret();
        if (direction == Vector3.zero)
        {
            return;
        }

        countdownResetRotation = timeToResetRotation;

        if (timeSinceLastShot > 10f) 
        {
            Debug.LogWarning("Turret: " + gameObject.name + " has not shot in 10 seconds");
        }
        if (attackCountdown > 0f)
        {
            attackCountdown -= Time.deltaTime;
            return;
        }
        if (FireRate <= 0f)
        {
            Debug.LogError("Attack speed is 0 or less, Turret: " + gameObject.name);
            return;
        }

        Debug.DrawRay(firePoint.position, direction, Color.blue);
        Debug.DrawRay(firePoint.position, currentTarget.transform.position - firePoint.position, Color.red);

        if (Vector3.Angle(firePoint.transform.forward, direction) < 20f) 
        {
            Shoot();
            attackCountdown = 1f / FireRate;
        }

        timeSinceLastShot += Time.deltaTime;
    }

    void Shoot()
    {
        timeSinceLastShot = 0f;

        GameObject projectile = Instantiate(currentProjectile, firePoint.position, firePoint.rotation);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        
        if (projectileScript == null)
        {
            Debug.LogError("Projectile script is null for projectile: " + projectile.name + " on turret: " + gameObject.name);
            Destroy(projectile);
            return;
        }

        projectileScript.SetModifier(projectileModifier);
        projectileScript.Seek(currentTarget.transform);
    }

    void OnDrawGizmosSelected()
    {
        Vector3 position = transform.position;
        Gizmos.DrawWireSphere(position, range);
    }    
}
