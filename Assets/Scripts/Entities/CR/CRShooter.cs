using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRShooter : MonoBehaviour
{
    [SerializeField] protected BulletPool bulletPool;
    [SerializeField] protected SpriteRenderer weaponGraphic;
    [SerializeField] protected Bullet testBulletPrefab_T;
    [SerializeField] protected ParticleSystem testBulletEffect;
    [SerializeField] protected DamageTextPool damageTextPool;
    [SerializeField] protected float weaponMinDistance = 0.5f;
    [SerializeField] protected float weaponMaxDistance = 6;
    [SerializeField] protected float weaponDistanceScale = 0.18f;
    [SerializeField] protected float shootCooldown = 0.25f;
    [SerializeField] protected float timer = 0;
    protected bool isShootable;
    public bool IsShootable
    {
        get => isShootable;
        set => isShootable = value;
    }
    protected Camera cam;
    public event System.Action<Enemy> OnEnemyHit;
    void Awake()
    {
        cam = GetComponent<Camera>();
    }
    protected void Update()
    {
        if (timer > 0) timer -= Time.deltaTime;

        if (Input.GetMouseButton(0) && timer <= 0)
        {
            Shoot();
            timer += shootCooldown;
        }
        if (weaponGraphic != null) WeaponPos();
    }
    protected void WeaponPos()
    {
        Vector2 mouseVector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        var distance = Mathf.Min(mouseVector.magnitude, weaponMaxDistance) * weaponDistanceScale + weaponMinDistance;
        var fixedWeaponPos = mouseVector.normalized * distance;
        var angle = Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;

        weaponGraphic.flipX = angle > 90 || angle < -90;
        angle -= angle > 90 || angle < -90 ? 180 : 0;
        weaponGraphic.transform.rotation = Quaternion.Euler(0, 0, angle);

        weaponGraphic.transform.position = transform.position + (Vector3)fixedWeaponPos;
    }
    protected void Shoot()
    {
        if (bulletPool == null) return;

        var bullet = bulletPool.GetBullet(testBulletPrefab_T);
        var direction = (Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        var normalizedDirection = direction.normalized;
        var startPos = weaponGraphic == null ? transform.position : weaponGraphic.transform.position;
        var damage = Random.Range(1, 100);

        bullet.OnBulletHit += (target) =>
        {
            if (target is Enemy)
            {
                var enemy = target as Enemy;

                Debug.Log($"데미지 발생! {Time.frameCount}");

                if (damageTextPool != null)
                {
                    var dt = damageTextPool.Get();

                    dt.SetText(damage);
                    dt.gameObject.SetActive(true);
                    dt.Jump(enemy.transform.position, damageTextPool).Forget();
                }

                enemy.Status.TakeDamage(damage);
                OnEnemyHit?.Invoke(enemy);
            }
        };
        bullet.OnBulletDeath += () =>
        {
            Destroy(Instantiate(testBulletEffect, bullet.transform.position, Quaternion.identity).gameObject, 1f);
            bulletPool.Return(bullet);
        };
        bullet.Launch(startPos, normalizedDirection, 65, 1f);
    }
}
