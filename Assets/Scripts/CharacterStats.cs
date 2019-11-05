using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitStats
{
    public delegate void HealthHandler(UnitStats stats);
    public event HealthHandler OnHPisZero;
    public event HealthHandler OnDamageTaken;

    public float MaxHp { get; private set; }
    public float CurrentHp { get; private set; }
    public float Damage { get; private set; }

    public float Speed { get; private set; }
    public float Jump { get; private set; }
    public float Gravity { get; private set; }

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
