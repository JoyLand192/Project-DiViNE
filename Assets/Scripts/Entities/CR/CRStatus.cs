using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRStatus : EntityStatus
{
    public override float MaxHP
    {
        get => base.MaxHP;
        set
        {
            base.MaxHP = value;
        }
    }
    public override float HP
    {
        get => base.HP;
        set
        {
            var difference = value - base.HP;

            base.HP = value;
            OnHPChanged?.Invoke(difference);
        }
    }
    [SerializeField] protected float strength;
    public float Strength
    {
        get
        {
            //...
            return strength;
        }
        set
        {
            strength = value;
        }
    }
    public float Damage
    {
        get
        {
            return strength;
        }
    }
    public event System.Action<float> OnHPChanged;
}
