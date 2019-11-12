using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitStats
{
    public delegate void HealthHandler(UnitStats stats);
    //public static event HealthHandler HPisZero;
    //public static event HealthHandler damageTaken;
    //public static event HealthHandler healingTaken;

    public float MaxHp { get; protected set; }
    protected float currentHP;
    public virtual float CurrentHp
    {
        get
        {
            return currentHP;
        }
        protected set
        {
            //var typeEvent = value < currentHP ? damageTaken : healingTaken;
            currentHP = value;
            //typeEvent?.Invoke(this);
            if (currentHP > MaxHp)
                currentHP = MaxHp;
        }
    }

    public float Damage { get; protected set; }
    public float WalkSpeed { get; protected set; }
    public float RunSpeed { get; protected set; }

    public virtual void TakeDamage(float damage)
    {
        CurrentHp -= damage;
        CheckIfDead();
    }

    public virtual void ResetStat()
    {
        CurrentHp = MaxHp;
    }

    public virtual bool CheckIfDead()
    {
        if (CurrentHp > 0)
        {
            return false;
        }
        //HPisZero?.Invoke(this);
        return true;
    }
}
