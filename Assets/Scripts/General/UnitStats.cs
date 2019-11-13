using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitStats
{
    public GameObject GameObjectBind { get; protected set; }

    public delegate void StatsHandler(UnitStats stats);
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
            CheckIfDead();
        }
    }

    public float MaxStamina { get; protected set; }
    public virtual float CurrentStamina { get; protected set; }
    public float StaminaRegen { get; protected set; }
    public float StaminaSuff { get; protected set; }
    public bool isStaminaEmpty => CurrentStamina <= 0;


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
    }

    public virtual void Heal(float healAmount)
    {
        CurrentHp += healAmount;
    }

    public virtual void SpendStamina()
    {
        CurrentStamina -= StaminaSuff * Time.deltaTime;
    }

    public virtual void RegenerateStamina()
    {
        CurrentStamina += StaminaRegen * Time.deltaTime;
    }

    public virtual void ResetStat()
    {
        CurrentHp = MaxHp;
        CurrentStamina = MaxStamina;
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
