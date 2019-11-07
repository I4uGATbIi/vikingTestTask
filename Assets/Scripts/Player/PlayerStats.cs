using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : UnitStats
{
    public static event HealthHandler PlayerHPisZero;
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
}