using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularMob : Mob
{
    protected static readonly string[] attackBlockingLayers = { "HardWall" };
    protected static int? blockingLayerMask;
    protected override void Awake()
    {
        base.Awake();
        blockingLayerMask ??= LayerMask.GetMask(attackBlockingLayers);
    }
    protected override void DecideBehaviour()
    {
        var currentTargetPosition = CurrentStageManager.CurrentCR.transform.position;
        var currentTargetDistance = Vector3.Magnitude(currentTargetPosition - transform.position);

        if (currentTargetDistance < status.AttackRange)
        {
            var blockedByWall = Physics2D.Linecast(
            shooter.LaunchPoint,
            currentTargetPosition,
            blockingLayerMask.Value
            ).collider != null;

            currentState = blockedByWall ? MobBehaviourState.Chasing : MobBehaviourState.Attacking;
        }
        else if (currentTargetDistance < status.AggroRange) currentState = MobBehaviourState.Chasing;
        else currentState = MobBehaviourState.Idle;

        shooter.CurrentTarget = currentTargetDistance < status.AggroRange ? CurrentStageManager.CurrentCR.transform : null;
        movement.IsChasing = currentState == MobBehaviourState.Chasing;
        shooter.IsAttacking = currentState == MobBehaviourState.Attacking;

        base.DecideBehaviour();
    }
    protected override void OnAttacking()
    {
        
    }

    protected override void OnChasing()
    {
    }

    protected override void OnIdle()
    {
    }
}
