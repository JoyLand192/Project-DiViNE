using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShooter
{
    public void Shoot();
    public void OnBulletHit(
        Bullet bullet,
        Vector2 direction,
        Collider2D target,
        float damage,
        ParticleSystem hitEffect = null,
        ParticleSystem breakEffect = null);
    public void OnBulletBreak(Bullet bullet);
    public void OnMeleeHit(Collider2D[] hitEntities, AttackInfo info, ParticleSystem part);
}
