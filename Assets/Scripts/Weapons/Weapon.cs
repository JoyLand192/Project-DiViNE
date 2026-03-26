using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/New Weapon", fileName = "New Weapon")]
public class Weapon : ScriptableObject
{
    public Sprite Sprite;
    public Sprite BulletSprite;
    public ParticleSystem BreakEffect;
    public ParticleSystem HitEffect;
    public float ShotCooldown;
    public float BulletSpeed;
    public int BaseDamage;
}
