using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RangedWeapon", menuName = "Project: DiViNE/Items/New RangedWeapon")]
public class RangedWeapon : Weapon
{
    public Bullet bulletPrefab;
    public ParticleSystem launchEffect;
    public float BulletSpeed;
    public float BulletLifetime;
    public Vector2 LaunchPoint;
    public override void Launch(AttackInfo info)
    {
        var bullet = info.Pool.GetBullet(bulletPrefab, info.Shooter, info.Damage);
        bullet.Launch(info.Position, info.Direction, BulletSpeed, BulletLifetime, HitEffect, BreakEffect);

        var launchEff = Instantiate(launchEffect, info.Position, launchEffect.transform.rotation);
        launchEff.transform.SetParent(info.Weapon.transform, true);
        Destroy(launchEff.gameObject, launchEff.main.duration);
    }
}
