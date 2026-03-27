using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/New Ranged Weapon", fileName = "New Ranged Weapon")]
public class RangedWeapon : Weapon
{
    public Bullet bulletPrefab;
    public float BulletSpeed;
    public float BulletLifetime;
    public override void Launch(AttackInfo info)
    {
        var bullet = info.Pool.GetBullet(bulletPrefab, info.Shooter, info.Damage);
        bullet.Launch(info.Position, info.Direction, BulletSpeed, BulletLifetime);
    }
}
