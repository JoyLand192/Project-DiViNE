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
    public event System.Action<float> OnMoveSpeedChanged;
}
