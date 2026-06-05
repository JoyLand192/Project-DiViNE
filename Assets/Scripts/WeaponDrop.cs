using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrop : MonoBehaviour, IInteraction, ILaunchable
{
    [SerializeField] Weapon DEBUGWEAPON;
    readonly string[] blockingLayers = { "HardWall" }; // TODO : Add BreakableWall layer and include it here
    WeaponSlot weaponSlotInfo;
    SpriteRenderer render;
    public Vector3 CurrentPosition => transform.position;
    bool isAvailable = true;
    public bool IsAvailable
    {
        get => isAvailable;
        set => isAvailable = value;
    }
    bool isDisplayingUI;
    public bool IsDisplayingUI
    {
        get => isDisplayingUI;
        set
        {
            isDisplayingUI = value;
        }
    }
    Tween currentLaunchTween;
    public Tween CurrentLaunchTween => currentLaunchTween;
    void Awake()
    {
        render = gameObject.GetComponent<SpriteRenderer>();

        Initialize(DEBUGWEAPON);
        Launch();
    }
    public void Initialize(Weapon weapon)
    {
        weaponSlotInfo = new WeaponSlot(weapon);
        render.sprite = weapon.Sprite;
    }
    public void Interact(CR cr)
    {
        cr.Shooter.CurrentWeaponSlot = weaponSlotInfo;
    }
    public void Launch(float power = 2.5f, float duration = 0.6f)
    {
        var direction = (Vector3)Random.insideUnitCircle.normalized;
        var fixedPower = power * Random.Range(0.85f, 1.15f);
        var destination = transform.position + direction * fixedPower;

        var blockRay = Physics2D.Raycast(transform.position, direction, power, LayerMask.GetMask(blockingLayers));
        if (blockRay) destination = (Vector3)blockRay.point - direction * power / 7;

        currentLaunchTween = transform.DOMove(destination, duration).SetEase(Ease.OutCirc);
    }
}
