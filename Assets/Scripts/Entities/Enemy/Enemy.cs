using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected ParticleSystem testDeathParticle;
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
        status.OnDeath += Death;
        status.Initialize();
    }
    protected virtual void Dispose()
    {
        status.OnMoveSpeedChanged -= movement.ChangeSpeed;
        status.OnDeath -= Death;
    }
    public virtual void Death()
    {
        Destroy(gameObject);
        var eff = Instantiate(testDeathParticle, transform.position, testDeathParticle.transform.rotation);
        Destroy(eff.gameObject, eff.main.duration);
    }
    public virtual void SetTarget(GameObject target) => movement.DebugTarget = target;
}
