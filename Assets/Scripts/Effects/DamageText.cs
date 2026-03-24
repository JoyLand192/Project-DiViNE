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
    [SerializeField] float jumpPower;
    [SerializeField] float duration;
    Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void SetText(float damage) { if (TMP != null) TMP.text = $"{damage}"; }
    public void SetSize(float size) => transform.localScale = Vector3.one * size;
    public async UniTask Jump(Vector3 pos, DamageTextPool pool)
    {
        transform.position = pos;

        rb.velocity = Vector3.zero;
        rb.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(0.4f, 1.4f)).normalized * jumpPower, ForceMode2D.Impulse);

        transform.DOScale(0, duration).SetEase(Ease.InCubic);
        await UniTask.Delay(System.TimeSpan.FromSeconds(duration));

        if (pool == null)
        {
            Destroy(gameObject);
            return;
        }
        pool.Return(this);
    }
}
