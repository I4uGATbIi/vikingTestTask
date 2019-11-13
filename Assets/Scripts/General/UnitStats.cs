using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitStats
{
    public GameObject GameObjectBind { get; protected set; }
    
    public delegate void HealthHandler(UnitStats stats);
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
            currentHP = value;
            if (currentHP > MaxHp)
                currentHP = MaxHp;
        }
    }

    public float Damage { get; protected set; }
    public float WalkSpeed { get; protected set; }
    public float RunSpeed { get; protected set; }

    public UnitStats(GameObject gameObjectBind)
    {
        GameObjectBind = gameObjectBind;
        ResetStat();
    }
    
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
        return true;
    }
}
