using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public struct AttackInfo
{
    public Vector3 Position;
    public Vector2 Direction;
    public bool IsFilpped;
    public float Damage;
    public CRShooter Shooter;
    public BulletPool Pool;
    public DamageTextPool DTPool;
    public AttackInfo(Vector3 position, Vector2 direction, bool isFlipped, float damage, CRShooter shooter, BulletPool pool, DamageTextPool dtPool)
    {
        Position = position;
        Direction = direction;
        IsFilpped = isFlipped;
        Damage = damage;
        Shooter = shooter;
        Pool = pool;
        DTPool = dtPool;
    }
}
public class CRShooter : MonoBehaviour
{
    [SerializeField] protected List<KeyCode> changingKeys = new();
    [SerializeField] protected WeaponDisplayer display;
    [SerializeField] protected BulletPool bulletPool;
    [SerializeField] protected SpriteRenderer weaponGraphic;
    [SerializeField] protected DamageTextPool damageTextPool;
    [SerializeField] protected Weapon currentWeapon;
    [SerializeField] protected float weaponMinDistance = 0.5f;
    [SerializeField] protected float weaponMaxDistance = 6;
    [SerializeField] protected float weaponDistanceScale = 0.18f;
    [SerializeField] protected float timer = 0;
    protected Queue<Weapon> weaponQueue = new();
    public Weapon CurrentWeapon
    {
        get => currentWeapon;
        set
        {
            currentWeapon = value;
            if (value == null) return;

            weaponGraphic.sprite = currentWeapon.Sprite;
        }
    }
    public bool IsShootable { get; set; }
    public bool IsFlipped { get; set; }
    public bool IsSlashing { get; set; }
    protected Camera cam;
    public System.Func<int, float> DamageCalcRequest;
    public event System.Action<Enemy> OnEnemyHit;
    void Awake()
    {
        cam = GetComponent<Camera>();
    }
    protected void Update()
    {
        foreach (var key in changingKeys)
        {
            if (Input.GetKeyDown(key))
            {
                NextWeapon(CurrentWeapon, weaponQueue.Dequeue());
                break;
            }
        }
        Cooldown();
        if (weaponGraphic != null) WeaponPos();
    }
    protected virtual void Cooldown()
    {
        if (timer > 0) timer -= Time.deltaTime;
        if (CurrentWeapon != null && Input.GetMouseButton(0) && timer <= 0)
        {
            Shoot();
            timer += CurrentWeapon.AttackCooldown;
        }
    }
    protected void WeaponPos()
    {
        if (IsSlashing) return;

        Vector2 mouseVector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        var distance = Mathf.Min(mouseVector.magnitude, weaponMaxDistance) * weaponDistanceScale + weaponMinDistance;
        var fixedWeaponPos = mouseVector.normalized * distance;
        var angle = Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;

        var sign = angle > 90 || angle < -90 ? -1 : 1;
        IsFlipped = sign < 0;

        var scaling = weaponGraphic.transform.localScale;
        scaling.x = Mathf.Abs(scaling.x) * sign;
        weaponGraphic.transform.localScale = scaling;

        angle -= angle > 90 || angle < -90 ? 180 : 0;
        weaponGraphic.transform.rotation = Quaternion.Euler(0, 0, angle);

        weaponGraphic.transform.position = transform.position + (Vector3)fixedWeaponPos;
    }
    public void DelayedAction(float delay, System.Action action) => StartCoroutine(DelayedActionCoroutine(delay, action));
    public IEnumerator DelayedActionCoroutine(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
    public void NextWeapon(Weapon before, Weapon after)
    {
        weaponQueue.Enqueue(before);
        CurrentWeapon = after;
    }
    public void NewWeapon(Weapon after)
    {

    }
    public void OnMeleeHit(Collider2D[] hitEntities, AttackInfo info, ParticleSystem part)
    {
        foreach (var hitEntity in hitEntities)
        {
            if (hitEntity.TryGetComponent<Enemy>(out var target))
            {
                var eff = Instantiate(part, target.transform.position, Quaternion.Euler(-90, 0, 0));
                Destroy(eff.gameObject, eff.main.duration);

                target.Status.TakeDamage(info.Damage, damageTextPool);
            }
        }
    }
    public void OnBulletHit(Bullet bullet, Vector2 direction, Collider2D target, float damage)
    {
        if (target.TryGetComponent<Enemy>(out var enemy))
        {
            Debug.Log($"데미지 발생! {Time.frameCount}");

            var eff = Instantiate(CurrentWeapon.HitEffect, enemy.transform.position, Quaternion.Euler(-90, 0, 0));
            foreach (var dp in eff.GetComponentsInChildren<DirectionalParticle>()) dp.SetShapeAngle(new Vector3(0, -1 * (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90), 0));
            foreach (var part in eff.GetComponentsInChildren<ParticleSystem>()) part.Play();

            Destroy(eff.gameObject, CurrentWeapon.HitEffect.main.duration);

            enemy.Status.TakeDamage(damage, damageTextPool);
            OnEnemyHit?.Invoke(enemy);
        }
        else
        {
            var eff = Instantiate(CurrentWeapon.BreakEffect, bullet.transform.position, Quaternion.Euler(-90, 0, 0));
            foreach (var dp in eff.GetComponentsInChildren<DirectionalParticle>()) dp.SetShapeAngle(new Vector3(0, -1 * (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90), 0));
            foreach (var part in eff.GetComponentsInChildren<ParticleSystem>()) part.Play();

            Destroy(eff.gameObject, CurrentWeapon.BreakEffect.main.duration);
        }
    }
    public async void OnBulletBreak(Bullet bullet)
    {
        await UniTask.Delay(30);
        bulletPool.Return(bullet);
    }
    protected void Shoot()
    {
        if (bulletPool == null) return;

        var direction = (Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        var normalizedDirection = direction.normalized;
        var startPos = weaponGraphic == null ? transform.position : weaponGraphic.transform.position;
        float damage = DamageCalcRequest.Invoke(CurrentWeapon.BaseDamage);

        CurrentWeapon.Launch(new AttackInfo(startPos, normalizedDirection, IsFlipped, damage, this, bulletPool, damageTextPool));
    }
}
