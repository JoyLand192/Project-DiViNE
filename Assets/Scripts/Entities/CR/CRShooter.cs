using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class WeaponSlot
{
    public Weapon Weapon;
    public int AmmoLeft;
    public int MagazineLeft;
    public WeaponSlot(Weapon weapon)
    {
        Weapon = weapon;
        AmmoLeft = weapon.AmmoCount;
        MagazineLeft = weapon.MagazineCount;
    }
}
public class CRShooter : MonoBehaviour, IShooter
{
    const int weaponSlotsCount = 3;
    protected WeaponSlot[] weapons = new WeaponSlot[weaponSlotsCount];
    [SerializeField] protected Weapon[] DEBUGWeapons;
    [SerializeField] protected WeaponUIDisplayer weaponUIDisplay;
    [SerializeField] protected BulletPool bulletPool;
    [SerializeField] protected SpriteRenderer weaponGraphic;
    [SerializeField] protected DamageTextPool damageTextPool;
    [SerializeField] protected Weapon currentWeapon;
    [SerializeField] protected int CurrentWeaponIndex = 1;
    [SerializeField] protected float weaponMinDistance = 0.5f;
    [SerializeField] protected float weaponMaxDistance = 6;
    [SerializeField] protected float weaponDistanceScale = 0.18f;
    [SerializeField] protected float timer = 0;
    public Weapon CurrentWeapon
    {
        get => currentWeapon;
        protected set
        {
            currentWeapon = value;
            if (value == null)
            {
                weaponGraphic.gameObject.SetActive(false);
                return;
            }
            weaponGraphic.gameObject.SetActive(true);
            weaponGraphic.sprite = currentWeapon.Sprite;
        }
    }
    public WeaponSlot CurrentWeaponSlot => weapons[CurrentWeaponIndex];
    public bool IsShootable { get; set; }
    public bool IsFlipped { get; set; }
    public bool IsSlashing { get; set; }
    public bool IsReloading { get; set; }
    protected Camera cam;
    protected Vector3 weaponShakeOffset;
    protected Tween weaponShakeTween;
    public System.Func<int, float> DamageCalcRequest;
    public event System.Action<Enemy> OnEnemyHit;
    public event System.Action<WeaponSlot> OnWeaponChanged;
    public event System.Action<WeaponSlot> OnShoot;
    public event System.Action<WeaponSlot, System.Action> OnReload;
    void Awake()
    {
        cam = GetComponent<Camera>();
        for (int i = 0; i < weapons.Length; i++) weapons[i] = new WeaponSlot(DEBUGWeapons[i]);

        if (weaponUIDisplay != null) weaponUIDisplay.Initialize(this);
    }
    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeWeapon(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeWeapon(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeWeapon(2);
        else if (Input.GetKeyDown(KeyCode.Space)) NextWeapon();
        else if (Input.GetKeyDown(KeyCode.R) && CurrentWeapon is RangedWeapon)
        {
            if (IsReloading || CurrentWeaponSlot.AmmoLeft >= CurrentWeapon.AmmoCount || CurrentWeaponSlot.MagazineLeft <= 0) return;
            Reload(CurrentWeaponSlot);
        }

        Cooldown();
        if (weaponGraphic != null) WeaponPos();
    }
    protected void OnDestroy()
    {
        OnEnemyHit = null;
        OnShoot = null;
        OnReload = null;
        OnWeaponChanged = null; 
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

        var distance = Mathf.Min(mouseVector.magnitude * weaponDistanceScale + weaponMinDistance, weaponMaxDistance);
        var fixedWeaponPos = mouseVector.normalized * distance;
        var angle = Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;

        var sign = angle > 90 || angle < -90 ? -1 : 1;
        IsFlipped = sign < 0;

        var scaling = weaponGraphic.transform.localScale;
        scaling.x = Mathf.Abs(scaling.x) * sign;
        weaponGraphic.transform.localScale = scaling;

        angle -= angle > 90 || angle < -90 ? 180 : 0;
        weaponGraphic.transform.rotation = Quaternion.Euler(0, 0, angle);

        weaponGraphic.transform.position = transform.position + (Vector3)fixedWeaponPos + weaponShakeOffset;
    }
    public void Reload(WeaponSlot weaponSlot)
    {
        if (weaponSlot.Weapon.ReloadEffect != null)
        {
            var eff = Instantiate(weaponSlot.Weapon.ReloadEffect, weaponGraphic.transform.position, weaponSlot.Weapon.ReloadEffect.transform.rotation);
            Destroy(eff.gameObject, eff.main.duration);
        }

        weaponShakeTween.Kill(true);
        weaponShakeTween = DOTween.Shake(
            () => weaponShakeOffset,
            (x) => weaponShakeOffset = x,
            0.45f,
            strength: 0.25f,
            vibrato: 12);

        IsReloading = true;
        OnReload?.Invoke(weaponSlot, () =>
        {
            IsReloading = false;
            weaponSlot.AmmoLeft = weaponSlot.Weapon.AmmoCount;
            weaponSlot.MagazineLeft--;
        });
    }
    public void NextWeapon() => ChangeWeapon(++CurrentWeaponIndex % 3);
    public void ChangeWeapon(int index)
    {
        if (IsReloading)
        {
            weaponUIDisplay.CancelReload(CurrentWeaponSlot);
            IsReloading = false;
        }

        CurrentWeaponIndex = index;

        CurrentWeapon = weapons[index].Weapon;
        weaponUIDisplay.SetCurrentSlot(index);

        weaponShakeTween.Kill(true);
        weaponShakeTween = DOTween.Shake(
            () => weaponShakeOffset,
            (x) => weaponShakeOffset = x,
            0.15f,
            strength: 0.175f,
            vibrato: 35);

        OnWeaponChanged?.Invoke(CurrentWeaponSlot);
    }
    public void DelayedAction(float delay, System.Action action) => StartCoroutine(DelayedActionCoroutine(delay, action));
    public IEnumerator DelayedActionCoroutine(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
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
    public void OnBulletHit(Bullet bullet, Vector2 direction, Collider2D target, float damage, ParticleSystem hitEffect = null, ParticleSystem breakEffect = null)
    {
        if (target.TryGetComponent<Enemy>(out var enemy))
        {
            Debug.Log($"데미지 발생! {Time.frameCount}");

            if (hitEffect != null)
            {
                var eff = Instantiate(hitEffect, enemy.transform.position, Quaternion.Euler(-90, 0, 0));
                foreach (var dp in eff.GetComponentsInChildren<DirectionalParticle>()) dp.SetShapeAngle(new Vector3(0, -1 * (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90), 0));
                foreach (var part in eff.GetComponentsInChildren<ParticleSystem>()) part.Play();

                Destroy(eff.gameObject, hitEffect.main.duration);
            }

            enemy.Status.TakeDamage(damage, damageTextPool);
            OnEnemyHit?.Invoke(enemy);
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
    public async void OnBulletBreak(Bullet bullet)
    {
        await UniTask.Delay(30);
        bulletPool.Return(bullet);
    }
    public void Shoot()
    {
        if (bulletPool == null) return;
        if (IsReloading) return;

        var ranged = CurrentWeapon as RangedWeapon;
        if (ranged != null && CurrentWeaponSlot.AmmoLeft < CurrentWeapon.AmmoCost)
        {
            if (weapons[CurrentWeaponIndex].MagazineLeft <= 0)
            {
                //TODO : not enough fucking magazines
                return;
            }
            Reload(CurrentWeaponSlot);

            return;
        }

        var direction = (Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        var normalizedDirection = direction.normalized;
        Vector3 startPos;
        if (weaponGraphic == null) startPos = transform.position;
        else
        {
            if (ranged != null) startPos = weaponGraphic.transform.position + (weaponGraphic.transform.rotation * Vector3.Scale((Vector3)ranged.LaunchPoint, weaponGraphic.transform.lossyScale));
            else startPos = weaponGraphic.transform.position;
        }
        float damage = DamageCalcRequest.Invoke(CurrentWeapon.BaseDamage);

        CurrentWeapon.Launch(new AttackInfo(startPos, normalizedDirection, IsFlipped, damage, this, transform, weaponGraphic, bulletPool, damageTextPool));

        CurrentWeaponSlot.AmmoLeft -= CurrentWeapon.AmmoCost;
        OnShoot?.Invoke(CurrentWeaponSlot);
    }
}
