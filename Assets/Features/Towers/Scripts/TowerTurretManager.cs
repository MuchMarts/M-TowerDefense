using System.Collections.Generic;
using UnityEngine;

public class TowerTurretManager : MonoBehaviour
{

    public TowerSO towerData;    
    public Transform firePoint;
    
    // Base attributes can be modified by rings
    private float fireRate;
    private float range;
    private float turnSpeed;
    private ProjectileSO projectile;
    private ProjectileModifier projectileModifier;

    // Attributes needed for logic
    private float attackCountdown = 0f;
    private float timeSinceLastShot = 0f;

    private TargetType targetType;
    protected GameObject currentTarget;

    // Referenced components
    private TowerRingManager ringManager;
    protected Vector3 originalPosition;

    // Time to reset rotation for the turret
    private float timeToResetRotation = 3f;
    private float countdownResetRotation = 0f;

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
    }

    void Start()
    {
        if (towerData == null)
        {
            Debug.LogError("Tower data is null, Tower: " + gameObject.name);
            return;
        }

        fireRate = towerData.baseFireRate;
        range = towerData.baseRange;
        turnSpeed = towerData.baseTurnSpeed;
        projectile = towerData.baseProjectile;
        targetType = towerData.targetingType;
        ResetTurret();
    }

    public void SetTowerData(TowerSO towerSO)
    {
        towerData = towerSO;
    }

    void ResetTurret()
    {
        range = towerData.baseRange;
        fireRate = towerData.baseFireRate;
        projectile = towerData.baseProjectile;

        ProjectileModifier projMod = new(projectile);
        projectileModifier = projMod;
    }

    // On RingeStackChange calculate the new projectile modifier
    public void OnRingStackChanged()
    {
        ResetTurret();
        Debug.Log("Ring stack changed");
        
        List<RingEffect> ringEffects = ringManager.GetRingEffects();
        
        if (ringEffects == null) {
            Debug.LogWarning("Ring effects are null, Tower: " + gameObject.transform.parent.name);
            return;
        }

        for(int index = 0; index < ringEffects.Count; index++)
        {
           RingEffect effect = ringEffects[index];
            
            // Apply the ring effect to the tower
            switch (effect.type)
              {
                case RingEffectType.fRange:
                    range += (float)effect.value;
                    break;
                case RingEffectType.pRange:
                    range *= (float)effect.value;
                    break;
                case RingEffectType.fFireRate:
                    fireRate += (float)effect.value;
                    break;
                case RingEffectType.pFireRate:
                    fireRate *= (float)effect.value;
                    break;
                case RingEffectType.fDamage:
                    projectileModifier.damage += (float)effect.value;
                    break;
                case RingEffectType.pDamage:
                    projectileModifier.damage *= (float)effect.value;
                    break;
                case RingEffectType.Pierce:
                    projectileModifier.pierce += (int)effect.value;
                    break;
                case RingEffectType.Projectile:
                    projectile = (ProjectileSO)effect.value;
                    projectileModifier.ChangeProjectile(projectile);
                    break;
                case RingEffectType.isHoming:
                    projectileModifier.isHoming = (bool)effect.value;
                    break;
                case RingEffectType.HomingRange:
                    projectileModifier.homingRadius += (float)effect.value;
                    break;
                case RingEffectType.isTimed:
                    projectileModifier.isTimed = (bool)effect.value;
                    break;
                case RingEffectType.TimeToLive:
                    projectileModifier.timeToLive += (float)effect.value;
                    break;
                case RingEffectType.isSplash:
                    projectileModifier.isSplash = (bool)effect.value;
                    break;
                case RingEffectType.SplashRadius:
                    projectileModifier.splashRadius += (float)effect.value;
                    break;
                case RingEffectType.isTrueDamage:
                    projectileModifier.isTrueDamage = (bool)effect.value;
                    break;
                case RingEffectType.Buff:
                    projectileModifier.AddBuff((BuffSO)effect.value);
                    break;
                default:
                    Debug.LogWarning("Ring effect not implemented: " + effect.type);
                    break;
              }
        }

        if (projectileModifier == null) 
        {
            Debug.LogError("Projectile modifier is null, Tower: " + gameObject.transform.parent.name);
        }
        DebugAllModifiers();
    }

    void DebugAllModifiers()
    {
        string debugString = "Projectile modifiers: \n";
        debugString += "Damage: " + projectileModifier.damage + "\n";
        debugString += "Pierce: " + projectileModifier.pierce + "\n";
        debugString += "Speed: " + projectileModifier.speed + "\n";
        debugString += "Homing: " + projectileModifier.isHoming + "\n";
        debugString += "Homing Radius: " + projectileModifier.homingRadius + "\n";
        debugString += "Timed: " + projectileModifier.isTimed + "\n";
        debugString += "Time to live: " + projectileModifier.timeToLive + "\n";
        debugString += "Splash: " + projectileModifier.isSplash + "\n";
        debugString += "Splash Radius: " + projectileModifier.splashRadius + "\n";
        debugString += "True Damage: " + projectileModifier.isTrueDamage + "\n";
        debugString += "Buffs: ";
        projectileModifier.buffs.ForEach(buff => debugString += buff.buffName + ", ");
        debugString += "\n";
        debugString += "Projectile: " + projectile.projectileName + "\n";
        debugString += "Fire rate: " + fireRate + "\n";
        debugString += "Range: " + range + "\n";
        Debug.Log(debugString);
    }

    void UpdateTarget()
    {
        List<GameObject> inRangeTargets = EnemyManager.Instance.GetEnemiesInRange(transform.position, range);
        GameObject newTarget = Targeting.getTarget(towerData.targetingPriority, targetType, transform, inRangeTargets);
        currentTarget = newTarget;
    }

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

        float clampedX = Mathf.Clamp(rotation.x, -50f, 50f);
        float clampedZ = Mathf.Clamp(rotation.z, -50f, 50f);

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
            Debug.LogWarning("Turret: " + gameObject.transform.parent.name + " has not shot in 10 seconds");
        }
        if (attackCountdown > 0f)
        {
            attackCountdown -= Time.deltaTime;
            return;
        }
        if (fireRate <= 0f)
        {
            Debug.LogError("Attack speed is 0 or less, Turret: " + gameObject.transform.parent.name);
            return;
        }

        Debug.DrawRay(firePoint.position, direction, Color.blue);
        Debug.DrawRay(firePoint.position, currentTarget.transform.position - firePoint.position, Color.red);

        if (Vector3.Angle(firePoint.transform.forward, direction) < 20f) 
        {
            Shoot();
            attackCountdown = 1f / fireRate;
        }

        timeSinceLastShot += Time.deltaTime;
    }

    void Shoot()
    {
        timeSinceLastShot = 0f;

        GameObject projectile = ProjectileManager.Instance.SpawnProjectile(this.projectile, firePoint.position, firePoint.rotation);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        
        if (projectileScript == null)
        {
            Debug.LogError("Projectile script is null for projectile: " + projectile.name + " on turret: " + gameObject.name);
            ProjectileManager.Instance.DespawnProjectile(this.projectile, projectile);
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

    public GameObject ShowTurretRange(GameObject rangeIndicator)
    {
        Vector3 position = transform.position;
        
        GameObject indicator = Instantiate(rangeIndicator);
        indicator.transform.position = new Vector3(position.x, position.y - 1f, position.z);
        indicator.transform.localScale = new Vector3(range * 2, 1, range * 2);
        indicator.SetActive(true);
        return indicator;
    }

    public void HideTurretRange(GameObject rangeIndicator)
    {
        Destroy(rangeIndicator);
    }
}
