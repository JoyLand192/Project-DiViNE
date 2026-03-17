using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer graphic;
    protected Rigidbody2D rb;
    protected Vector2 directionVector;
    public Vector2 DirectionVector
    {
        get => directionVector;
        set => directionVector = value;
    }
    protected float moveSpeed;
    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }
    protected bool isMoving;
    public bool IsMoving
    {
        get => isMoving;
        set => isMoving = value;
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Launch()
    {
        IsMoving = true;
        rb.velocity = DirectionVector * MoveSpeed;
    }
}
