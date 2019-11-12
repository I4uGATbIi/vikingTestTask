using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : UnitStats
{
    public event HealthHandler playerHPisZero;
    public event HealthHandler playerDamageTaken;
    public event HealthHandler playerHealingTaken;

    public override float CurrentHp
    {
        protected set
        {
            base.CurrentHp = value;
            var playerTypeEvent = value < 0 ? playerDamageTaken : playerHealingTaken;
            playerTypeEvent?.Invoke(this);
        }
    }

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

    public override bool CheckIfDead()
    {
        if (!base.CheckIfDead())
        {
            return false;
        }
        playerHPisZero?.Invoke(this);
        return true;
    }
}