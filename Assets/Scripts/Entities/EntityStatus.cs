using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class EntityStatus : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    public virtual float MoveSpeed
    {
        get => moveSpeed;
        set
        {
            moveSpeed = value;
        }
    }
    [SerializeField] protected float maxHP;
    public virtual float MaxHP
    {
        get => maxHP;
        set => maxHP = value;
    }
    [SerializeField] protected float hp;
    public virtual float HP
    {
        get => hp;
        set
        {
            if (value > maxHP)
            {
                // 초과량 보호막으로 전환 등
            }

            hp = Mathf.Clamp(value, 0, maxHP);
        }
    }
    public virtual void TakeDamage(float damage, DamageTextPool dtPool)
    {
        var dmg = Random.Range(0.8f, 1.4f) * damage;

        if (dtPool == null) dtPool = FindAnyObjectByType<DamageTextPool>();

        var dt = dtPool.Get();

        //DEBUG
        dt.IsTargetCR = this is CRStatus;
        dt.IsCritical = dmg >= 1000;

        dt.SetText(dmg);
        dt.SetSize(1f + Mathf.InverseLerp(5, 500, dmg) * 1.5f + Mathf.InverseLerp(500, 5000, dmg) * 0.5f);
        dt.gameObject.SetActive(true);
        dt.Jump(transform.position, dtPool).Forget();

        HP -= dmg;
    }
}
