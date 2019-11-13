using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : UnitStats
{
    public event StatsHandler playerHPisZero;
    public event StatsHandler playerDamageTaken;
    public event StatsHandler playerHealingTaken;
    public event StatsHandler playerStaminaChanged;

    public override float CurrentHp
    {
        protected set
        {
            var playerTypeEvent = value < CurrentHp ? playerDamageTaken : playerHealingTaken;
            base.CurrentHp = value;
            playerTypeEvent?.Invoke(this);
        }
    }

    public override float CurrentStamina
    {
        get => base.CurrentStamina; protected set
        {
            base.CurrentStamina = value;
            playerStaminaChanged?.Invoke(this);
        }
    }

    public override void ResetStat()
    {
        MaxHp = 20;
        Damage = 1;
        WalkSpeed = 3.2f;
        RunSpeed = 5;
        StaminaRegen = 1;
        StaminaSuff = 5;
        MaxStamina = 20;
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

    public PlayerStats(GameObject gameObjectBind) : base(gameObjectBind)
    {
    }
}