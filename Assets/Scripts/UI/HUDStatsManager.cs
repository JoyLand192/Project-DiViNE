using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDStatsManager : MonoBehaviour
{
    [SerializeField] float hpWarnThreshold = 0.3f;
    [SerializeField] Image hpIcon;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] CRStatus cr;
    [SerializeField] Color hpHurtColor;
    [SerializeField] Color hpRegenColor;
    RectTransform hpIconRect;
    RectTransform hpTextRect;
    Color hpIconOriginColor;
    Color hpTextOriginColor;
    Vector3 hpIconOriginPos;
    void OnEnable()
    {
        hpIconRect = hpIcon.transform as RectTransform;
        hpTextRect = hpText.transform as RectTransform;

        hpIconOriginPos = hpIconRect.anchoredPosition;
        hpTextOriginColor = hpText.color;
        hpIconOriginColor = hpIcon.color;

        cr.OnHPChanged += UpdateHP;
    }
    public void UpdateHP(float diff)
    {
        hpText.text = $"{cr.HP:0}";

        var heartColor = cr.HP / cr.MaxHP > hpWarnThreshold ? hpIconOriginColor : hpHurtColor;
        var textColor = cr.HP / cr.MaxHP > hpWarnThreshold ? hpTextOriginColor : hpHurtColor;

        if (diff < 0)
        {
            float intensity = Mathf.InverseLerp(0, cr.MaxHP * -0.3f, diff) + Mathf.InverseLerp(cr.MaxHP * -0.3f, cr.MaxHP * -1f, diff);

            hpIconRect.DOKill(true);
            hpTextRect.DOKill(true);
            hpIcon.DOKill(true);
            hpText.DOKill(true);

            hpIcon.color = hpHurtColor;
            hpText.color = hpHurtColor;
            hpIcon.DOColor(heartColor, 0.4f);
            hpText.DOColor(textColor, 0.4f);

            hpIconRect.DOShakeAnchorPos(duration: 0.4f, strength: intensity * 15, vibrato: 20, fadeOut: true);
            hpTextRect.DOShakeAnchorPos(duration: 0.4f, strength: intensity * 10, vibrato: 20, fadeOut: true);
        }
        else if (diff > 0)
        {
            float intensity = Mathf.InverseLerp(0, cr.MaxHP * 0.3f, diff) + Mathf.InverseLerp(cr.MaxHP * -0.3f, cr.MaxHP * -1f, diff);

            hpIconRect.DOKill(true);
            hpTextRect.DOKill(true);
            hpIcon.DOKill(true);
            hpText.DOKill(true);

            hpIcon.color = hpRegenColor;
            hpText.color = hpRegenColor;
            hpIcon.DOColor(heartColor, 0.4f);
            hpText.DOColor(textColor, 0.4f);

            hpIconRect.DOShakeAnchorPos(duration: 0.4f, strength: intensity * 12, vibrato: 15, fadeOut: true);
            hpTextRect.DOShakeAnchorPos(duration: 0.4f, strength: intensity * 8, vibrato: 15, fadeOut: true);
        }
    }
}
