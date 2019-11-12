using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStats : UnitStats
{
    public event HealthHandler monsterHPisZero;
    public event HealthHandler monsterDamageTaken;

    public float TimesDied { get; private set; }

    public MonsterStats()
    {
        TimesDied = 0;
        ResetStat();
    }

    public void ReviveStats()
    {
        MaxHp = 1 + TimesDied;
        base.ResetStat();
    }

    public override void ResetStat()
    {
        TimesDied = 0;
        MaxHp = 1;
        Damage = 1;
        WalkSpeed = 1.5f;
        RunSpeed = 5;
        base.ResetStat();
    }

    public override void TakeDamage(float damage)
    {
        if (CurrentHp <= 0)
            return;
        base.TakeDamage(damage);
        monsterDamageTaken?.Invoke(this);
    }

    public override bool CheckIfDead()
    {
        if (!base.CheckIfDead())
        {
            return false;
        }
        monsterHPisZero?.Invoke(this);
        TimesDied++;
        return true;
    }
}
