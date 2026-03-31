using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected SpriteRenderer graphic;
    public override SpriteRenderer Graphic => graphic;
    protected EnemyMovement movement;
    public EnemyMovement Movement => movement;
    protected EnemyStatus status;
    public EnemyStatus Status => status;
    protected virtual void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        status = GetComponent<EnemyStatus>();

        status.OnMoveSpeedChanged += movement.ChangeSpeed;

        Init();
    }
    protected virtual void Init()
    {
        status.HP = status.MaxHP;
    }
    public virtual void SetTarget(GameObject target) => movement.DebugTarget = target;
}
