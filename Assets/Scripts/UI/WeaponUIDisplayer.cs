using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using TMPro;

public class WeaponUIDisplayer : MonoBehaviour
{
    [SerializeField] RectTransform magazineLinesContainer;
    [SerializeField] Image magazineLinePrefab;
    [SerializeField] List<Image> lines = new();
    [SerializeField] List<Image> slots = new();
    [SerializeField] RectTransform currentSlotUI;
    [SerializeField] Image reloadCircle;
    [SerializeField] Image magazineBar;
    [SerializeField] TextMeshProUGUI ammoLeftText;
    [SerializeField] TextMeshProUGUI magazineLeftText;
    [SerializeField] Color disabledWeaponColor;
    [SerializeField] Color disabledMagazineColor;
    [SerializeField] Ease slotMoveEase;
    [SerializeField] float disableWeaponColorFadeTime;
    [SerializeField] float slotDistance;
    [SerializeField] float slotMoveDuration;
    Color defaultMagazineColor;
    CancellationTokenSource reloadCTS;
    Sequence currentReloadSeq;
    RectTransform ammoLeftTextBody;
    void Awake()
    {
        reloadCTS = new();
        defaultMagazineColor = magazineBar.color;

        ammoLeftTextBody = ammoLeftText.transform as RectTransform;
    }
    void OnShootHandler(WeaponSlot weaponSlot)
    {
        ammoLeftTextBody.DOKill(true);
        ammoLeftTextBody.DOShakeAnchorPos(0.25f, 3, 30);
        UpdateMagazine(weaponSlot);
    }
    void UpdateMagazine(WeaponSlot weaponSlot)
    {
        magazineBar.DOKill(true);
        magazineBar.DOFillAmount((float)weaponSlot.AmmoLeft / weaponSlot.Weapon.AmmoCount, 0.16f).SetEase(Ease.OutExpo);
        ammoLeftText.text = $"{weaponSlot.AmmoLeft}";
        magazineLeftText.text = $"{weaponSlot.MagazineLeft}";

        for (int i = 0; i < weaponSlot.Weapon.AmmoCount; i++)
        {
            if (i >= lines.Count)
            {
                Image newLine = Instantiate(magazineLinePrefab, magazineLinesContainer);
                lines.Add(newLine);
            }
            lines[i].gameObject.SetActive(true);
        }
        for (int i = weaponSlot.Weapon.AmmoCount; i < lines.Count; i++) lines[i].gameObject.SetActive(false);
    }
    void OnReloadHandler(WeaponSlot slot, System.Action callback) => ReloadAnimation(slot, callback).Forget();
    async UniTask ReloadAnimation(WeaponSlot weaponSlot, System.Action callback)
    {
        if (reloadCTS != null) CancelReload(weaponSlot);
        reloadCTS = new();

        try
        {
            magazineBar.DOKill(true);
            magazineBar.color = disabledMagazineColor;
            magazineBar.fillAmount = 0;
            reloadCircle.fillAmount = 0;
            reloadCircle.gameObject.SetActive(true);

            currentReloadSeq = DOTween.Sequence();

            await currentReloadSeq
                .Append(magazineBar.DOFillAmount(1, weaponSlot.Weapon.ReloadTime).SetEase(Ease.Linear))
                .Join(reloadCircle.DOFillAmount(1, weaponSlot.Weapon.ReloadTime).SetEase(Ease.Linear))
                .AppendCallback(() => callback?.Invoke());
        }
        catch (OperationCanceledException)
        {

        }
        finally
        {
            magazineBar.color = defaultMagazineColor;
            reloadCircle.gameObject.SetActive(false);
            CancelReload(weaponSlot);
        }
    }
    public void Initialize(CRShooter shooter)
    {
        shooter.OnShoot += OnShootHandler;
        shooter.OnWeaponChanged += UpdateMagazine;
        shooter.OnReload += OnReloadHandler;

        UpdateMagazine(shooter.CurrentWeaponSlot);
    }
    public void CancelReload(WeaponSlot weaponSlot)
    {
        currentReloadSeq?.Kill(true);
        currentReloadSeq = null;

        reloadCTS?.Cancel();
        reloadCTS?.Dispose();
        reloadCTS = null;

        UpdateMagazine(weaponSlot);
    }
    public void SetCurrentSlot(int index)
    {
        slots[(index + 1) % 3].DOColor(disabledWeaponColor, disableWeaponColorFadeTime);
        slots[(index + 2) % 3].DOColor(disabledWeaponColor, disableWeaponColorFadeTime);
        slots[index].DOColor(Color.white, disableWeaponColorFadeTime);

        currentSlotUI.DOAnchorPosX((index - 1) * slotDistance, slotMoveDuration).SetEase(slotMoveEase);
    }
    public void UpdateWeaponImage(Weapon weapon, int index)
    {
        if (weapon == null)
        {
            slots[index].gameObject.SetActive(false);
            return;
        }
        else slots[index].gameObject.SetActive(true);

        slots[index].sprite = weapon.Sprite;
    }
}
