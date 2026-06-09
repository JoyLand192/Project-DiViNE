using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    protected static StageManager currentStageManager;
    public static StageManager CurrentStageManager
    {
        get
        {
            if (currentStageManager == null) currentStageManager = FindAnyObjectByType<StageManager>();
            return currentStageManager;
        }
        set => currentStageManager = value;
    }
    [SerializeField] protected EnemyInfo info;
    public EnemyInfo Info => info;
    [SerializeField] protected ParticleSystem testDeathParticle;
    [SerializeField] protected SpriteRenderer graphic;
    public override SpriteRenderer Graphic => graphic;
    protected EnemyMovement movement;
    public EnemyMovement Movement => movement;
    protected EnemyShooter shooter;
    public EnemyShooter Shooter => shooter;
    protected EnemyStatus status;
    public EnemyStatus Status => status;
    protected virtual void Awake()
    {
        if (Info == null)
        {
            Debug.LogError("Enemy Info is not assigned!", this);
            Destroy(gameObject);

            return;
        }
        movement = GetComponent<EnemyMovement>();
        status = GetComponent<EnemyStatus>();
        shooter = GetComponent<EnemyShooter>();

        status.OnMoveSpeedChanged += movement.ChangeSpeed;
        status.OnDeath += Death;
        shooter.CurrentWeapon = Info.EnemyWeapon;
        shooter.BulletPool = CurrentStageManager.BulletPool;
        shooter.DamageTextPool = CurrentStageManager.DamageTextPool;
        shooter.DamageCalcRequest = (damageBase) => damageBase * status.Strength;
    }
    protected virtual void Start()
    {
        status.Initialize(Info);
    }
    protected virtual void Update()
    {
        movement.Move(CurrentStageManager == null ? null : CurrentStageManager.CurrentCR);
    }
    protected virtual void OnDestroy()
    {
        if (CurrentStageManager != null) CurrentStageManager.EnemyDeath(Info);

        if (status != null)
        {
            status.OnMoveSpeedChanged -= movement.ChangeSpeed;
            status.OnDeath -= Death;
        }
    }
    public virtual void Death()
    {
        var eff = Instantiate(testDeathParticle, transform.position, testDeathParticle.transform.rotation);
        Destroy(eff.gameObject, eff.main.duration);
        Destroy(gameObject);

        var coinValue = Mathf.RoundToInt(Random.Range(Info.LootInfo.MinCoinAmount, Info.LootInfo.MaxCoinAmount));
        var coin = Instantiate(LootDrops.GetCoinObject(coinValue), transform.position, Quaternion.identity);
        coin.CoinAmount = coinValue;
        coin.Launch();

        var drop = Info.LootInfo.Roll();
        if (drop == null) return;
        if (drop.Drop is Weapon dropWeapon)
        {
            var dropObj = Instantiate(LootDrops.WeaponDropPrefab, transform.position, Quaternion.identity);
            dropObj.Initialize(dropWeapon);
            dropObj.Launch();
        }

        //TODO: drop loot
    }
}
