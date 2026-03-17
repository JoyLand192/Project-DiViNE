using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityMovement : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }
    [SerializeField] protected bool isMovable;
    public bool IsMovable
    {
        get => isMovable;
        set => isMovable = value;
    }
}
