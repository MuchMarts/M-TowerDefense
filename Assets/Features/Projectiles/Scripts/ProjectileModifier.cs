using System.Collections.Generic;

public class ProjectileModifier
{
    public float damage;
    public int pierce;
    public float speed;
    public bool isHoming;
    public float homingRadius;
    public bool isTimed;
    public float timeToLive;
    public bool isSplash;
    public float splashRadius;
    public bool isTrueDamage;
    public List<BuffSO> buffs;

    // Reference to the last projectile for base values
    private ProjectileSO lastProjectile;

    public ProjectileModifier(ProjectileSO _projectile)
    {
        damage = _projectile.damage;
        pierce = _projectile.pierce;
        speed = _projectile.speed;
        isHoming = _projectile.isHoming;
        homingRadius = _projectile.homingRadius;
        isTimed = _projectile.isTimed;
        timeToLive = _projectile.timeToLive;
        isSplash = _projectile.isSplash;
        splashRadius = _projectile.splashRadius;
        isTrueDamage = _projectile.isTrueDamage;
        buffs = _projectile.buffs;
        lastProjectile = _projectile;
    }

    public void AddBuff(BuffSO _buffs)
    {
        // Ignore duplicates
        if (!buffs.Contains(_buffs))
        {
            buffs.Add(_buffs);
        }
    }

    public void ChangeProjectile(ProjectileSO _projectile)
    {
        // Remove the last projectile values
        // Add the new projectile values
        damage = damage - lastProjectile.damage + _projectile.damage;
        pierce = pierce - lastProjectile.pierce + _projectile.pierce;
        speed = speed - lastProjectile.speed + _projectile.speed;
        homingRadius = homingRadius - lastProjectile.homingRadius + _projectile.homingRadius;
        timeToLive = timeToLive - lastProjectile.timeToLive + _projectile.timeToLive;
        splashRadius = splashRadius - lastProjectile.splashRadius + _projectile.splashRadius;

        // Boolean values are not additive
        if (lastProjectile.isHoming != _projectile.isHoming)
        {
            isHoming = _projectile.isHoming;
        }

        if (lastProjectile.isTimed != _projectile.isTimed)
        {
            isTimed = _projectile.isTimed;
        }

        if (lastProjectile.isSplash != _projectile.isSplash)
        {
            isSplash = _projectile.isSplash;
        }

        if (lastProjectile.isTrueDamage != _projectile.isTrueDamage)
        {
            isTrueDamage = _projectile.isTrueDamage;
        }

        // Remove buffs that are the same as the last projectile
        for (int i = 0; i < lastProjectile.buffs.Count; i++)
        {
            if (buffs.Contains(lastProjectile.buffs[i]))
            {
                buffs.Remove(lastProjectile.buffs[i]);
            }
        }

        // Add buffs that are in the new projectile, ignore duplicates
        for (int i = 0; i < _projectile.buffs.Count; i++)
        {
            if (!buffs.Contains(_projectile.buffs[i]))
            {
                buffs.Add(_projectile.buffs[i]);
            }
        }

        lastProjectile = _projectile;
    }
}