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
    [SerializeField] protected float hp;
    public float HP
    {
        get => hp; 
        set
        {
            hp = Mathf.Max(0, value);
        }
    }
    public void TakeDamage(float damage)
    {
        HP -= damage;
    }
}
