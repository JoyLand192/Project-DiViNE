using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected SpriteRenderer graphic;
    public override SpriteRenderer Graphic => graphic;
    protected EnemyMovement movement;
    public override EntityMovement Movement => movement;
    protected EnemyStatus status;
    public override EntityStatus Status => status;
    protected virtual void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        status = GetComponent<EnemyStatus>();
    }
}
