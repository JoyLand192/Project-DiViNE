using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityStatus : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    public virtual float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }
    [SerializeField] protected float maxHP;
    public float MaxHP
    {
        get => maxHP; 
        set => maxHP = value;
    }
    [SerializeField] protected float hp;
    public float HP
    {
        get => hp; 
        set
        {
            if (value > maxHP)
            {
                // 초과량 보호막으로 전환 등
            }

            hp = Mathf.Clamp(value, 0, maxHP);
            if (value <= 0) Destroy(gameObject);
        }
    }
    public void TakeDamage(float damage)
    {
        HP -= damage;
    }
}
