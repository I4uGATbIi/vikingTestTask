using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitStats
{
    public delegate void HealthHandler(UnitStats stats);
    public static event HealthHandler OnHPisZero;
    public static event HealthHandler OnDamageTaken;

    public float MaxHp { get; protected set; }
    public float CurrentHp { get; protected set; }
    
    public float Damage { get; protected set; }
    public float WalkSpeed { get; protected set; }
    public float RunSpeed { get; protected set; }

    public virtual void TakeDamage(float damage)
    {
        CurrentHp -= damage;
        IsHPZero();
        OnDamageTaken(this);
    }

    public virtual void ResetStat()
    {
        CurrentHp = MaxHp;
    }

    public virtual void IsHPZero()
    {
        if(CurrentHp <= 0)
        {
            OnHPisZero(this);
        }
    }
}
