using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CR : Entity
{
    protected CRMovement movement;
    public CRMovement Movement => movement;
    protected CRStatus status;
    public CRStatus Status => status;
    protected CRShooter shooter;
    public CRShooter Shooter => shooter;
    protected CREquipment equipment;
    public CREquipment Equipment => equipment;
    [SerializeField] protected SpriteRenderer graphic;
    public override SpriteRenderer Graphic => graphic;

    protected virtual void Awake()
    {
        movement = GetComponent<CRMovement>();
        status = GetComponent<CRStatus>();
        shooter = GetComponent<CRShooter>();
        equipment = GetComponent<CREquipment>();

        shooter.DamageCalcRequest = (damageBase) => damageBase * status.Damage;
    }
    protected virtual void FixedUpdate()
    {
        movement.Move(status.MoveSpeed);
    }
}
