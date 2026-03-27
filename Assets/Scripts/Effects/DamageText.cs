using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

public class DamageText : MonoBehaviour
{
    [SerializeField] TextMeshPro tmp;
    public TextMeshPro TMP
    {
        get => tmp;
        set => tmp = value;
    }
    [SerializeField] Material normalMaterial;
    [SerializeField] Material criticalMaterial;
    [SerializeField] float jumpPower;
    [SerializeField] float jumpHeight;
    [SerializeField] float duration;
    bool isCritical;
    public bool IsCritical
    {
        get => isCritical;
        set
        {
            isCritical = value;
            TMP.fontMaterial = value ? criticalMaterial : normalMaterial;
            TMP.fontSize = value ? 10 : 7;
        }
    }
    Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void SetText(float damage) { if (TMP != null) TMP.text = $"{damage:00}"; }
    public void SetSize(float size) => transform.localScale = Vector3.one * size;
    public async UniTask Jump(Vector3 pos, DamageTextPool pool)
    {
        transform.position = pos;

        rb.velocity = Vector3.zero;
        rb.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f)).normalized * jumpPower + Vector2.up * jumpHeight, ForceMode2D.Impulse);

        DOTween.Sequence().AppendInterval(duration / 2f).Append(transform.DOScale(0, duration / 2f).SetEase(Ease.InCubic));
        await UniTask.Delay(System.TimeSpan.FromSeconds(duration));

        if (pool == null)
        {
            Destroy(gameObject);
            return;
        }
        pool.Return(this);
    }
}
