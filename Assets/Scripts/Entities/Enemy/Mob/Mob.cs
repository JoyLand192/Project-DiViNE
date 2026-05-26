using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public enum MobBehaviourState { Idle, Chasing, Attacking }
public abstract class Mob : Enemy
{
    protected MobBehaviourState currentState;
    public MobBehaviourState CurrentState => currentState;
    protected CancellationTokenSource stateCTS;
    protected float stateLoopInterval = 0.1f;
    protected override void Awake()
    {
        base.Awake();
        MobStateLoop(stateLoopInterval).Forget();
    }
    public void CancelStateLoop()
    {
        if (stateCTS != null)
        {
            stateCTS.Cancel();
            stateCTS.Dispose();
            stateCTS = null;
        }
    }
    protected async UniTask MobStateLoop(float interval)
    {
        try
        {
            while (!stateCTS.IsCancellationRequested)
            {
                DecideBehaviour();
                await UniTask.Delay(System.TimeSpan.FromSeconds(interval), cancellationToken: stateCTS.Token);
            }
        }
        catch (OperationCanceledException)
        {

        }
        finally
        {
            CancelStateLoop();
        }
    }
    protected virtual void DecideBehaviour()
    {
        switch (currentState)
        {
            case MobBehaviourState.Idle:
                {
                    OnIdle();
                    break;
                }
            case MobBehaviourState.Chasing:
                {
                    OnChasing();
                    break;
                }
            case MobBehaviourState.Attacking:
                {
                    OnAttacking();
                    break;
                }
        }
    }
    protected abstract void OnIdle();
    protected abstract void OnChasing();
    protected abstract void OnAttacking();
}
