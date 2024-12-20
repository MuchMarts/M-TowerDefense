using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum BuffType { Regen, Poison, Bleed, Slow, Stun, Speed}

public class Buff
{
    public BuffType type;
    public float duration;
    public float tickRate; // Per second
    public float value;
    public GameObject source;
}

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance;

    private Dictionary<Enemy, List<Buff>> activeBuffs = new ();

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one BuffManager in scene!");
            return;
        }
        Instance = this;
    }

    private bool HasBuff(Enemy enemy, BuffType type)
    {
        if (activeBuffs.ContainsKey(enemy))
        {
            return activeBuffs[enemy].Exists(b => b.type == type);
        }
        return false;
    }

    public void ApplyBuff(Enemy enemy, Buff buff)
    {
        if (!activeBuffs.ContainsKey(enemy))
        {
            activeBuffs.Add(enemy, new List<Buff>());
        }

        // Check if the buff is already applied
        // Not all buffs can be stacked
        switch (buff.type)
        {
            case BuffType.Regen:
                if (HasBuff(enemy, BuffType.Regen)) return;
                activeBuffs[enemy].Add(buff);
                break;
            case BuffType.Poison:
                if (HasBuff(enemy, BuffType.Poison)) return;
                activeBuffs[enemy].Add(buff);
                break;
            case BuffType.Bleed:
                if (HasBuff(enemy, BuffType.Bleed)) return;
                activeBuffs[enemy].Add(buff);
                break;
            case BuffType.Slow:
                if (HasBuff(enemy, BuffType.Slow)) return;
                activeBuffs[enemy].Add(buff);
                break;
            case BuffType.Stun:
                if (HasBuff(enemy, BuffType.Stun)) return;
                activeBuffs[enemy].Add(buff);
                break;
            case BuffType.Speed:
                if (HasBuff(enemy, BuffType.Speed)) return;
                activeBuffs[enemy].Add(buff);
                break;
            default:
                Debug.LogError("Buff type not implemented: " + buff.type);
                return;
        }

        StartCoroutine(StartBuff(enemy, buff));
    }

    IEnumerator StartBuff(Enemy enemy, Buff buff)
    {
        int ticks = (int)Math.Floor(buff.duration / buff.tickRate);

        switch (buff.type)
        {
            case BuffType.Regen:
                for (int i = 0; i < ticks; i++)
                {
                    enemy.Heal(buff.value);
                    yield return new WaitForSeconds(buff.duration / buff.tickRate);
                }
                break;
            case BuffType.Poison:
                for (int i = 0; i < ticks; i++)
                {
                    enemy.TakeDamage(buff.value, buff.source);
                    yield return new WaitForSeconds(buff.duration / buff.tickRate);
                }
                break;
            case BuffType.Bleed:
                yield return new WaitForSeconds(buff.duration);
                enemy.TakeDamage(buff.value, buff.source);
                break;
            case BuffType.Slow:
                enemy.ChangeSpeedModifier(buff.value);
                yield return new WaitForSeconds(buff.duration);
                enemy.ChangeSpeedModifier(1);
                break;
            case BuffType.Stun:
                enemy.ChangeSpeedModifier(0);
                yield return new WaitForSeconds(buff.duration);
                enemy.ChangeSpeedModifier(1);
                break;
            case BuffType.Speed:
                enemy.ChangeSpeedModifier(buff.value);
                yield return new WaitForSeconds(buff.duration);
                enemy.ChangeSpeedModifier(1);
                break;
            default:
                Debug.LogError("Buff type not implemented: " + buff.type);
                break;
        }

        RemoveBuff(enemy, buff);
    }

    void RemoveBuff(Enemy enemy, Buff buff)
    {
        if (activeBuffs.ContainsKey(enemy))
        {
            activeBuffs[enemy].Remove(buff);
        }
    }

}
