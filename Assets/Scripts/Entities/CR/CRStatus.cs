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
        protected set
        {
            var difference = value - base.HP;

            base.HP = value;
            OnHPChanged?.Invoke(difference);
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
