using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponDropDisplayer : InteractionDisplayer
{
    const float duration = 0.38f;
    const Ease ease = Ease.InOutQuad;
    [SerializeField] Vector3 defaultCanvasOffset;
    [SerializeField] Canvas displayerCanvas;
    [SerializeField] RectTransform container;
    [SerializeField] TextMeshProUGUI label;
    Vector3 originPos;
    Tween currentTween;
    public Vector3 CanvasPosition
    {
        get => displayerCanvas.transform.position;
        set => displayerCanvas.transform.position = value + defaultCanvasOffset;
    }
    public override bool TweenExist => currentTween != null;
    public string LabelText
    {
        get => label.text;
        set
        {
            label.text = value;
        }
    }
    void Awake()
    {
        if (container == null) return;
        originPos = container.anchoredPosition; 
    }
    public override void Initialize()
    {
        container.anchoredPosition = originPos;
        container.gameObject.SetActive(true);
        currentTween?.Kill();
        currentTween = container.DOAnchorPosY(0, duration)
            .SetEase(ease)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public override void Pause()
    {
        container.gameObject.SetActive(false);
        currentTween?.Pause();
    }

    public override void Resume()
    {
        container?.gameObject.SetActive(true);
        currentTween?.Play();
    }

    public override void Stop()
    {
        container.gameObject.SetActive(false);
        if (currentTween != null)
        {
            currentTween.Kill();
            currentTween = null;
            container.anchoredPosition = originPos;
        }
    }
}
