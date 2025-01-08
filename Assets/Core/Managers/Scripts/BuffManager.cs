using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private bool HasBuff(Enemy enemy, BuffSO _buffSO)
    {
        if (activeBuffs.ContainsKey(enemy))
        {
            return activeBuffs[enemy].Exists(b => b.buffSO == _buffSO);
        }
        return false;
    }

    public void ApplyBuff(Enemy enemy, BuffSO buffSO)
    {
        if (!activeBuffs.ContainsKey(enemy))
        {
            activeBuffs.Add(enemy, new List<Buff>());
        }


        if( HasBuff(enemy, buffSO) )
        {
            if (buffSO.canStack)
            {   
                activeBuffs[enemy].Find(b => b.buffSO == buffSO).stackCount++;
            }
            return;
        }

        Buff buff = new Buff(buffSO);
        activeBuffs[enemy].Add(buff);

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
                    enemy.Heal(buff.Value);
                    yield return new WaitForSeconds(buff.duration / buff.tickRate);
                }
                break;
            case BuffType.Poison:
                for (int i = 0; i < ticks; i++)
                {
                    enemy.TakeDamage(buff.Value, buff.source);
                    yield return new WaitForSeconds(buff.duration / buff.tickRate);
                }
                break;
            case BuffType.Bleed:
                yield return new WaitForSeconds(buff.duration);
                enemy.TakeDamage(buff.Value, buff.source);
                break;
            case BuffType.Slow:
                enemy.ChangeSpeedModifier(buff.Value);
                yield return new WaitForSeconds(buff.duration);
                enemy.ChangeSpeedModifier(1);
                break;
            case BuffType.Stun:
                enemy.ChangeSpeedModifier(0);
                yield return new WaitForSeconds(buff.duration);
                enemy.ChangeSpeedModifier(1);
                break;
            case BuffType.Speed:
                enemy.ChangeSpeedModifier(buff.Value);
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
