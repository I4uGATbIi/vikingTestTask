using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : UnitStats
{
    public static event HealthHandler playerHPisZero;
    public static event HealthHandler playerDamageTaken;
    public PlayerStats()
    {
        ResetStat();
    }

    public override void ResetStat()
    {
        MaxHp = 20;
        Damage = 1;
        WalkSpeed = 2;
        RunSpeed = 5;
        base.ResetStat();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        playerDamageTaken?.Invoke(this);
    }

    public override void CheckIfDead()
    {
        base.CheckIfDead();
        playerHPisZero?.Invoke(this);
    }
}