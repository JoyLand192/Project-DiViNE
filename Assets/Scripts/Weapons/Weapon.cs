using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    public Sprite Sprite;
    public ParticleSystem BreakEffect;
    public ParticleSystem HitEffect;
    public float AttackCooldown;
    public int BaseDamage;
    public abstract void Launch(AttackInfo info);
}
