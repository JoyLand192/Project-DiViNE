using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngineInternal;

public class WeaponDrop : MonoBehaviour, IInteraction, ILaunchable
{
    [SerializeField] WeaponDropDisplayer interactionDisplayer;
    [SerializeField] SpriteRenderer graphic;
    readonly string[] blockingLayers = { "HardWall" }; // TODO : Add BreakableWall layer and include it here
    WeaponSlot weaponSlotInfo;
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
            if (interactionDisplayer.TweenExist)
            {
                if (value) interactionDisplayer.Resume();
                else interactionDisplayer.Pause();
            }
            else if (value) interactionDisplayer.Initialize();
        }
    }
    Tween currentLaunchTween;
    public Tween CurrentLaunchTween => currentLaunchTween;
    bool isDestroyed;

    public bool IsDestroyed 
    { 
        get => isDestroyed; 
        set => isDestroyed = value; 
    }
    public event System.Action<IInteraction> OnDestroyed;
    void OnDestroy()
    {
        interactionDisplayer.Stop();
        OnDestroyed?.Invoke(this);
        IsDestroyed = true;
    }
    public void Initialize(Weapon weapon)
    {
        weaponSlotInfo = new WeaponSlot(weapon);
        graphic.sprite = weapon.Sprite;
        interactionDisplayer.LabelText = $"{weapon.WeaponName}";
        interactionDisplayer.CanvasPosition = graphic.bounds.center;
    }
    public void Interact(CR cr)
    {
        var swapped = cr.Shooter.GainWeapon(weaponSlotInfo);
        if (swapped?.Weapon == null)
        {
            Destroy(gameObject);
            return;
        }
        weaponSlotInfo = swapped;
        graphic.sprite = weaponSlotInfo.Weapon.Sprite;
        interactionDisplayer.LabelText = $"{weaponSlotInfo.Weapon.WeaponName}";
        interactionDisplayer.CanvasPosition = graphic.bounds.center;

        transform.position = cr.transform.position;

        Launch(power: 1.2f, duration: 0.4f);
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
