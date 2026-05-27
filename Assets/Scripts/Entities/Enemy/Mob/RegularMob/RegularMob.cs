using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularMob : Mob
{
    protected override void OnAttacking()
    {
        movement.IsChasing = false;
    }

    protected override void OnChasing()
    {
        movement.IsChasing = true;
    }

    protected override void OnIdle()
    {
        movement.IsChasing = false;
    }
}
