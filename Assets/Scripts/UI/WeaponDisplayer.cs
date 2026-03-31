using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDisplayer : MonoBehaviour
{
    [SerializeField] List<Image> slots = new();
    [SerializeField] RectTransform currentSlotUI;
    [SerializeField] Color disabledWeaponColor;
    [SerializeField] Ease slotMoveEase;
    [SerializeField] float disableWeaponColorFadeTime;
    [SerializeField] float slotDistance;
    [SerializeField] float slotMoveDuration;
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
