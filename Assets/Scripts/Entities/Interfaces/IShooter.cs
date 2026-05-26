using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShooter
{
    public void OnBulletHit(
        Bullet bullet,
        Vector2 direction,
        Collider2D target,
        float damage,
        ParticleSystem hitEffect = null,
        ParticleSystem breakEffect = null);
    public void OnBulletBreak(Bullet bullet);
}
