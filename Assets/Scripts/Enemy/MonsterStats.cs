using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStats : UnitStats
{
    public event HealthHandler monsterHPisZero;
    public event HealthHandler monsterDamageTaken;

    public MonsterStats()
    {
        ResetStat();
    }

    public override void ResetStat()
    {
        MaxHp++;
        Damage = 1;
        WalkSpeed = 1.5f;
        RunSpeed = 5;
        base.ResetStat();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        monsterDamageTaken?.Invoke(this);
    }

    public override void CheckIfDead()
    {
        base.CheckIfDead();
        monsterHPisZero?.Invoke(this);
    }
}
