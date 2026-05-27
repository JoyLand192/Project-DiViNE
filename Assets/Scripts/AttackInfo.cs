using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct AttackInfo
{
    public Vector3 Position;
    public Vector2 Direction;
    public bool IsFilpped;
    public float Damage;
    public IShooter Shooter;
    public Transform ShooterTransform;
    public SpriteRenderer Weapon;
    public BulletPool Pool;
    public DamageTextPool DTPool;
    public AttackInfo(Vector3 position, Vector2 direction, bool isFlipped, float damage, IShooter shooter, Transform shooterTransform, SpriteRenderer weapon,  BulletPool pool, DamageTextPool dtPool)
    {
        Position = position;
        Direction = direction;
        IsFilpped = isFlipped;
        Damage = damage;
        Shooter = shooter;
        ShooterTransform = shooterTransform;
        Weapon = weapon;
        Pool = pool;
        DTPool = dtPool;
    }
}