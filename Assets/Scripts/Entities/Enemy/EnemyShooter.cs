using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour, IShooter
{
    [SerializeField] float damage;
    [SerializeField] DamageTextPool dtPool;
    float cd = 0.3f;
    float timer;
    void Update()
    {
        if (timer > 0) timer -= Time.deltaTime;
    }
    public void OnBulletHit(Bullet bullet, Vector2 direction, Collider2D target, 
        float damage, ParticleSystem hitEffect = null, ParticleSystem breakEffect = null)
    {
        throw new NotImplementedException();
    }
    public void OnBulletBreak(Bullet bullet)
    {
        throw new NotImplementedException();
    }
}
