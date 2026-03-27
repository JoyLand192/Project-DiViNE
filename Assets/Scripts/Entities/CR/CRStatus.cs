using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRStatus : EntityStatus
{
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
            //...
            strength = value;
        }
    }
    public float CalcFinalDamage(int damageBase)
    {
        return damageBase * strength;
    }
}
