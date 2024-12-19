using UnityEngine;
using UnityEngine.Video;

public enum BuffType { Speed, Regen, Slow, Freeze, Poison }
public class Buff : MonoBehaviour
{
    public BuffType type;
    public float value;
    public float duration;
    public float tickRate;
    public float tickDamage;
    public float tickTimer;
    public GameObject source;

    void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            Destroy(gameObject);
        }

        if (tickRate > 0)
        {
            tickTimer -= Time.deltaTime;
            if (tickTimer <= 0)
            {
                tickTimer = tickRate;
                ApplyTick();
            }
        }
    }

    void ApplyTick()
    {
        switch (type)
        {
            case BuffType.Poison:
                GetComponent<Enemy>().TakeDamage(tickDamage, source);
                break;
            case BuffType.Slow:
                GetComponent<Enemy>().BuffSpeed(value);
                break;
            case BuffType.Freeze:
                GetComponent<Enemy>().BuffSpeed(value);
                break;
            case BuffType.Regen:
                GetComponent<Enemy>().Heal(value);
                break;
            case BuffType.Speed:
                GetComponent<Enemy>().BuffSpeed(value);
                break;
        }
    }


    void OnDestroy()
    {
        switch (type)
        {
            case BuffType.Poison:
                break;
            case BuffType.Slow:
                GetComponent<Enemy>().BuffSpeed(1);
                break;
            case BuffType.Freeze:
                GetComponent<Enemy>().BuffSpeed(1);
                break;
            case BuffType.Regen:
                GetComponent<Enemy>().Heal(0);
                break;
            case BuffType.Speed:
                GetComponent<Enemy>().BuffSpeed(1);
                break;
        }
    }

}
