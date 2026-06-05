using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyShooter : MonoBehaviour, IShooter
{
    [SerializeField] DamageTextPool damageTextPool;
    [SerializeField] BulletPool bulletPool;
    [SerializeField] SpriteRenderer weaponGraphic;
    [SerializeField] Weapon currentWeapon;
    [SerializeField] protected float weaponMinDistance = 0.5f;
    [SerializeField] protected float weaponMaxDistance = 6;
    [SerializeField] protected float weaponDistanceScale = 0.18f;
    protected float attackTimer = 0f;
    protected float? attackCooldown;
    public float? AttackCooldown
    {
        get => attackCooldown;
        set => attackCooldown = value;
    }
    protected Transform currentTarget;
    public Transform CurrentTarget
    {
        get => currentTarget;
        set => currentTarget = value;
    }
    public DamageTextPool DamageTextPool
    {
        get => damageTextPool;
        set => damageTextPool = value;
    }
    public BulletPool BulletPool
    {
        get => bulletPool;
        set => bulletPool = value;
    }
    public Weapon CurrentWeapon
    {
        get => currentWeapon;
        set
        {
            currentWeapon = value;
            AttackCooldown = currentWeapon != null ? currentWeapon.AttackCooldown : null;
        }
    }
    public Vector3 LaunchPoint
    {
        get
        {
            Vector3 startPos;
            if (weaponGraphic == null) startPos = transform.position;
            else
            {
                if (CurrentWeapon is RangedWeapon ranged) startPos = weaponGraphic.transform.position + (weaponGraphic.transform.rotation * Vector3.Scale((Vector3)ranged.LaunchPoint, weaponGraphic.transform.lossyScale));
                else startPos = weaponGraphic.transform.position;
            }
            return startPos;
        }
    }
    public bool IsAttacking { get; set; }
    public bool IsFlipped { get; set; }
    public event Action<CR> OnCRHit;
    public Func<int, float> DamageCalcRequest;
    protected virtual void Update()
    {
        if (weaponGraphic != null) WeaponPos();
        WeaponCooldown();
    }
    protected virtual void WeaponCooldown()
    {
        if (AttackCooldown == null) return;

        if (attackTimer < 0f)
        {
            if (IsAttacking)
            {
                attackTimer = AttackCooldown.Value;
                Shoot();
            }
        }
        else attackTimer -= Time.deltaTime;
    }
    public virtual void Shoot()
    {
        if (CurrentTarget == null) return;
        if (currentWeapon == null) return;

        var normalizedDirection = (CurrentTarget.position - LaunchPoint).normalized;
        var damage = DamageCalcRequest?.Invoke(CurrentWeapon.BaseDamage) ?? CurrentWeapon.BaseDamage;

        currentWeapon.Launch(new AttackInfo(LaunchPoint, normalizedDirection, IsFlipped, damage, this, transform, weaponGraphic, bulletPool, DamageTextPool));
    }
    public void OnBulletHit(Bullet bullet, Vector2 direction, Collider2D target, float damage, ParticleSystem hitEffect = null, ParticleSystem breakEffect = null)
    {
        if (target.TryGetComponent<CR>(out var cr))
        {
            if (hitEffect != null)
            {
                var eff = Instantiate(hitEffect, cr.transform.position, Quaternion.Euler(-90, 0, 0));
                foreach (var dp in eff.GetComponentsInChildren<DirectionalParticle>()) dp.SetShapeAngle(new Vector3(0, -1 * (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90), 0));
                foreach (var part in eff.GetComponentsInChildren<ParticleSystem>()) part.Play();

                Destroy(eff.gameObject, hitEffect.main.duration);
            }

            cr.Status.TakeDamage(damage, DamageTextPool);
            OnCRHit?.Invoke(cr);
        }
        else
        {
            if (breakEffect == null) return;

            var eff = Instantiate(breakEffect, bullet.transform.position, Quaternion.Euler(-90, 0, 0));
            foreach (var dp in eff.GetComponentsInChildren<DirectionalParticle>()) dp.SetShapeAngle(new Vector3(0, -1 * (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90), 0));
            foreach (var part in eff.GetComponentsInChildren<ParticleSystem>()) part.Play();

            Destroy(eff.gameObject, breakEffect.main.duration);
        }
    }
    protected void WeaponPos()
    {
        if (CurrentTarget == null) return;

        Vector2 vectorToTarget = currentTarget.position - transform.position;

        var distance = Mathf.Min(vectorToTarget.magnitude * weaponDistanceScale + weaponMinDistance, weaponMaxDistance); 
        var fixedWeaponPos = vectorToTarget.normalized * distance;
        var angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;

        var sign = angle > 90 || angle < -90 ? -1 : 1;
        IsFlipped = sign < 0;

        var scaling = weaponGraphic.transform.localScale;
        scaling.x = Mathf.Abs(scaling.x) * sign;
        weaponGraphic.transform.localScale = scaling;

        angle -= angle > 90 || angle < -90 ? 180 : 0;
        weaponGraphic.transform.rotation = Quaternion.Euler(0, 0, angle);

        weaponGraphic.transform.position = transform.position + (Vector3)fixedWeaponPos;
    }
    public async void OnBulletBreak(Bullet bullet)
    {
        await UniTask.Delay(30);
        bulletPool.Return(bullet);
    }
    public void OnMeleeHit(Collider2D[] hitEntities, AttackInfo info, ParticleSystem part)
    {
        foreach (var hitEntity in hitEntities)
        {
            if (hitEntity.TryGetComponent<CR>(out var cr))
            {
                var eff = Instantiate(part, cr.transform.position, Quaternion.Euler(-90, 0, 0));
                Destroy(eff.gameObject, eff.main.duration);

                cr.Status.TakeDamage(info.Damage, DamageTextPool);
            }
        }
    }
}
