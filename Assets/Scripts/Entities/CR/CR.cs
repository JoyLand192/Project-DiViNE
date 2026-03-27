using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CR : Entity
{
    protected CRMovement movement;
    public override EntityMovement Movement => movement;
    protected CRStatus status;
    public override EntityStatus Status => status;
    protected CRShooter shooter;
    public CRShooter Shooter => shooter;
    [SerializeField] protected SpriteRenderer graphic;
    public override SpriteRenderer Graphic => graphic;

    protected virtual void Awake()
    {
        movement = GetComponent<CRMovement>();
        status = GetComponent<CRStatus>();
        shooter = GetComponent<CRShooter>();

        shooter.DamageCalcRequest = status.CalcFinalDamage;
    }
    protected virtual void FixedUpdate()
    {
        movement.Move(status.MoveSpeed);
    }
}
