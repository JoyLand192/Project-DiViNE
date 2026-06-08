using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, ILaunchable
{
    readonly string[] blockingLayers = { "HardWall" };
    const float gainDistance = 0.5f;
    const float accelerationSpeed = 50f;
    const float defaultSpeed = 1f;
    const float maxSpeed = 70f;
    [SerializeField] int coinAmount;
    public int CoinAmount
    {
        get => coinAmount;
        set => coinAmount = value;
    }
    CircleCollider2D crDetector;
    CR currentTarget;
    Tween currentLaunchTween;
    float currentSpeed = defaultSpeed;
    public Tween CurrentLaunchTween => currentLaunchTween;

    void Awake()
    {
        crDetector = GetComponent<CircleCollider2D>();
    }
    void Update()
    {
        if (currentTarget == null) return;
        if (Vector2.Distance(transform.position, currentTarget.transform.position) < gainDistance)
        {
            DataManager.Instance.CurrentCoin += coinAmount;

            var prefab = LootCoins.GetCoinEffect(coinAmount);
            var eff = Instantiate(prefab, transform.position, prefab.transform.rotation);

            Destroy(eff.gameObject, eff.main.duration);
            Destroy(gameObject);

            return;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, currentTarget.transform.position, currentSpeed * Time.deltaTime);
            currentSpeed = Mathf.Min(maxSpeed, currentSpeed + accelerationSpeed * Time.deltaTime);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        currentTarget = collision.GetComponent<CR>();
        crDetector.enabled = false;

        if (currentLaunchTween != null)
        {
            currentLaunchTween.Kill(false);
            currentLaunchTween = null;
        }
    }
    public void Launch(float power = 7f, float duration = 0.6f)
    {
        var direction = (Vector3)Random.insideUnitCircle.normalized;
        var fixedPower = power * Random.Range(0.85f, 1.15f);
        var destination = transform.position + direction * fixedPower;

        var blockRay = Physics2D.Raycast(transform.position, direction, power, LayerMask.GetMask(blockingLayers));
        if (blockRay) destination = (Vector3)blockRay.point - direction * power / 7;

        currentLaunchTween = transform.DOMove(destination, duration).SetEase(Ease.OutCirc);
    }
}
