using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CR : Entity
{
    protected CRMovement movement;
    public override EntityMovement Movement => movement;
    protected CRStatus status;
    public override EntityStatus Status => status;
    [SerializeField] protected SpriteRenderer graphic;
    public override SpriteRenderer Graphic => graphic;

    void Awake()
    {
        movement = GetComponent<CRMovement>();
        status = GetComponent<CRStatus>();
    }
}
