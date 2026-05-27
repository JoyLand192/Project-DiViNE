using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : EntityStatus
{
    public override float MoveSpeed 
    {
        get => moveSpeed;
        set
        {
            moveSpeed = value;
            OnMoveSpeedChanged?.Invoke(value);
        }
    }
    public float AggroRange { get; set; }
    public float AttackRange { get; set; }
    public event Action<float> OnMoveSpeedChanged;
    public void Initialize(EnemyInfo info)
    {
        MoveSpeed = info.MoveSpeed;
        MaxHP = info.MaxHP;
        Strength = info.Strength;
        AggroRange = info.AggroRange;
        AttackRange = info.AttackRange;

        HP = MaxHP;
    }
}
