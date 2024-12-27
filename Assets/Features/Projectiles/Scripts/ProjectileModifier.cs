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

    public void UpdateProjectile(ProjectileSO proj)
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