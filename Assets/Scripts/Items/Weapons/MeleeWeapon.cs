using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MeleeWeapon", menuName = "Project: DiViNE/Items/New MeleeWeapon")]
public class MeleeWeapon : Weapon
{
    public ParticleSystem SlashEffect;
    public Vector3 rangePoint1;
    public Vector3 rangePoint2;
    public override void Launch(AttackInfo info)
    {
        var slash = Instantiate(SlashEffect, info.ShooterTransform);
        if (info.IsFilpped) slash.transform.localScale = Vector3.Scale(slash.transform.localScale, new Vector3(-1, 1, 1));
        Destroy(slash.gameObject, slash.main.duration);

        var crPos = info.ShooterTransform.position; 
        var hitEntities = Physics2D.OverlapAreaAll(crPos + rangePoint1, crPos + rangePoint2);

        info.Shooter.OnMeleeHit(hitEntities, info, HitEffect);
    }
}
